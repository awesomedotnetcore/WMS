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
        public PB_StorageBusiness(IDbAccessor db)
            : base(db)
        {
        }

        #region 外部接口

        public async Task<PageResult<PB_Storage>> GetDataListAsync(PB_StoragePageInput input)
        {
            var q = GetIQueryable();
            var where = LinqHelper.True<PB_Storage>();
            var search = input.Search;

            //筛选
            if (!search.Keyword.IsNullOrEmpty())
            {
                search.Keyword = search.Keyword.Trim();
                where = where.And(w => w.Name.Contains(search.Keyword) || w.Code.Contains(search.Keyword));
            }
                

            return await q.Where(where).GetPageResultAsync(input);
        }

        public async Task<PB_Storage> GetTheDataAsync(string id)
        {
            return await GetEntityAsync(id);
        }

        [DataAddLog(UserLogType.仓库管理, "Name", "仓库")]
        [DataRepeatValidate(new string[] { "Code", "Name" }, new string[] { "编号", "名称" })]
        public async Task AddDataAsync(PB_Storage data)
        {
            await InsertAsync(data);
        }

        [DataEditLog(UserLogType.仓库管理, "Name", "仓库")]
        [DataRepeatValidate(new string[] { "Code", "Name" }, new string[] { "编号", "名称" })]
        public async Task UpdateDataAsync(PB_Storage data)
        {            
            await UpdateAsync(data);
        }

        [DataDeleteLog(UserLogType.仓库管理, "Name", "仓库")]
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

        public async Task ModifyIsTrayAsync(string Id)
        {
            var entity = await GetEntityAsync(Id);
            if (entity.IsTray)
            {
                entity.IsTray = false;
                if (entity.IsZone) entity.IsZone = false;
            }
            else
            {
                entity.IsTray = true;
            }

            await UpdateAsync(entity);
        }

        public async Task ModifyIsZoneAsync(string Id)
        {
            var entity = await GetEntityAsync(Id);
            if (entity.IsZone)
            {
                entity.IsZone = false;
            }
            else
            {
                entity.IsZone = true;
            }

            await UpdateAsync(entity);
        }

        public async Task ModifyDisableAsync(string Id)
        {
            var entity = await GetEntityAsync(Id);
            if (entity.Disable)
            {
                entity.Disable = false;
            }
            else
            {
                entity.Disable = true;
            }

            await UpdateAsync(entity);
        }

        #endregion

        #region 私有成员

        #endregion
    }
}