using Coldairarrow.Business.PB;
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
        public TD_BadBusiness(IRepository repository, IServiceProvider svcProvider)
            : base(repository)
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
                where = where.And(w => w.Code.Contains(search.Code));
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

        [Transactional]
        public async Task UpdateDataAsync(TD_Bad data)
        {
            var curDetail = data.BadDetails;
            var listDetail = await Service.GetIQueryable<TD_BadDetail>().Where(w => w.BadId == data.Id).ToListAsync();

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



        [Transactional]
        public async Task Approve(AuditDTO audit)
        {
            var now = DateTime.Now;
            var data = await this.GetTheDataAsync(audit.Id);
            var detail = data.BadDetails;
            var dicMUnit = detail.Select(s => s.Material).Distinct().ToDictionary(k => k.Id, v => v.MeasureId);

            PB_Location defaultBadLocation = null;
            // 找到默认的报损货位
            {
                var localSvc = Service.GetIQueryable<PB_Location>();
                defaultBadLocation = await localSvc.Where(w => w.StorId == audit.StorId && w.Type == "Bad").OrderByDescending(o => o.IsDefault).FirstOrDefaultAsync();
                if (defaultBadLocation == null) throw new Exception("没有指定默认报损货位");
            }
            // 原库位出库，报损货位入库，同时修改库存与台帐
            var detailGroup = detail
                .GroupBy(w => new { w.FromLocalId, w.TrayId, w.ZoneId, w.MaterialId, w.BatchNo, w.BarCode })
                .Select(s => new { s.Key.FromLocalId, s.Key.TrayId, s.Key.ZoneId, s.Key.MaterialId, s.Key.BatchNo, s.Key.BarCode, BadNum = s.Sum(o => o.BadNum) })
                .ToList();
            var localIds = detailGroup.Select(s => s.FromLocalId).ToList();
            var trayIds = detailGroup.Select(s => s.TrayId).ToList();
            var zoneIds = detailGroup.Select(s => s.ZoneId).ToList();
            var batchNos = detailGroup.Select(s => s.BatchNo).ToList();
            var barCodes = detailGroup.Select(s => s.BarCode).ToList();




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