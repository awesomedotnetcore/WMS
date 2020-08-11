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
    public partial class PB_FeedPointBusiness : BaseBusiness<PB_FeedPoint>, IPB_FeedPointBusiness, ITransientDependency
    {
        public async Task<PageResult<PB_FeedPoint>> GetDataListAsync(PageInput<PB_FeedPointQM> input)
        {
            var q = GetIQueryable()
                .Include(i=>i.Storage)
                .Include(i => i.Laneway);
            var where = LinqHelper.True<PB_FeedPoint>();
            var search = input.Search;
            where = where.AndIf(!search.Keyword.IsNullOrEmpty(), w => w.Name.Contains(search.Keyword) || w.Code.Contains(search.Keyword));
            where = where.AndIf(!search.StorId.IsNullOrEmpty(), w => w.StorId == search.StorId);
            where = where.AndIf(!search.Type.IsNullOrEmpty(), w => w.Type == search.Type);
            return await q.Where(where).GetPageResultAsync(input);
        }
        public async Task Enable(string id, bool enable)
        {
            await UpdateSqlAsync(w => w.Id == id, ("IsEnable", UpdateType.Equal, enable));
        }
    }
}