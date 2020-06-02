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
    public partial class PB_CustomerBusiness
    {
        public async Task<PageResult<PB_Customer>> QueryDataListAsync(PageInput<PBCustomerCoditionDTO> input)
        {
            var q = GetIQueryable();
            var where = LinqHelper.True<PB_Customer>();
            var search = input.Search;

            if(!search.Type.IsNullOrWhiteSpace())
            {
                where = where.And(p => p.Type == search.Type);
            }

            if (!search.KeyWord.IsNullOrWhiteSpace())
            {
                where = where.And(p => p.Code.Contains(search.KeyWord) || p.Name.Contains(search.KeyWord));
            }

            return await q.Where(where).GetPageResultAsync(input);
        }

        public async Task<List<PB_Customer>> QueryAllDataAsync()
        {
            var q = GetIQueryable();
            var where = LinqHelper.True<PB_Customer>();

            return await q.Where(where).ToListAsync();
        }
    }
}