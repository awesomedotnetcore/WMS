using Coldairarrow.Entity.PB;
using Coldairarrow.IBusiness.DTO;
using Coldairarrow.Util;
using EFCore.Sharding;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Coldairarrow.Business.PB
{
    public partial class PB_LocationBusiness : BaseBusiness<PB_Location>, IPB_LocationBusiness, ITransientDependency
    {
        public async Task<List<PB_Location>> GetQueryData(SelectQueryDTO search, string storId)
        {
            var q = GetIQueryable().Where(w => w.StorId == storId);
            var where = LinqHelper.True<PB_Location>();

            if (!search.Id.IsNullOrEmpty())
                where = where.And(w => w.Id == search.Id);
            if (!search.Keyword.IsNullOrEmpty())
                where = where.Or(w => w.Name.Contains(search.Keyword) || w.Code.Contains(search.Keyword));

            return await q.Where(where).OrderBy(o => o.Name).Take(search.Take).ToListAsync();
        }
    }
}