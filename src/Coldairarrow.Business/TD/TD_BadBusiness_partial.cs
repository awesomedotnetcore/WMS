using Coldairarrow.Entity.TD;
using Coldairarrow.Util;
using EFCore.Sharding;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Coldairarrow.Business.TD
{
    public partial class TD_BadBusiness : BaseBusiness<TD_Bad>, ITD_BadBusiness, ITransientDependency
    {
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
    }
}