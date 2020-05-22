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
    public class PB_StorAreaBusiness : BaseBusiness<PB_StorArea>, IPB_StorAreaBusiness, ITransientDependency
    {
        public PB_StorAreaBusiness(IRepository repository)
            : base(repository)
        {
        }

        #region 外部接口

        public async Task<PageResult<PB_StorArea>> GetDataListAsync(PageInput<ConditionDTO> input)
        {
            var q = GetIQueryable();
            var search = input.Search;
            q = q.Include(i => i.PB_Storage);

            //筛选
            if (!search.Keyword.IsNullOrEmpty())
            {
                q = q.Where(w => w.Name.Contains(search.Keyword) || w.Code.Contains(search.Keyword));
            }

            return await q.GetPageResultAsync(input);
        }

        public async Task<PB_StorArea> GetTheDataAsync(string id)
        {
            return await GetEntityAsync(id);
        }

        public async Task AddDataAsync(PB_StorArea data)
        {
            await InsertAsync(data);
        }

        public async Task UpdateDataAsync(PB_StorArea data)
        {
            await UpdateAsync(data);
        }

        public async Task DeleteDataAsync(List<string> ids)
        {
            await DeleteAsync(ids);
        }

        #endregion

        #region 私有成员

        #endregion
    }
}