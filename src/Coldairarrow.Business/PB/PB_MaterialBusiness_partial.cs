using Coldairarrow.Entity.PB;
using Coldairarrow.IBusiness.DTO;
using Coldairarrow.Util;
using EFCore.Sharding;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Quartz.Util;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Coldairarrow.Business.PB
{
    public partial class PB_MaterialBusiness : BaseBusiness<PB_Material>, IPB_MaterialBusiness, ITransientDependency
    {
        public async Task<List<PB_Material>> GetQueryData(SelectQueryDTO search)
        {
            var q = GetIQueryable();
            var where = LinqHelper.True<PB_Material>();

            if (!search.Id.IsNullOrEmpty())
                where = where.And(w => w.Id == search.Id);
            if (!search.Keyword.IsNullOrEmpty())
                where = where.Or(w => w.Name.Contains(search.Keyword) || w.Code.Contains(search.Keyword));

            return await q.Where(where).OrderBy(o => o.Name).Take(search.Take).ToListAsync();
        }

        public async Task<PageResult<PB_Material>> QueryDataListAsync(PageInput<PBMaterialConditionDTO> input)
        {
            var q = GetIQueryable();
            var where = LinqHelper.True<PB_Material>();
            var search = input.Search;

            if (!search.TypeId.IsNullOrWhiteSpace()) where = where.And(p => p.MaterialTypeId == search.TypeId);
            if (!search.CustomerId.IsNullOrWhiteSpace()) where = where.And(p => p.CusId == search.CustomerId);
            if (!search.SupplierId.IsNullOrWhiteSpace()) where = where.And(p => p.SupId == search.SupplierId);
            if (!search.Keyword.IsNullOrWhiteSpace()) where = where.And(p => p.Name.Contains(search.Keyword) || p.Code.Contains(search.Keyword));


            return await q.Where(where).GetPageResultAsync(input);
        }
    }
}