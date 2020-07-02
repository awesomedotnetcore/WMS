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
    public partial class TD_MoveBusiness : BaseBusiness<TD_Move>, ITD_MoveBusiness, ITransientDependency
    {
        IServiceProvider _ServiceProvider { get; }
        public TD_MoveBusiness(IDbAccessor db, IServiceProvider svcProvider)
            : base(db)
        {
            _ServiceProvider = svcProvider;
        }

        public async Task<PageResult<TD_Move>> GetDataListAsync(TD_MovePageInput input)
        {
            var q = GetIQueryable()
                .Include(i => i.CreateUser)
                .Include(i => i.AuditUser)
                .Where(w => w.StorId == input.StorId);
            var where = LinqHelper.True<TD_Move>();
            var search = input.Search;

            if (!search.Code.IsNullOrEmpty())
                where = where.And(w => w.Code.Contains(search.Code) || w.RefCode.Contains(search.Code));
            if (!search.Type.IsNullOrEmpty())
                where = where.And(w => w.Type == search.Type);
            if (search.BadTimeStart.HasValue)
                where = where.And(w => w.MoveTime >= search.BadTimeStart.Value);
            if (search.BadTimeEnd.HasValue)
                where = where.And(w => w.MoveTime <= search.BadTimeEnd.Value);
            if (search.Status.HasValue)
                where = where.And(w => w.Status == search.Status.Value);

            return await q.Where(where).GetPageResultAsync(input);
        }

        public async Task<TD_Move> GetTheDataAsync(string id)
        {
            return await this.GetIQueryable()
                .Include(i => i.MoveDetails)
                    .ThenInclude(t => t.Material)
                .Include(i => i.MoveDetails)
                    .ThenInclude(t => t.FromLocal)
                .Include(i => i.MoveDetails)
                    .ThenInclude(t => t.FromTray)
                .Include(i => i.MoveDetails)
                    .ThenInclude(t => t.FromZone)
                .Include(i => i.MoveDetails)
                    .ThenInclude(t => t.ToLocal)
                .Include(i => i.MoveDetails)
                    .ThenInclude(t => t.ToTray)
                .Include(i => i.MoveDetails)
                    .ThenInclude(t => t.ToZone)
                .SingleOrDefaultAsync(w => w.Id == id);
        }

        [DataAddLog(UserLogType.移库管理, "Code", "移库单")]
        [Transactional]
        public async Task AddDataAsync(TD_Move data)
        {
            if (data.Code.IsNullOrEmpty())
            {
                var codeSvc = _ServiceProvider.GetRequiredService<IPB_BarCodeTypeBusiness>();
                data.Code = await codeSvc.Generate("TD_Move");
            }
            data.MoveNum = data.MoveDetails.Sum(s => s.MoveNum);
            data.TotalAmt = data.MoveDetails.Sum(s => s.Amount);
            await InsertAsync(data);
        }

        [DataEditLog(UserLogType.移库管理, "Code", "移库单")]
        [Transactional]
        public async Task UpdateDataAsync(TD_Move data)
        {
            var curDetail = data.MoveDetails;
            var listDetail = await Db.GetIQueryable<TD_MoveDetail>().Where(w => w.MoveId == data.Id).ToListAsync();

            var curIds = curDetail.Select(s => s.Id).ToList();
            var dbIds = listDetail.Select(s => s.Id).ToList();
            var deleteIds = dbIds.Except(curIds).ToList();
            var detailSvc = _ServiceProvider.GetRequiredService<ITD_MoveDetailBusiness>();
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

            data.MoveNum = data.MoveDetails.Sum(s => s.MoveNum);
            data.TotalAmt = data.MoveDetails.Sum(s => s.Amount);

            await UpdateAsync(data);
        }


        [DataEditLog(UserLogType.移库管理, "Id", "移库单审批")]
        [Transactional]
        public async Task Approve(AuditDTO audit)
        {
            var now = DateTime.Now;
            var data = await this.GetTheDataAsync(audit.Id);
            var detail = data.MoveDetails;
            var dicMUnit = detail.GroupBy(s => new { s.Material.Id, s.Material.MeasureId }).Select(s => new { s.Key.Id, s.Key.MeasureId }).ToDictionary(k => k.Id, v => v.MeasureId);

            var lmSvc = _ServiceProvider.GetRequiredService<IIT_LocalMaterialBusiness>();
            var ldSvc = _ServiceProvider.GetRequiredService<IIT_LocalDetailBusiness>();

            // 原库位出库
            {
                var badGroup = detail
                    .GroupBy(w => new { w.FromLocalId, w.FromTrayId, w.FromZoneId, w.MaterialId, w.BatchNo, w.BarCode })
                    .Select(s => new { s.Key.FromLocalId, s.Key.FromTrayId, s.Key.FromZoneId, s.Key.MaterialId, s.Key.BatchNo, s.Key.BarCode, MoveNum = s.Sum(o => o.MoveNum) })
                    .ToList();
                var localIds = badGroup.Select(s => s.FromLocalId).ToList();
                var trayIds = badGroup.Select(s => s.FromTrayId).ToList();
                var zoneIds = badGroup.Select(s => s.FromZoneId).ToList();
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
                    foreach (var move in badGroup)
                    {
                        var lm = listLM.Where(w => w.StorId == audit.StorId && w.LocalId == move.FromLocalId && w.TrayId == move.FromTrayId && w.ZoneId == move.FromZoneId && w.MaterialId == move.MaterialId && w.BatchNo == move.BatchNo && w.BarCode == move.BarCode).SingleOrDefault();
                        if (lm == null || lm.Num < move.MoveNum) throw new Exception($"没有找到对应物料的库存数据/库存数量不够({move.MoveNum})");
                        if (lm.Num == move.MoveNum) listDel.Add(lm);
                        else if (lm.Num > move.MoveNum)
                        {
                            lm.Num -= move.MoveNum;
                            listUpdate.Add(lm);
                        }
                    }
                    if (listDel.Count > 0) await lmSvc.DeleteDataAsync(listDel);
                    if (listUpdate.Count > 0) await lmSvc.UpdateDataAsync(listUpdate);
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
                        var badNum = bad.MoveNum;
                        var lds = listLD.Where(w => w.StorId == audit.StorId && w.LocalId == bad.FromLocalId && w.TrayId == bad.FromTrayId && w.ZoneId == bad.FromZoneId && w.MaterialId == bad.MaterialId && w.BatchNo == bad.BatchNo && w.BarCode == bad.BarCode).OrderBy(o => o.InTime).ToList();
                        if (lds.Sum(s => s.Num) < badNum) throw new Exception($"库存(明细)数量不够({bad.MoveNum})");
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

            //目标库位入库
            {
                // 增加库存
                {
                    var lmGrout = detail
                        .GroupBy(g => new { g.StorId, g.ToLocalId, g.ToTrayId, g.ToZoneId, g.MaterialId, g.BatchNo, g.BarCode })
                        .Select(s => new { s.Key.StorId, s.Key.ToLocalId, s.Key.ToTrayId, s.Key.ToZoneId, s.Key.MaterialId, s.Key.BatchNo, s.Key.BarCode, MoveNum = s.Sum(o => o.MoveNum) })
                        .ToList();
                    //查询当前库存记录
                    var localIds = lmGrout.Select(s => s.ToLocalId).ToList();
                    var trayIds = lmGrout.Select(s => s.ToTrayId).ToList();
                    var zoneIds = lmGrout.Select(s => s.ToZoneId).ToList();
                    var materIds = lmGrout.Select(s => s.MaterialId).ToList();
                    var batchNos = lmGrout.Select(s => s.BatchNo).ToList();
                    var barCodes = lmGrout.Select(s => s.BarCode).ToList();

                    var lmQuery = Db.GetIQueryable<IT_LocalMaterial>();
                    lmQuery = lmQuery.Where(w => w.StorId == audit.StorId);
                    if (localIds.Count > 0)
                        lmQuery = lmQuery.Where(w => localIds.Contains(w.LocalId));
                    if (trayIds.Count > 0)
                        lmQuery = lmQuery.Where(w => trayIds.Contains(w.TrayId));
                    if (zoneIds.Count > 0)
                        lmQuery = lmQuery.Where(w => zoneIds.Contains(w.ZoneId));
                    if (materIds.Count > 0)
                        lmQuery = lmQuery.Where(w => materIds.Contains(w.MaterialId));
                    if (batchNos.Count > 0)
                        lmQuery = lmQuery.Where(w => batchNos.Contains(w.BatchNo));
                    if (barCodes.Count > 0)
                        lmQuery = lmQuery.Where(w => barCodes.Contains(w.BarCode));
                    var lmDbData = await lmQuery.ToListAsync();

                    //处理库存
                    var listLmAdd = new List<IT_LocalMaterial>();
                    var listLmUpdate = new List<IT_LocalMaterial>();
                    foreach (var item in lmGrout)
                    {
                        //如果存再当前相同的物料，就在库存上直接相加
                        var lmItem = lmDbData.Where(w => w.StorId == item.StorId && w.LocalId == item.ToLocalId && w.TrayId == item.ToTrayId && w.ZoneId == item.ToZoneId && w.MaterialId == item.MaterialId && w.BatchNo == item.BatchNo && w.BarCode == item.BarCode).SingleOrDefault();
                        if (lmItem != null)
                        {
                            lmItem.Num += item.MoveNum;
                            listLmUpdate.Add(lmItem);
                        }
                        else
                        {
                            //如果当前库存不存再相同的物料，就增加此物料的记录
                            var lm = new IT_LocalMaterial();
                            lm.Id = IdHelper.GetId();
                            lm.StorId = item.StorId;
                            lm.LocalId = item.ToLocalId;
                            lm.TrayId = item.ToTrayId;
                            lm.ZoneId = item.ToZoneId;
                            lm.MaterialId = item.MaterialId;
                            lm.MeasureId = dicMUnit[item.MaterialId];
                            lm.BatchNo = item.BatchNo;
                            lm.BarCode = item.BarCode;
                            lm.Num = item.MoveNum;
                            listLmAdd.Add(lm);
                        }
                    }
                    if (listLmAdd.Count > 0)
                    {
                        await lmSvc.AddDataAsync(listLmAdd);
                    }
                    if (listLmUpdate.Count > 0)
                    {
                        await lmSvc.UpdateDataAsync(listLmUpdate);
                    }

                    
                }

                // 增加库存明细
                {
                    var ldGrout = detail
                        .GroupBy(g => new { g.StorId, g.ToLocalId, g.ToTrayId, g.ToZoneId, g.MaterialId, g.BatchNo, g.BarCode, g.Price })
                        .Select(s => new { s.Key.StorId, s.Key.ToLocalId, s.Key.ToTrayId, s.Key.ToZoneId, s.Key.MaterialId, s.Key.BatchNo, s.Key.BarCode, s.Key.Price, TotalAmt = s.Sum(o => o.Amount), MoveNum = s.Sum(o => o.MoveNum) })
                        .ToList();
                    var listLd = new List<IT_LocalDetail>();
                    foreach (var item in ldGrout)
                    {
                        var ld = new IT_LocalDetail();
                        ld.Id = IdHelper.GetId();
                        ld.StorId = audit.StorId;
                        ld.InStorId = null;
                        ld.LocalId = item.ToLocalId;
                        ld.TrayId = item.ToTrayId;
                        ld.ZoneId = item.ToZoneId;
                        ld.MaterialId = item.MaterialId;
                        ld.MeasureId = dicMUnit[item.MaterialId];
                        ld.BatchNo = item.BatchNo;
                        ld.BarCode = item.BarCode;
                        ld.InTime = audit.AuditTime;
                        ld.Amount = item.TotalAmt;
                        ld.CreateTime = now;
                        ld.CreatorId = audit.AuditUserId;
                        ld.Price = item.Price;
                        ld.Num = item.MoveNum;
                        ld.Deleted = false;
                        listLd.Add(ld);
                    }
                    if (listLd.Count > 0)
                    {
                        await ldSvc.AddDataAsync(listLd);
                    }
                }
            }

            // 处理台帐
            {
                var listRB = new List<IT_RecordBook>();
                foreach (var move in detail)
                {
                    // 增加（移库-出）MoveOut台帐
                    var rbOut = new IT_RecordBook()
                    {
                        Id = IdHelper.GetId(),
                        RefCode = data.Code,
                        Type = "MoveOut",
                        FromStorId = audit.StorId,
                        FromLocalId = move.FromLocalId,
                        ToStorId = audit.StorId,
                        ToLocalId = move.ToLocalId,
                        MaterialId = move.MaterialId,
                        MeasureId = dicMUnit[move.MaterialId],
                        BarCode = move.BarCode,
                        BatchNo = move.BatchNo,
                        Num = move.MoveNum,
                        CreateTime = audit.AuditTime,
                        CreatorId = audit.AuditUserId,
                        Deleted = false
                    };
                    listRB.Add(rbOut);
                    // 增加（移库-入）MoveIn台帐
                    var rbIn = new IT_RecordBook()
                    {
                        Id = IdHelper.GetId(),
                        RefCode = data.Code,
                        Type = "MoveIn",
                        FromStorId = audit.StorId,
                        FromLocalId = move.FromLocalId,
                        ToStorId = audit.StorId,
                        ToLocalId = move.ToLocalId,
                        MaterialId = move.MaterialId,
                        MeasureId = dicMUnit[move.MaterialId],
                        BarCode = move.BarCode,
                        BatchNo = move.BatchNo,
                        Num = move.MoveNum,
                        CreateTime = audit.AuditTime,
                        CreatorId = audit.AuditUserId,
                        Deleted = false
                    };
                    listRB.Add(rbIn);
                }
                // 保存数据台帐
                var rbSvc = _ServiceProvider.GetRequiredService<IIT_RecordBookBusiness>();
                await rbSvc.AddDataAsync(listRB);
            }

            // 处理托盘位置数据
            {
                var dicTray = new Dictionary<string, string>();
                foreach (var item in detail)
                {
                    if (item.ToTrayId != null && !dicTray.ContainsKey(item.ToTrayId))
                        dicTray.Add(item.ToTrayId, item.ToLocalId);
                }
                var trayIds = dicTray.Keys.ToList();
                if (trayIds.Count > 0)
                {
                    var listTray = await Db.GetIQueryable<PB_Tray>().Where(w => trayIds.Contains(w.Id)).ToListAsync();
                    foreach (var tray in listTray)
                    {
                        tray.LocalId = dicTray[tray.Id];
                    }
                    var traySvc = _ServiceProvider.GetRequiredService<IPB_TrayBusiness>();
                    await traySvc.UpdateDataAsync(listTray);
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

        [DataEditLog(UserLogType.移库管理, "Id", "移库单驳回")]
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