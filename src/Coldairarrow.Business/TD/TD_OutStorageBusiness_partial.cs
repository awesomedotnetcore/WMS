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
    public partial class TD_OutStorageBusiness : BaseBusiness<TD_OutStorage>, ITD_OutStorageBusiness, ITransientDependency
    {
        public async Task<PageResult<TD_OutStorage>> GetDataListAsync(TD_OutStoragePageInput input)
        {
            var q = GetIQueryable()
                .Include(i => i.Customer)
                .Include(i => i.Address)
                .Include(i => i.CreateUser)
                .Include(i => i.AuditUser)
                .Where(w => w.StorageId == input.StorId);
            var where = LinqHelper.True<TD_OutStorage>();
            var search = input.Search;

            if (search.Status > -1) where = where.And(p => p.Status == search.Status);

            if (!search.Code.IsNullOrEmpty())
                where = where.And(w => w.Code.Contains(search.Code));
            if (!search.OutType.IsNullOrEmpty())
                where = where.And(w => w.OutType == search.OutType);
            if (search.OutStorTimeStart.HasValue)
                where = where.And(w => w.OutTime >= search.OutStorTimeStart.Value);
            if (search.OutStorTimeEnd.HasValue)
                where = where.And(w => w.OutTime <= search.OutStorTimeEnd.Value);

            return await q.Where(where).GetPageResultAsync(input);
        }

        public async Task<TD_OutStorage> GetTheDataAsync(string id)
        {
            return await this.GetIQueryable()
                .Include(i => i.OutStorDetails)
                    .ThenInclude(t => t.Location)
                .Include(i => i.OutStorDetails)
                    .ThenInclude(t => t.Material)
                .Include(i => i.OutStorDetails)
                    .ThenInclude(t => t.Tray)
                .Include(i => i.OutStorDetails)
                    .ThenInclude(t => t.TrayZone)
                .SingleOrDefaultAsync(w => w.Id == id);
        }

        public async Task AddDataAsync(TD_OutStorage data)
        {
            if (data.Code.IsNullOrEmpty())
            {
                var codeSvc = _ServiceProvider.GetRequiredService<IPB_BarCodeTypeBusiness>();
                data.Code = await codeSvc.Generate("TD_OutStorage");
            }
            data.OutNum = data.OutStorDetails.Sum(s => s.LocalNum);
            data.TotalAmt = data.OutStorDetails.Sum(s => s.TotalAmt);
            await InsertAsync(data);
        }

        public async Task UpdateDataAsync(TD_OutStorage data)
        {
            var curDetail = data.OutStorDetails;
            var listDetail = await Service.GetIQueryable<TD_OutStorDetail>().Where(w => w.OutStorId == data.Id).ToListAsync();

            var curIds = curDetail.Select(s => s.Id).ToList();
            var dbIds = listDetail.Select(s => s.Id).ToList();
            var deleteIds = dbIds.Except(curIds).ToList();
            var detailSvc = _ServiceProvider.GetRequiredService<ITD_OutStorDetailBusiness>();
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

            data.OutNum = data.OutStorDetails.Sum(s => s.LocalNum);
            data.TotalAmt = data.OutStorDetails.Sum(s => s.TotalAmt);

            await UpdateAsync(data);
        }
    }
}