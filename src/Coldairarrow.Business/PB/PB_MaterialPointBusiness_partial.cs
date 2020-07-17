using Coldairarrow.Entity.PB;
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
    public partial class PB_MaterialPointBusiness : BaseBusiness<PB_MaterialPoint>, IPB_MaterialPointBusiness, ITransientDependency
    {
        public async Task<PageResult<PB_MaterialPoint>> GetDataListAsync(PB_MaterialPointPageInput input)
        {
            var q = GetIQueryable()
                .Include(i => i.Material)
                .Include(i => i.Point)
                .Where(w => w.PointId == input.PointId);
            var where = LinqHelper.True<PB_MaterialPoint>();
            var search = input.Search;

            where = where.AndIf(!search.Keyword.IsNullOrEmpty(), w => w.Material.Name.Contains(search.Keyword) || w.Material.Code.Contains(search.Keyword) || w.Material.BarCode.Contains(search.Keyword) || w.Material.SimpleName.Contains(search.Keyword));
            where = where.AndIf(!search.MaterialTypeId.IsNullOrEmpty(), w => w.Material.MaterialTypeId == search.MaterialTypeId);

            return await q.Where(where).GetPageResultAsync(input);
        }
    }
}