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
                where = where.And(w => w.Code.Contains(search.Code));
            if (!search.InType.IsNullOrEmpty())
                where = where.And(w => w.InType == search.InType);
            if (search.InStorTimeStart.HasValue)
                where = where.And(w => w.InStorTime >= search.InStorTimeStart.Value);
            if (search.InStorTimeEnd.HasValue)
                where = where.And(w => w.InStorTime <= search.InStorTimeEnd.Value);

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
        }

        [Transactional]
        public async Task UpdateDataAsync(TD_InStorage data)
        {
            var curDetail = data.InStorDetails;
            var listDetail = await Service.GetIQueryable<TD_InStorDetail>().Where(w => w.InStorId == data.Id).ToListAsync();

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
        }

        [Transactional]
        public async Task Approve(AuditDTO audit)
        {
            var now = DateTime.Now;
            var data = await this.GetTheDataAsync(audit.Id);
            var detail = data.InStorDetails;
            var dicMUnit = detail.Select(s => s.Material).Distinct().ToDictionary(k => k.Id, v => v.MeasureId);

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

                var lmQuery = Service.GetIQueryable<IT_LocalMaterial>();
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
                    var listTray = await Service.GetIQueryable<PB_Tray>().Where(w => trayIds.Contains(w.Id)).ToListAsync();
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