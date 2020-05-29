using Coldairarrow.Business.PB;
using Coldairarrow.Entity.TD;
using Coldairarrow.Util;
using EFCore.Sharding;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
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
    }
}