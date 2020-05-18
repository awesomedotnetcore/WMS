using Coldairarrow.Entity.Base;
using Coldairarrow.Util;
using EFCore.Sharding;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Coldairarrow.Business.Base
{
    public class Base_EnumItemBusiness : BaseBusiness<Base_EnumItem>, IBase_EnumItemBusiness, ITransientDependency
    {
        public Base_EnumItemBusiness(IRepository repository)
            : base(repository)
        {
        }

        #region 外部接口

        public async Task<PageResult<Base_EnumItem>> GetDataListAsync(Base_EnumItemPageInput input)
        {
            var q = GetIQueryable().Where(w => w.EnumId == input.EnumId);
            var where = LinqHelper.True<Base_EnumItem>();
            var search = input.Search;

            if (!search.Value.IsNullOrEmpty())
                where = where.And(w => w.Value.Contains(search.Value));
            if (!search.Code.IsNullOrEmpty())
                where = where.And(w => w.Code.Contains(search.Code));

            return await q.Where(where).GetPageResultAsync(input);
        }

        public async Task<Base_EnumItem> GetTheDataAsync(string id)
        {
            return await GetEntityAsync(id);
        }

        public async Task AddDataAsync(Base_EnumItem data)
        {
            await InsertAsync(data);
        }

        public async Task UpdateDataAsync(Base_EnumItem data)
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