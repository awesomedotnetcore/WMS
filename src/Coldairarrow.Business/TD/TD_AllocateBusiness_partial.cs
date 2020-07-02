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
    public partial class TD_AllocateBusiness : BaseBusiness<TD_Allocate>, ITD_AllocateBusiness, ITransientDependency
    {
        IServiceProvider _ServiceProvider { get; }
        public TD_AllocateBusiness(IDbAccessor db, IServiceProvider svcProvider)
            : base(db)
        {
            _ServiceProvider = svcProvider;
        }

        public async Task<PageResult<TD_Allocate>> GetDataListAsync(TD_AllocatePageInput input)
        {
            var q = GetIQueryable()
                .Include(i => i.ToStorage)
                .Include(i => i.CreateUser)
                .Include(i => i.AuditUser)
                .Where(w => w.StorId == input.StorId);
            var where = LinqHelper.True<TD_Allocate>();
            var search = input.Search;

            if (!search.Code.IsNullOrEmpty())
                where = where.And(w => w.Code.Contains(search.Code) || w.RefCode.Contains(search.Code));
            if (!search.Type.IsNullOrEmpty())
                where = where.And(w => w.Type == search.Type);
            if (!search.ToStorId.IsNullOrEmpty())
                where = where.And(w => w.ToStorId == search.ToStorId);
            if (search.AllocateTimeStart.HasValue)
                where = where.And(w => w.AllocateTime >= search.AllocateTimeStart.Value);
            if (search.AllocateTimeEnd.HasValue)
                where = where.And(w => w.AllocateTime <= search.AllocateTimeEnd.Value);
            if (search.Status.HasValue)
                where = where.And(w => w.Status == search.Status.Value);

            return await q.Where(where).GetPageResultAsync(input);
        }

        public async Task<TD_Allocate> GetTheDataAsync(string id)
        {
            return await this.GetIQueryable()
                .Include(i => i.AllocateDetails)
                    .ThenInclude(t => t.FromLocal)
                .Include(i => i.AllocateDetails)
                    .ThenInclude(t => t.Material)
                .Include(i => i.AllocateDetails)
                    .ThenInclude(t => t.FromTray)
                .Include(i => i.AllocateDetails)
                    .ThenInclude(t => t.FromZone)
                .SingleOrDefaultAsync(w => w.Id == id);
        }

        [DataAddLog(UserLogType.调拨管理, "Code", "调拨单")]
        [Transactional]
        public async Task AddDataAsync(TD_Allocate data)
        {
            if (data.Code.IsNullOrEmpty())
            {
                var codeSvc = _ServiceProvider.GetRequiredService<IPB_BarCodeTypeBusiness>();
                data.Code = await codeSvc.Generate("TD_Allocate");
            }
            data.AllocateNum = data.AllocateDetails.Sum(s => s.AllocateNum);
            data.Amount = data.AllocateDetails.Sum(s => s.Amount);
            await InsertAsync(data);
        }

        [DataEditLog(UserLogType.调拨管理, "Code", "调拨单")]
        [Transactional]
        public async Task UpdateDataAsync(TD_Allocate data)
        {
            var curDetail = data.AllocateDetails;
            var listDetail = await Db.GetIQueryable<TD_AllocateDetail>().Where(w => w.AllocateId == data.Id).ToListAsync();

            var curIds = curDetail.Select(s => s.Id).ToList();
            var dbIds = listDetail.Select(s => s.Id).ToList();
            var deleteIds = dbIds.Except(curIds).ToList();
            var detailSvc = _ServiceProvider.GetRequiredService<ITD_AllocateDetailBusiness>();
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

            data.AllocateNum = data.AllocateDetails.Sum(s => s.AllocateNum);
            data.Amount = data.AllocateDetails.Sum(s => s.Amount);

            await UpdateAsync(data);
        }


        [DataEditLog(UserLogType.调拨管理, "Id", "调拨单审批")]
        [Transactional]
        public async Task Approve(AuditDTO audit)
        {
            var now = DateTime.Now;
            var data = await this.GetTheDataAsync(audit.Id);
            var detail = data.AllocateDetails;
            var dicMUnit = detail.GroupBy(s => new { s.Material.Id, s.Material.MeasureId }).Select(s => new { s.Key.Id, s.Key.MeasureId }).ToDictionary(k => k.Id, v => v.MeasureId);

            PB_Location defaultAllocateLocation = null;
            // 找到目标仓库的默认待入库位
            {
                var localSvc = Db.GetIQueryable<PB_Location>();
                //这里要修改，要从货区的类型来过滤
                defaultAllocateLocation = await localSvc.Where(w => w.StorId == data.ToStorId && w.PB_StorArea.Type == "In").OrderByDescending(o => o.IsDefault).FirstOrDefaultAsync();
                if (defaultAllocateLocation == null) throw new Exception("没有找到目标仓库默认待入库位");
            }

            var lmSvc = _ServiceProvider.GetRequiredService<IIT_LocalMaterialBusiness>();
            var ldSvc = _ServiceProvider.GetRequiredService<IIT_LocalDetailBusiness>();

            // 原库位出库
            {
                var AllocateGroup = detail
                    .GroupBy(w => new { w.FromLocalId, w.FromTrayId, w.FromZoneId, w.MaterialId, w.BatchNo, w.BarCode })
                    .Select(s => new { s.Key.FromLocalId, s.Key.FromTrayId, s.Key.FromZoneId, s.Key.MaterialId, s.Key.BatchNo, s.Key.BarCode, AllocateNum = s.Sum(o => o.AllocateNum) })
                    .ToList();
                var localIds = AllocateGroup.Select(s => s.FromLocalId).ToList();
                var trayIds = AllocateGroup.Select(s => s.FromTrayId).ToList();
                var zoneIds = AllocateGroup.Select(s => s.FromZoneId).ToList();
                var materialIds = AllocateGroup.Select(s => s.MaterialId).ToList();
                var batchNos = AllocateGroup.Select(s => s.BatchNo).ToList();
                var barCodes = AllocateGroup.Select(s => s.BarCode).ToList();

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
                    foreach (var Allocate in AllocateGroup)
                    {
                        var lm = listLM.Where(w => w.StorId == audit.StorId && w.LocalId == Allocate.FromLocalId && w.TrayId == Allocate.FromTrayId && w.ZoneId == Allocate.FromZoneId && w.MaterialId == Allocate.MaterialId && w.BatchNo == Allocate.BatchNo && w.BarCode == Allocate.BarCode).SingleOrDefault();
                        if (lm == null || lm.Num < Allocate.AllocateNum) throw new Exception($"没有找到对应物料的库存数据/库存数量不够({Allocate.AllocateNum})");
                        if (lm.Num == Allocate.AllocateNum) listDel.Add(lm);
                        else if (lm.Num > Allocate.AllocateNum)
                        {
                            lm.Num -= Allocate.AllocateNum;
                            listUpdate.Add(lm);
                        }
                        // 增加（调拨-出）AllocateOut台帐
                        var rbOut = new IT_RecordBook()
                        {
                            Id = IdHelper.GetId(),
                            RefCode = data.Code,
                            Type = "AllocateOut",
                            FromStorId = audit.StorId,
                            FromLocalId = Allocate.FromLocalId,
                            ToStorId = data.ToStorId,
                            ToLocalId = defaultAllocateLocation.Id,
                            MaterialId = Allocate.MaterialId,
                            MeasureId = dicMUnit[Allocate.MaterialId],
                            BarCode = Allocate.BarCode,
                            BatchNo = Allocate.BatchNo,
                            Num = Allocate.AllocateNum,
                            CreateTime = audit.AuditTime,
                            CreatorId = audit.AuditUserId,
                            Deleted = false
                        };
                        listRB.Add(rbOut);
                        // 增加（调拨-入）AllocateIn台帐
                        var rbIn = new IT_RecordBook()
                        {
                            Id = IdHelper.GetId(),
                            RefCode = data.Code,
                            Type = "AllocateIn",
                            FromStorId = audit.StorId,
                            FromLocalId = Allocate.FromLocalId,
                            ToStorId = data.ToStorId,
                            ToLocalId = defaultAllocateLocation.Id,
                            MaterialId = Allocate.MaterialId,
                            MeasureId = dicMUnit[Allocate.MaterialId],
                            BarCode = Allocate.BarCode,
                            BatchNo = Allocate.BatchNo,
                            Num = Allocate.AllocateNum,
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
                    foreach (var Allocate in AllocateGroup)
                    {
                        var AllocateNum = Allocate.AllocateNum;
                        var lds = listLD.Where(w => w.StorId == audit.StorId && w.LocalId == Allocate.FromLocalId && w.TrayId == Allocate.FromTrayId && w.ZoneId == Allocate.FromZoneId && w.MaterialId == Allocate.MaterialId && w.BatchNo == Allocate.BatchNo && w.BarCode == Allocate.BarCode).OrderBy(o => o.InTime).ToList();
                        if (lds.Sum(s => s.Num) < AllocateNum) throw new Exception($"库存(明细)数量不够({Allocate.AllocateNum})");
                        foreach (var item in lds)
                        {
                            if (item.Num <= AllocateNum)
                            {
                                listDel.Add(item);
                                AllocateNum -= item.Num.GetValueOrDefault(0);
                            }
                            else
                            {
                                item.Num -= AllocateNum;
                                listUpdate.Add(item);
                                AllocateNum = 0;
                            }
                            if (AllocateNum == 0) break;
                        }
                    }
                    if (listDel.Count > 0) await ldSvc.DeleteDataAsync(listDel);
                    if (listUpdate.Count > 0) await ldSvc.UpdateDataAsync(listUpdate);
                }
            }

            //默认调拨库位入库
            {
                var AllocateGroup = detail
                    .GroupBy(w => new { w.MaterialId, w.BatchNo, w.BarCode })
                    .Select(s => new { s.Key.MaterialId, s.Key.BatchNo, s.Key.BarCode, AllocateNum = s.Sum(o => o.AllocateNum) })
                    .ToList();
                var materialIds = AllocateGroup.Select(s => s.MaterialId).ToList();
                var batchNos = AllocateGroup.Select(s => s.BatchNo).ToList();
                var barCodes = AllocateGroup.Select(s => s.BarCode).ToList();
                // 修改库存
                {
                    var lmQuery = Db.GetIQueryable<IT_LocalMaterial>().Where(w => w.StorId == data.ToStorId && w.LocalId == defaultAllocateLocation.Id);
                    if (materialIds.Count > 0)
                        lmQuery = lmQuery.Where(w => materialIds.Contains(w.MaterialId));
                    if (batchNos.Count > 0)
                        lmQuery = lmQuery.Where(w => batchNos.Contains(w.BatchNo));
                    if (barCodes.Count > 0)
                        lmQuery = lmQuery.Where(w => barCodes.Contains(w.BarCode));
                    var listLM = await lmQuery.ToListAsync();

                    var listAdd = new List<IT_LocalMaterial>();
                    var listUpdate = new List<IT_LocalMaterial>();
                    foreach (var Allocate in AllocateGroup)
                    {
                        var lm = listLM.Where(w => w.StorId == data.ToStorId && w.LocalId == defaultAllocateLocation.Id && w.MaterialId == Allocate.MaterialId && w.BatchNo == Allocate.BatchNo && w.BarCode == Allocate.BarCode).SingleOrDefault();
                        if (lm != null)
                        {
                            lm.Num += Allocate.AllocateNum;
                            listUpdate.Add(lm);
                        }
                        else
                        {
                            lm = new IT_LocalMaterial()
                            {
                                Id = IdHelper.GetId(),
                                StorId = data.ToStorId,
                                LocalId = defaultAllocateLocation.Id,
                                MaterialId = Allocate.MaterialId,
                                MeasureId = dicMUnit[Allocate.MaterialId],
                                BatchNo = Allocate.BatchNo,
                                BarCode = Allocate.BarCode,
                                Num = Allocate.AllocateNum
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
                    foreach (var Allocate in detail)
                    {
                        var ld = new IT_LocalDetail();
                        ld.Id = IdHelper.GetId();
                        ld.StorId = data.ToStorId;
                        ld.LocalId = defaultAllocateLocation.Id;
                        ld.MaterialId = Allocate.MaterialId;
                        ld.MeasureId = dicMUnit[Allocate.MaterialId];
                        ld.BatchNo = Allocate.BatchNo;
                        ld.BarCode = Allocate.BarCode;
                        ld.InTime = audit.AuditTime;
                        ld.Amount = Allocate.Amount;
                        ld.CreateTime = now;
                        ld.CreatorId = audit.AuditUserId;
                        ld.Price = Allocate.Price;
                        ld.Num = Allocate.AllocateNum;
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
                data.ToLocalId = defaultAllocateLocation.Id;
                data.AuditeTime = audit.AuditTime;
                data.AuditUserId = audit.AuditUserId;
                await UpdateAsync(data);
            }
        }

        [DataEditLog(UserLogType.调拨管理, "Id", "调拨单驳回")]
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