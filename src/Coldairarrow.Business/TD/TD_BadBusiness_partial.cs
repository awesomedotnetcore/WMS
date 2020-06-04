using Coldairarrow.Business.PB;
using Coldairarrow.Entity.TD;
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
    }
}