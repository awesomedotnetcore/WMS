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
    public partial class PB_AddressBusiness 
    {
        public async Task<PageResult<PB_Address>> QueryDataListAsync(PageInput<PBAddressConditionDTO> input)
        {
            var q = GetIQueryable();
            var where = LinqHelper.True<PB_Address>();
            var search = input.Search;

            //筛选
            if(!search.CusId.IsNullOrWhiteSpace())
            {
                where = where.And(p => p.CusId == search.CusId);
            }
            else if(!search.SupId.IsNullOrWhiteSpace())
            {
                where = where.And(p => p.SupId == search.SupId);
            }
            else
            {
                where = where.And(p => 1 == 0);
            }

            return await q.Where(where).GetPageResultAsync(input);
        }
    }
}