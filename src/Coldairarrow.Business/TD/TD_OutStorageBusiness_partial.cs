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
                .Include(i => i.CreateUser)
                .Include(i => i.AuditUser)
                .Where(w => w.StorageId == input.StorId);
            var where = LinqHelper.True<TD_OutStorage>();
            var search = input.Search;

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
    }
}