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
    public partial class TD_ReceivingBusiness : BaseBusiness<TD_Receiving>, ITD_ReceivingBusiness, ITransientDependency
    {
        public async Task<PageResult<TD_Receiving>> GetDataListAsync(TD_ReceivingPageInput input)
        {
            var q = this.GetIQueryable().Include(i=>i.Supplier).Where(w => w.StorId == input.StorId);
            var search = input.Search;
            var where = LinqHelper.True<TD_Receiving>();
            where = where.AndIf(!search.Keyword.IsNullOrEmpty(), w => w.Code.Contains(search.Keyword) || w.RefCode.Contains(search.Keyword) || w.Supplier.Code.Contains(search.Keyword) || w.Supplier.Name.Contains(search.Keyword));
            where = where.AndIf(!search.Type.IsNullOrEmpty(), w => w.Type == search.Type);
            where = where.AndIf(search.Status.HasValue, w => w.Status == search.Status.Value);
            where = where.AndIf(search.OrderTimeStart.HasValue, w => w.OrderTime >= search.OrderTimeStart.Value);
            where = where.AndIf(search.OrderTimeEnd.HasValue, w => w.OrderTime <= search.OrderTimeEnd.Value);
            return await q.Where(where).GetPageResultAsync(input);
        }
    }
}