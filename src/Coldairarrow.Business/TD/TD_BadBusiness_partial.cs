using Coldairarrow.Business.IT;
using Coldairarrow.Business.PB;
using Coldairarrow.Entity.IT;
using Coldairarrow.Entity.PB;
using Coldairarrow.Entity.TD;
using Coldairarrow.IBusiness.DTO;
using Coldairarrow.Util;
using EFCore.Sharding;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Coldairarrow.Business.TD
{
    public partial class TD_BadBusiness : BaseBusiness<TD_Bad>, ITD_BadBusiness, ITransientDependency
    {
        IServiceProvider _ServiceProvider { get; }
        public TD_BadBusiness(IDbAccessor db, IServiceProvider svcProvider)
            : base(db)
        {
            _ServiceProvider = svcProvider;
        }

        public async Task<PageResult<TD_Bad>> GetDataListAsync(TD_BadPageInput input)
        {
            var q = GetIQueryable()
                .Include(i => i.CreateUser)
                .Include(i => i.AuditUser)
                .Where(w => w.StorId == input.StorId);
            var where = LinqHelper.True<TD_Bad>();
            var search = input.Search;

            if (!search.Code.IsNullOrEmpty())
                where = where.And(w => w.Code.Contains(search.Code) || w.RefCode.Contains(search.Code));
            if (!search.Type.IsNullOrEmpty())
                where = where.And(w => w.Type == search.Type);
            if (search.BadTimeStart.HasValue)
                where = where.And(w => w.BadTime >= search.BadTimeStart.Value);
            if (search.BadTimeEnd.HasValue)
                where = where.And(w => w.BadTime <= search.BadTimeEnd.Value);
            if (search.Status.HasValue)
                where = where.And(w => w.Status == search.Status.Value);

            return await q.Where(where).GetPageResultAsync(input);
        }

        public async Task<TD_Bad> GetTheDataAsync(string id)
        {
            return await this.GetIQueryable()
                .Include(i => i.BadDetails)
                    .ThenInclude(t => t.FromLocal)
                .Include(i => i.BadDetails)
                    .ThenInclude(t => t.Material)
                .Include(i => i.BadDetails)
                    .ThenInclude(t => t.Tray)
                .Include(i => i.BadDetails)
                    .ThenInclude(t => t.TrayZone)
                .SingleOrDefaultAsync(w => w.Id == id);
        }

        [DataAddLog(UserLogType.报损管理, "Code", "报损单")]
        [Transactional]
        public async Task AddDataAsync(TD_Bad data)
        {
            if (data.Code.IsNullOrEmpty())
            {
                var codeSvc = _ServiceProvider.GetRequiredService<IPB_BarCodeTypeBusiness>();
                data.Code = await codeSvc.Generate("TD_Bad");
            }
            data.BadNum = data.BadDetails.Sum(s => s.BadNum);
            data.TotalAmt = data.BadDetails.Sum(s => s.Amount);
            await InsertAsync(data);
        }

        [DataEditLog(UserLogType.报损管理, "Code", "报损单")]
        [Transactional]
        public async Task UpdateDataAsync(TD_Bad data)
        {
            var curDetail = data.BadDetails;
            var listDetail = await Db.GetIQueryable<TD_BadDetail>().Where(w => w.BadId == data.Id).ToListAsync();

            var curIds = curDetail.Select(s => s.Id).ToList();
            var dbIds = listDetail.Select(s => s.Id).ToList();
            var deleteIds = dbIds.Except(curIds).ToList();
            var detailSvc = _ServiceProvider.GetRequiredService<ITD_BadDetailBusiness>();
            if (deleteIds.Count > 0)
                await detailSvc.DeleteDataAsync(deleteIds);

            var addIds = curIds.Except(dbIds).ToList();
            if (addIds.Count > 0)
            {
                var listAdds = curDetail.Where(w => addIds.Contains(w.Id)).ToList();
                await detailSvc.AddDataAsync(listAdds);
            }

            var updateIds = curIds.Except(addIds).ToList();
            if (updateIds.Count > 0)
            {
                var listUpdates = curDetail.Where(w => updateIds.Contains(w.Id)).ToList();
                await detailSvc.UpdateDataAsync(listUpdates);
            }

            data.BadNum = data.BadDetails.Sum(s => s.BadNum);
            data.TotalAmt = data.BadDetails.Sum(s => s.Amount);

            await UpdateAsync(data);
        }


        [DataEditLog(UserLogType.报损管理, "Id", "报损单审批")]
        [Transactional]
        public async Task Approve(AuditDTO audit)
        {
            var now = DateTime.Now;
            var data = await this.GetTheDataAsync(audit.Id);
            var detail = data.BadDetails;
            var dicMUnit = detail.GroupBy(s => new { s.Material.Id, s.Material.MeasureId }).Select(s => new { s.Key.Id, s.Key.MeasureId }).ToDictionary(k => k.Id, v => v.MeasureId);

            PB_Location defaultBadLocation = null;
            // 找到默认的报损货位
            {
                var localSvc = Db.GetIQueryable<PB_Location>();
                //这里要修改，要从货区的类型来过滤
                defaultBadLocation = await localSvc.Where(w => w.StorId == audit.StorId && w.PB_StorArea.Type == "Bad").OrderByDescending(o => o.IsDefault).FirstOrDefaultAsync();
                if (defaultBadLocation == null) throw new Exception("没有指定默认报损货位");
            }

            var lmSvc = _ServiceProvider.GetRequiredService<IIT_LocalMaterialBusiness>();
            var ldSvc = _ServiceProvider.GetRequiredService<IIT_LocalDetailBusiness>();

            // 原库位出库
            {
                var badGroup = detail
                    .GroupBy(w => new { w.FromLocalId, w.TrayId, w.ZoneId, w.MaterialId, w.BatchNo, w.BarCode })
                    .Select(s => new { s.Key.FromLocalId, s.Key.TrayId, s.Key.ZoneId, s.Key.MaterialId, s.Key.BatchNo, s.Key.BarCode, BadNum = s.Sum(o => o.BadNum) })
                    .ToList();
                var localIds = badGroup.Select(s => s.FromLocalId).ToList();
                var trayIds = badGroup.Select(s => s.TrayId).ToList();
                var zoneIds = badGroup.Select(s => s.ZoneId).ToList();
                var materialIds = badGroup.Select(s => s.MaterialId).ToList();
                var batchNos = badGroup.Select(s => s.BatchNo).ToList();
                var barCodes = badGroup.Select(s => s.BarCode).ToList();

                //修改库存
                {
                    var lmQuery = Db.GetIQueryable<IT_LocalMaterial>();
                    if (localIds.Count > 0)
                        lmQuery = lmQuery.Where(w => localIds.Contains(w.LocalId));
                    if (trayIds.Count > 0)
                        lmQuery = lmQuery.Where(w => trayIds.Contains(w.TrayId));
                    if (zoneIds.Count > 0)
                        lmQuery = lmQuery.Where(w => zoneIds.Contains(w.ZoneId));
                    if (materialIds.Count > 0)
                        lmQuery = lmQuery.Where(w => materialIds.Contains(w.MaterialId));
                    if (batchNos.Count > 0)
                        lmQuery = lmQuery.Where(w => batchNos.Contains(w.BatchNo));
                    if (barCodes.Count > 0)
                        lmQuery = lmQuery.Where(w => barCodes.Contains(w.BarCode));

                    var listLM = await lmQuery.ToListAsync();
                    var listDel = new List<IT_LocalMaterial>();
                    var listUpdate = new List<IT_LocalMaterial>();
                    var listRB = new List<IT_RecordBook>();
                    foreach (var bad in badGroup)
                    {
                        var lm = listLM.Where(w => w.StorId == audit.StorId && w.LocalId == bad.FromLocalId && w.TrayId == bad.TrayId && w.ZoneId == bad.ZoneId && w.MaterialId == bad.MaterialId && w.BatchNo == bad.BatchNo && w.BarCode == bad.BarCode).SingleOrDefault();
                        if (lm == null || lm.Num < bad.BadNum) throw new Exception($"没有找到对应物料的库存数据/库存数量不够({bad.BadNum})");
                        if (lm.Num == bad.BadNum) listDel.Add(lm);
                        else if (lm.Num > bad.BadNum)
                        {
                            lm.Num -= bad.BadNum;
                            listUpdate.Add(lm);
                        }
                        // 增加（报损-出）BadOut台帐
                        var rbOut = new IT_RecordBook()
                        {
                            Id = IdHelper.GetId(),
                            RefCode = data.Code,
                            Type = "BadOut",
                            FromStorId = audit.StorId,
                            FromLocalId = bad.FromLocalId,
                            ToStorId = audit.StorId,
                            ToLocalId = defaultBadLocation.Id,
                            MaterialId = bad.MaterialId,
                            MeasureId = dicMUnit[bad.MaterialId],
                            BarCode = bad.BarCode,
                            BatchNo = bad.BatchNo,
                            Num = bad.BadNum,
                            CreateTime = audit.AuditTime,
                            CreatorId = audit.AuditUserId,
                            Deleted = false
                        };
                        listRB.Add(rbOut);
                        // 增加（报损-入）BadOut台帐
                        var rbIn = new IT_RecordBook()
                        {
                            Id = IdHelper.GetId(),
                            RefCode = data.Code,
                            Type = "BadIn",
                            FromStorId = audit.StorId,
                            FromLocalId = bad.FromLocalId,
                            ToStorId = audit.StorId,
                            ToLocalId = defaultBadLocation.Id,
                            MaterialId = bad.MaterialId,
                            MeasureId = dicMUnit[bad.MaterialId],
                            BarCode = bad.BarCode,
                            BatchNo = bad.BatchNo,
                            Num = bad.BadNum,
                            CreateTime = audit.AuditTime,
                            CreatorId = audit.AuditUserId,
                            Deleted = false
                        };
                        listRB.Add(rbIn);
                    }
                    if (listDel.Count > 0) await lmSvc.DeleteDataAsync(listDel);
                    if (listUpdate.Count > 0) await lmSvc.UpdateDataAsync(listUpdate);

                    // 保存数据台帐
                    var rbSvc = _ServiceProvider.GetRequiredService<IIT_RecordBookBusiness>();
                    await rbSvc.AddDataAsync(listRB);
                }

                // 修改库存明细
                {
                    var ldQuery = Db.GetIQueryable<IT_LocalDetail>();
                    if (localIds.Count > 0)
                        ldQuery = ldQuery.Where(w => localIds.Contains(w.LocalId));
                    if (trayIds.Count > 0)
                        ldQuery = ldQuery.Where(w => trayIds.Contains(w.TrayId));
                    if (zoneIds.Count > 0)
                        ldQuery = ldQuery.Where(w => zoneIds.Contains(w.ZoneId));
                    if (materialIds.Count > 0)
                        ldQuery = ldQuery.Where(w => materialIds.Contains(w.MaterialId));
                    if (batchNos.Count > 0)
                        ldQuery = ldQuery.Where(w => batchNos.Contains(w.BatchNo));
                    if (barCodes.Count > 0)
                        ldQuery = ldQuery.Where(w => barCodes.Contains(w.BarCode));

                    var listLD = await ldQuery.ToListAsync();

                    var listDel = new List<IT_LocalDetail>();
                    var listUpdate = new List<IT_LocalDetail>();
                    foreach (var bad in badGroup)
                    {
                        var badNum = bad.BadNum;
                        var lds = listLD.Where(w => w.StorId == audit.StorId && w.LocalId == bad.FromLocalId && w.TrayId == bad.TrayId && w.ZoneId == bad.ZoneId && w.MaterialId == bad.MaterialId && w.BatchNo == bad.BatchNo && w.BarCode == bad.BarCode).OrderBy(o => o.InTime).ToList();
                        if (lds.Sum(s => s.Num) < badNum) throw new Exception($"库存(明细)数量不够({bad.BadNum})");
                        foreach (var item in lds)
                        {
                            if (item.Num <= badNum)
                            {
                                listDel.Add(item);
                                badNum -= item.Num.GetValueOrDefault(0);
                            }
                            else
                            {
                                item.Num -= badNum;
                                listUpdate.Add(item);
                                badNum = 0;
                            }
                            if (badNum == 0) break;
                        }
                    }
                    if (listDel.Count > 0) await ldSvc.DeleteDataAsync(listDel);
                    if (listUpdate.Count > 0) await ldSvc.UpdateDataAsync(listUpdate);
                }
            }

            //默认报损库位入库
            {
                var badGroup = detail
                    .GroupBy(w => new { w.MaterialId, w.BatchNo, w.BarCode })
                    .Select(s => new { s.Key.MaterialId, s.Key.BatchNo, s.Key.BarCode, BadNum = s.Sum(o => o.BadNum) })
                    .ToList();
                var materialIds = badGroup.Select(s => s.MaterialId).ToList();
                var batchNos = badGroup.Select(s => s.BatchNo).ToList();
                var barCodes = badGroup.Select(s => s.BarCode).ToList();
                // 修改库存
                {
                    var lmQuery = Db.GetIQueryable<IT_LocalMaterial>().Where(w => w.StorId == audit.StorId && w.LocalId == defaultBadLocation.Id);
                    if (materialIds.Count > 0)
                        lmQuery = lmQuery.Where(w => materialIds.Contains(w.MaterialId));
                    if (batchNos.Count > 0)
                        lmQuery = lmQuery.Where(w => batchNos.Contains(w.BatchNo));
                    if (barCodes.Count > 0)
                        lmQuery = lmQuery.Where(w => barCodes.Contains(w.BarCode));
                    var listLM = await lmQuery.ToListAsync();

                    var listAdd = new List<IT_LocalMaterial>();
                    var listUpdate = new List<IT_LocalMaterial>();
                    foreach (var bad in badGroup)
                    {
                        var lm = listLM.Where(w => w.StorId == audit.StorId && w.LocalId == defaultBadLocation.Id && w.MaterialId == bad.MaterialId && w.BatchNo == bad.BatchNo && w.BarCode == bad.BarCode).SingleOrDefault();
                        if (lm != null)
                        {
                            lm.Num += bad.BadNum;
                            listUpdate.Add(lm);
                        }
                        else
                        {
                            lm = new IT_LocalMaterial()
                            {
                                Id = IdHelper.GetId(),
                                StorId = audit.StorId,
                                LocalId = defaultBadLocation.Id,
                                MaterialId = bad.MaterialId,
                                MeasureId = dicMUnit[bad.MaterialId],
                                BatchNo = bad.BatchNo,
                                BarCode = bad.BarCode,
                                Num = bad.BadNum
                            };
                            listAdd.Add(lm);
                        }
                        
                    }
                    if (listAdd.Count > 0) await lmSvc.AddDataAsync(listAdd);
                    if (listUpdate.Count > 0) await lmSvc.UpdateDataAsync(listUpdate);
                }

                // 增加库存明细
                {
                    var listAdd = new List<IT_LocalDetail>();
                    foreach (var bad in detail)
                    {
                        var ld = new IT_LocalDetail();
                        ld.Id = IdHelper.GetId();
                        ld.StorId = audit.StorId;
                        ld.LocalId = defaultBadLocation.Id;
                        ld.MaterialId = bad.MaterialId;
                        ld.MeasureId = dicMUnit[bad.MaterialId];
                        ld.BatchNo = bad.BatchNo;
                        ld.BarCode = bad.BarCode;
                        ld.InTime = audit.AuditTime;
                        ld.Amount = bad.Amount;
                        ld.CreateTime = now;
                        ld.CreatorId = audit.AuditUserId;
                        ld.Price = bad.Price;
                        ld.Num = bad.BadNum;
                        ld.Deleted = false;
                        listAdd.Add(ld);
                    }
                    if (listAdd.Count > 0)
                    {
                        var localdetailSvc = _ServiceProvider.GetRequiredService<IIT_LocalDetailBusiness>();
                        await localdetailSvc.AddDataAsync(listAdd);
                    }
                }
            }

            // 修改主数据状态
            {
                data.Status = 1;
                data.AuditeTime = audit.AuditTime;
                data.AuditUserId = audit.AuditUserId;
                await UpdateAsync(data);
            }
        }

        [DataEditLog(UserLogType.报损管理, "Id", "报损单驳回")]
        public async Task Reject(AuditDTO audit)
        {
            var data = await this.GetEntityAsync(audit.Id);
            // 修改主数据状态
            {
                data.Status = 2;
                data.AuditeTime = audit.AuditTime;
                data.AuditUserId = audit.AuditUserId;
                await UpdateAsync(data);
            }
        }
    }
}