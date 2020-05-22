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
    public class PB_StorageBusiness : BaseBusiness<PB_Storage>, IPB_StorageBusiness, ITransientDependency
    {
        public PB_StorageBusiness(IRepository repository)
            : base(repository)
        {
        }

        #region 外部接口

        public async Task<PageResult<PB_Storage>> GetDataListAsync(PB_StoragePageInput input)
        {
            var q = GetIQueryable();
            var where = LinqHelper.True<PB_Storage>();
            var search = input.Search;

            //筛选
            if (!search.Name.IsNullOrEmpty())
                where = where.And(w => w.Name.Contains(search.Name));
            if (!search.Code.IsNullOrEmpty())
                where = where.And(w => w.Code.Contains(search.Code));

            return await q.Where(where).GetPageResultAsync(input);
        }

        public async Task<PB_Storage> GetTheDataAsync(string id)
        {
            return await GetEntityAsync(id);
        }

        public async Task AddDataAsync(PB_Storage data)
        {
            await InsertAsync(data);
        }

        public async Task UpdateDataAsync(PB_Storage data)
        {            
            await UpdateAsync(data);
        }

        public async Task DeleteDataAsync(List<string> ids)
        {
            await DeleteAsync(ids);
        }

        public async Task ModifyDefaultAsync(PB_Storage data)
        {
            var oldDefaultData = await GetIQueryable().Where(p => p.IsDefault == true).FirstOrDefaultAsync();
            oldDefaultData.IsDefault = false;

            var modifyData= new List<PB_Storage>() { data, oldDefaultData };

            await UpdateAsync(modifyData);
        }

        public async Task<List<PB_Storage>> QueryStorageDataAsync()
        {
            var q = GetIQueryable();
            var where = LinqHelper.True<PB_Storage>();


            return await q.Where(where).ToListAsync();
        }
        #endregion

        #region 私有成员

        #endregion
    }
}