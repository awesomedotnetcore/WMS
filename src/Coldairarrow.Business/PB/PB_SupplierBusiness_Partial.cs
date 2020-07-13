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
    public partial class PB_SupplierBusiness
    {
        public async Task<PageResult<PB_Supplier>> QueryDataListAsync(PageInput<PBSupplierCoditionDTO> input)
        {
            var q = GetIQueryable();
            var where = LinqHelper.True<PB_Supplier>();
            var search = input.Search;

            if (!search.Type.IsNullOrWhiteSpace())
            {
                where = where.And(p => p.Type == search.Type);
            }

            if (!search.KeyWord.IsNullOrWhiteSpace())
            {
                where = where.And(p => p.Code.Contains(search.KeyWord) || p.Name.Contains(search.KeyWord));
            }

            return await q.Where(where).GetPageResultAsync(input);
        }

        public async Task<List<PB_Supplier>> QueryAllDataAsync()
        {
            var q = GetIQueryable();
            var where = LinqHelper.True<PB_Supplier>();

            return await q.Where(where).ToListAsync();
        }

        public async Task<List<PB_Supplier>> GetQueryData(SelectQueryDTO search)
        {
            var q = GetIQueryable();
            var where = LinqHelper.True<PB_Supplier>();
            if (!search.Keyword.IsNullOrEmpty())
                where = where.And(w => w.Name.Contains(search.Keyword) || w.Code.Contains(search.Keyword));
            var result = await q.Where(where).OrderBy(o => o.Name).Take(search.Take).ToListAsync();

            if (!search.Id.IsNullOrEmpty())
            {
                var one = await this.GetIQueryable().Where(w => w.Id == search.Id).SingleOrDefaultAsync();
                result.Add(one);
            }
            return result;
        }
    }
}