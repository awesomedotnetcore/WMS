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
    public partial class TD_InStorageBusiness : BaseBusiness<TD_InStorage>, ITD_InStorageBusiness, ITransientDependency
    {
        public TD_InStorageBusiness(IDbAccessor db, IServiceProvider svcProvider)
            : base(db)
        {
            _ServiceProvider = svcProvider;
        }
        readonly IServiceProvider _ServiceProvider;
        public async Task<PageResult<TD_InStorage>> GetDataListAsync(TD_InStoragePageInput input)
        {
            var q = GetIQueryable()
                .Include(i => i.Supplier)
                .Include(i => i.CreateUser)
                .Include(i => i.AuditUser)
                .Where(w => w.StorId == input.StorId);
            var where = LinqHelper.True<TD_InStorage>();
            var search = input.Search;

            if (!search.Code.IsNullOrEmpty())
                where = where.And(w => w.Code.Contains(search.Code) || w.RefCode.Contains(search.Code));
            if (!search.InType.IsNullOrEmpty())
                where = where.And(w => w.InType == search.InType);
            if (search.InStorTimeStart.HasValue)
                where = where.And(w => w.InStorTime >= search.InStorTimeStart.Value);
            if (search.InStorTimeEnd.HasValue)
                where = where.And(w => w.InStorTime <= search.InStorTimeEnd.Value);
            if (search.Status.HasValue)
                where = where.And(w => w.Status == search.Status.Value);

            return await q.Where(where).GetPageResultAsync(input);
        }

        public async Task<TD_InStorage> GetTheDataAsync(string id)
        {
            return await this.GetIQueryable()
                .Include(i => i.InStorDetails)
                    .ThenInclude(t => t.Location)
                .Include(i => i.InStorDetails)
                    .ThenInclude(t => t.Material)
                .Include(i => i.InStorDetails)
                    .ThenInclude(t => t.Tray)
                .Include(i => i.InStorDetails)
                    .ThenInclude(t => t.TrayZone)
                .SingleOrDefaultAsync(w => w.Id == id);
        }

        [DataAddLog(UserLogType.入库管理, "Code", "入库单")]
        [Transactional]
        public async Task AddDataAsync(TD_InStorage data)
        {
            if (data.Code.IsNullOrEmpty())
            {
                var codeSvc = _ServiceProvider.GetRequiredService<IPB_BarCodeTypeBusiness>();
                data.Code = await codeSvc.Generate("TD_InStorage");
            }
            data.TotalNum = data.InStorDetails.Sum(s => s.Num);
            data.TotalAmt = data.InStorDetails.Sum(s => s.TotalAmt);
            await InsertAsync(data);

            // 更新收货单数据
            if (!data.RecId.IsNullOrEmpty())
            {
                var recSvc = _ServiceProvider.GetRequiredService<ITD_ReceivingBusiness>();
                await recSvc.UpdateByInStorage(data.RecId);
            }
        }

        [DataEditLog(UserLogType.入库管理, "Code", "入库单")]
        [Transactional]
        public async Task UpdateDataAsync(TD_InStorage data)
        {
            var curDetail = data.InStorDetails;
            var listDetail = await Db.GetIQueryable<TD_InStorDetail>().Where(w => w.InStorId == data.Id).ToListAsync();

            var curIds = curDetail.Select(s => s.Id).ToList();
            var dbIds = listDetail.Select(s => s.Id).ToList();
            var deleteIds = dbIds.Except(curIds).ToList();
            var detailSvc = _ServiceProvider.GetRequiredService<ITD_InStorDetailBusiness>();
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

            data.TotalNum = data.InStorDetails.Sum(s => s.Num);
            data.TotalAmt = data.InStorDetails.Sum(s => s.TotalAmt);

            await UpdateAsync(data);

            // 更新收货单数据
            if (!data.RecId.IsNullOrEmpty())
            {
                var recSvc = _ServiceProvider.GetRequiredService<ITD_ReceivingBusiness>();
                await recSvc.UpdateByInStorage(data.RecId);
            }
        }

        [DataDeleteLog(UserLogType.入库管理, "Code", "入库单")]
        public async Task DeleteDataAsync(List<string> ids)
        {
            await DeleteAsync(ids);

            // 更新收货单数据
            var listRecId = await this.GetIQueryable().Where(w => ids.Contains(w.Id) && string.IsNullOrEmpty(w.RecId)).Select(s => s.RecId).ToListAsync();
            if (listRecId.Count > 0)
            {
                var recSvc = _ServiceProvider.GetRequiredService<ITD_ReceivingBusiness>();
                foreach (var item in listRecId)
                {
                    await recSvc.UpdateByInStorage(item);
                }
            }
        }

        [DataEditLog(UserLogType.入库管理, "Id", "入库单审批")]
        [Transactional]
        public async Task Approve(AuditDTO audit)
        {
            var now = DateTime.Now;
            var data = await this.GetTheDataAsync(audit.Id);
            var detail = data.InStorDetails;
            var dicMUnit = detail.Select(s => new { s.MaterialId, s.Material.MeasureId }).GroupBy(g => new { g.MaterialId, g.MeasureId }).ToDictionary(k => k.Key.MaterialId, v => v.Key.MeasureId);

            // 增加库存明细
            {
                var ldGrout = detail
                    .GroupBy(g => new { g.StorId, g.LocalId, g.TrayId, g.ZoneId, g.MaterialId, g.BatchNo, g.BarCode, g.Price })
                    .Select(s => new { s.Key.StorId, s.Key.LocalId, s.Key.TrayId, s.Key.ZoneId, s.Key.MaterialId, s.Key.BatchNo, s.Key.BarCode, s.Key.Price, TotalAmt = s.Sum(o => o.TotalAmt), Num = s.Sum(o => o.Num) })
                    .ToList();
                var listLd = new List<IT_LocalDetail>();
                foreach (var item in ldGrout)
                {
                    var ld = new IT_LocalDetail();
                    ld.Id = IdHelper.GetId();
                    ld.StorId = audit.StorId;
                    ld.InStorId = audit.Id;
                    ld.LocalId = item.LocalId;
                    ld.TrayId = item.TrayId;
                    ld.ZoneId = item.ZoneId;
                    ld.MaterialId = item.MaterialId;
                    ld.MeasureId = dicMUnit[item.MaterialId];
                    ld.BatchNo = item.BatchNo;
                    ld.BarCode = item.BarCode;
                    ld.InTime = audit.AuditTime;
                    ld.Amount = item.TotalAmt;
                    ld.CreateTime = now;
                    ld.CreatorId = audit.AuditUserId;
                    ld.Price = item.Price;
                    ld.Num = item.Num;
                    ld.Deleted = false;
                    listLd.Add(ld);
                }
                if (listLd.Count > 0)
                {
                    var localdetailSvc = _ServiceProvider.GetRequiredService<IIT_LocalDetailBusiness>();
                    await localdetailSvc.AddDataAsync(listLd);
                }
            }

            // 增加库存
            {
                var lmGrout = detail
                    .GroupBy(g => new { g.StorId, g.LocalId, g.TrayId, g.ZoneId, g.MaterialId, g.BatchNo, g.BarCode })
                    .Select(s => new { s.Key.StorId, s.Key.LocalId, s.Key.TrayId, s.Key.ZoneId, s.Key.MaterialId, s.Key.BatchNo, s.Key.BarCode, Num = s.Sum(o => o.Num) })
                    .ToList();
                //查询当前库存记录
                var localIds = lmGrout.Select(s => s.LocalId).ToList();
                var trayIds = lmGrout.Select(s => s.TrayId).ToList();
                var zoneIds = lmGrout.Select(s => s.ZoneId).ToList();
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
                var localMaterialSvc = _ServiceProvider.GetRequiredService<IIT_LocalMaterialBusiness>();
                var listLmAdd = new List<IT_LocalMaterial>();
                var listLmUpdate = new List<IT_LocalMaterial>();
                foreach (var item in lmGrout)
                {
                    //如果存再当前相同的物料，就在库存上直接相加
                    var lmItem = lmDbData.Where(w => w.StorId == item.StorId && w.LocalId == item.LocalId && w.TrayId == item.TrayId && w.ZoneId == item.ZoneId && w.MaterialId == item.MaterialId && w.BatchNo == item.BatchNo && w.BarCode == item.BarCode).SingleOrDefault();
                    if (lmItem != null)
                    {
                        lmItem.Num += item.Num;
                        listLmUpdate.Add(lmItem);
                    }
                    else
                    {
                        //如果当前库存不存再相同的物料，就增加此物料的记录
                        var lm = new IT_LocalMaterial();
                        lm.Id = IdHelper.GetId();
                        lm.StorId = item.StorId;
                        lm.LocalId = item.LocalId;
                        lm.TrayId = item.TrayId;
                        lm.ZoneId = item.ZoneId;
                        lm.MaterialId = item.MaterialId;
                        lm.MeasureId = dicMUnit[item.MaterialId];
                        lm.BatchNo = item.BatchNo;
                        lm.BarCode = item.BarCode;
                        lm.Num = item.Num;
                        listLmAdd.Add(lm);
                    }
                }
                if (listLmAdd.Count > 0)
                {
                    await localMaterialSvc.AddDataAsync(listLmAdd);
                }
                if (listLmUpdate.Count > 0)
                {
                    await localMaterialSvc.UpdateDataAsync(listLmUpdate);
                }
            }

            // 处理台帐
            {
                var lmGrout = detail
                    .GroupBy(g => new { g.StorId, g.LocalId, g.TrayId, g.ZoneId, g.MaterialId, g.BatchNo, g.BarCode })
                    .Select(s => new { s.Key.StorId, s.Key.LocalId, s.Key.TrayId, s.Key.ZoneId, s.Key.MaterialId, s.Key.BatchNo, s.Key.BarCode, Num = s.Sum(o => o.Num) })
                    .ToList();

                var listRB = new List<IT_RecordBook>();
                foreach (var item in lmGrout)
                {
                    var rb = new IT_RecordBook();
                    rb.Id = IdHelper.GetId();
                    rb.RefCode = data.Code;
                    rb.Type = "In";
                    rb.ToStorId = item.StorId;
                    rb.ToLocalId = item.LocalId;
                    rb.MaterialId = item.MaterialId;
                    rb.MeasureId = dicMUnit[item.MaterialId];
                    rb.BatchNo = item.BatchNo;
                    rb.BarCode = item.BarCode;
                    rb.Num = item.Num;
                    rb.CreateTime = now;
                    rb.CreatorId = audit.AuditUserId;
                    rb.Deleted = false;
                    listRB.Add(rb);
                }
                if (listRB.Count > 0)
                {
                    var bookSvc = _ServiceProvider.GetRequiredService<IIT_RecordBookBusiness>();
                    await bookSvc.AddDataAsync(listRB);
                }
            }

            // 处理托盘位置数据
            {
                var dicTray = new Dictionary<string, string>();
                foreach (var item in detail)
                {
                    if (item.TrayId != null && !dicTray.ContainsKey(item.TrayId))
                        dicTray.Add(item.TrayId, item.LocalId);
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

            // 解锁货位 在自动入库的时候，货位可能被入库锁锁定了
            {
                var localIds = detail.Select(s => s.LocalId).ToList();
                var listLocal = await Db.GetIQueryable<PB_Location>().Where(w => localIds.Contains(w.Id) && w.LockType == 1).ToListAsync();
                //await Db.Update_SqlAsync<PB_Location>(w => localIds.Contains(w.Id) && w.LockType == 1, ("LockType", UpdateType.Equal, 0));
                foreach (var item in listLocal)
                {
                    item.LockType = 0;
                }
                var localSvc = _ServiceProvider.GetRequiredService<IPB_LocationBusiness>();
                await localSvc.UpdateDataAsync(listLocal);
            }

            // 修改主数据状态
            {
                data.Status = 1;
                data.AuditeTime = audit.AuditTime;
                data.AuditUserId = audit.AuditUserId;
                await UpdateAsync(data);
            }

            // 更新收货单数据
            if (!data.RecId.IsNullOrEmpty())
            {
                var recSvc = _ServiceProvider.GetRequiredService<ITD_ReceivingBusiness>();
                await recSvc.UpdateByInStorage(data.RecId);
            }
        }
        [DataEditLog(UserLogType.入库管理, "Id", "入库单驳回")]
        [Transactional]
        public async Task Reject(AuditDTO audit)
        {
            var data = await this.GetTheDataAsync(audit.Id);
            // 修改主数据状态
            {
                data.Status = 2;
                data.AuditeTime = audit.AuditTime;
                data.AuditUserId = audit.AuditUserId;
                await UpdateAsync(data);
            }

            // 解锁货位 在自动入库的时候，货位可能被入库锁锁定了
            {
                var localIds = data.InStorDetails.Select(s => s.LocalId).ToList();
                var listLocal = await Db.GetIQueryable<PB_Location>().Where(w => localIds.Contains(w.Id) && w.LockType == 1).ToListAsync();
                foreach (var item in listLocal)
                {
                    item.LockType = 0;
                }
                var localSvc = _ServiceProvider.GetRequiredService<IPB_LocationBusiness>();
                await localSvc.UpdateDataAsync(listLocal);
            }

            // 更新收货单数据
            if (!data.RecId.IsNullOrEmpty())
            {
                var recSvc = _ServiceProvider.GetRequiredService<ITD_ReceivingBusiness>();
                await recSvc.UpdateByInStorage(data.RecId);
            }
        }

        public async Task InBlankTray(List<KeyValuePair<string, string>> listTray)
        {
            var listTrayIds = listTray.Select(s => s.Key).ToList();
            var trays = await Db.GetIQueryable<PB_Tray>().Where(w => listTrayIds.Contains(w.Id)).ToListAsync();
            var dicLocal = listTray.ToDictionary(k => k.Key, v => v.Value);
            foreach (var tray in trays)
            {
                if (dicLocal.ContainsKey(tray.Id))
                    tray.LocalId = dicLocal[tray.Id];
            }
            var traySvc = _ServiceProvider.GetRequiredService<IPB_TrayBusiness>();
            await traySvc.UpdateDataAsync(trays);
        }

        [Transactional(System.Data.IsolationLevel.Serializable)]
        public async Task<string> ReqLocation((string StorId, string MaterialId, string TaryId) data)
        {
            //托盘类型
            var tray = await Db.GetIQueryable<PB_Tray>().SingleOrDefaultAsync(w => w.Id == data.TaryId);

            //找到可存此物料的货区
            var listAreaId = from sa in Db.GetIQueryable<PB_StorArea>()
                             join am in Db.GetIQueryable<PB_AreaMaterial>() on sa.Id equals am.AreaId
                             where sa.StorId == data.StorId && am.MaterialId == data.MaterialId
                             select sa.Id;

            //有库存的货位
            var lmLocal = from lm in Db.GetIQueryable<IT_LocalMaterial>()
                          join l in Db.GetIQueryable<PB_Location>() on lm.LocalId equals l.Id
                          where l.StorId == data.StorId && lm.StorId == data.StorId
                          select l.Id;

            // 有托盘的货位
            var trayLocal = from t in Db.GetIQueryable<PB_Tray>()
                            join l in Db.GetIQueryable<PB_Location>() on t.LocalId equals l.Id
                            where l.StorId == data.StorId
                            select t.LocalId;

            //过滤货位
            var LocalQuery = from lt in Db.GetIQueryable<PB_LocalTray>()
                             join l in Db.GetIQueryable<PB_Location>() on lt.LocalId equals l.Id
                             join tt in Db.GetIQueryable<PB_TrayType>() on lt.TrayTypeId equals tt.Id
                             where l.StorId == data.StorId && tt.Id == tray.TrayTypeId
                             && l.IsForbid == false // 没有禁用
                             && listAreaId.Contains(l.AreaId)  //货区过滤
                             && l.LockType == 0  //锁定过滤
                             && !lmLocal.Contains(l.Id) //库存过滤
                             && !trayLocal.Contains(l.Id) // 托盘过滤
                             select new { l.Id };

            //TODO:可以做到从数据库随机取一个
            var count = await LocalQuery.CountAsync();
            if (count == 0) return null;
            var skip = RandomHelper.Next(0, count - 1);
            var result = await LocalQuery.Skip(skip).Take(1).FirstOrDefaultAsync();

            //锁定货位
            await Db.UpdateSqlAsync<PB_Location>(w => w.Id == result.Id, ("LockType", UpdateType.Equal, 1));

            return result.Id;
        }
    }
}