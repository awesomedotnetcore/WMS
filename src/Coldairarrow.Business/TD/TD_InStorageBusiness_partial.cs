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
    public partial class TD_InStorageBusiness : BaseBusiness<TD_InStorage>, ITD_InStorageBusiness, ITransientDependency
    {
        public async Task<PageResult<TD_InStorage>> GetDataListAsync(TD_InStoragePageInput input)
        {
            var q = GetIQueryable().Where(w => w.StorId == input.StorId);
            var where = LinqHelper.True<TD_InStorage>();
            var search = input.Search;

            if (!search.Code.IsNullOrEmpty())
                where = where.And(w => w.Code.Contains(search.Code));
            if (!search.InType.IsNullOrEmpty())
                where = where.And(w => w.InType == search.InType);
            if (search.InStorTimeStart.HasValue)
                where = where.And(w => w.InStorTime >= search.InStorTimeStart.Value);
            if (search.InStorTimeEnd.HasValue)
                where = where.And(w => w.InStorTime <= search.InStorTimeEnd.Value);

            return await q.Where(where).GetPageResultAsync(input);
        }
    }
}