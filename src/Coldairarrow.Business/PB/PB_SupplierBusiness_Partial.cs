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
    }
}