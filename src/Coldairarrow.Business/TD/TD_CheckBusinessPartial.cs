using Coldairarrow.Entity.TD;
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

namespace Coldairarrow.Business.TD
{
    public partial class TD_CheckBusiness
    {
        public async Task<PageResult<TD_Check>> QueryDataListAsync(string storId,PageInput<TDCheckQueryDTO> input)
        {
            var q = GetIQueryable();
            var where = LinqHelper.True<TD_Check>();
            var search = input.Search;

            where = where.And(p => p.StorId == storId);
            if (search.IsComplete > -1) where = where.And(p => p.IsComplete == (search.IsComplete == 1));
            if (search.Status > -1) where = where.And(p => p.Status == search.Status);
            if (!search.RefCode.IsNullOrWhiteSpace()) where = where.And(p => p.RefCode.Contains(search.RefCode));

            return await q.Where(where).GetPageResultAsync(input);
        }
    }
}