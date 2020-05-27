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
    public partial class PB_LocationBusiness : BaseBusiness<PB_Location>, IPB_LocationBusiness, ITransientDependency
    {
        public PB_LocationBusiness(IRepository repository)
            : base(repository)
        {
        }

        #region 外部接口

        public async Task<PageResult<PB_Location>> GetDataListAsync(PageInput<ConditionDTO> input)
        {
            var q = GetIQueryable();
            var search = input.Search;
            q = q.Include(i => i.PB_Storage).Include(i => i.PB_Laneway).Include(i => i.PB_StorArea).Include(i => i.PB_Rack);

            //筛选
            if (!search.Keyword.IsNullOrEmpty())
            {
                q = q.Where(w => w.Name.Contains(search.Keyword) || w.Code.Contains(search.Keyword));
            }

            return await q.GetPageResultAsync(input);
        }

        public async Task<List<PB_Location>> GetDataListAsync()
        {
            var q = GetIQueryable();

            return await q.ToListAsync();
        }

        public async Task<PB_Location> GetTheDataAsync(string id)
        {
            return await GetEntityAsync(id);
        }

        public async Task AddDataAsync(PB_Location data)
        {
            await InsertAsync(data);
        }

        public async Task UpdateDataAsync(PB_Location data)
        {
            await UpdateAsync(data);
        }

        public async Task DeleteDataAsync(List<string> ids)
        {
            await DeleteAsync(ids);
        }

        public async Task ModifyEnableAsync(string Id)
        {
            var entity = await GetEntityAsync(Id);
            if (entity.IsForbid)
            {
                entity.IsForbid = false;
                if (entity.IsDefault) entity.IsDefault = false;
            }
            else
            {
                entity.IsForbid = true;
            }

            await UpdateAsync(entity);
        }

        #endregion

        #region 私有成员

        #endregion
    }
}