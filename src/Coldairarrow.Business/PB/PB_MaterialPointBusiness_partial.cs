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
            var q = GetIQueryable().Where(w => w.PointId == input.PointId);
            var search = input.Search;
            q = q.Include(i => i.PB_Material);

            //筛选
            if (!search.MaterialName.IsNullOrEmpty())
            {
                q = q.Where(w => w.PB_Material.Name.Contains(search.MaterialName) || w.PB_Material.Code.Contains(search.MaterialName) || w.PB_Material.SimpleName.Contains(search.MaterialName) || w.PB_Material.BarCode.Contains(search.MaterialName));
            }
            var res = await q.GetPageResultAsync(input);

            return res;
        }
    }
}