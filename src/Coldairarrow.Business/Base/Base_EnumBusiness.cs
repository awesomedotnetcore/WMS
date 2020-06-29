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
    public class Base_EnumBusiness : BaseBusiness<Base_Enum>, IBase_EnumBusiness, ITransientDependency
    {
        public Base_EnumBusiness(IDbAccessor db)
            : base(db)
        {
        }

        #region 外部接口

        public async Task<PageResult<Base_Enum>> GetDataListAsync(PageInput<Base_EnumQM> input)
        {
            var q = GetIQueryable();
            var where = LinqHelper.True<Base_Enum>();
            var search = input.Search;

            if (!search.Name.IsNullOrEmpty())
                where = where.And(w => w.Name.Contains(search.Name));
            if (!search.Code.IsNullOrEmpty())
                where = where.And(w => w.Code.Contains(search.Code));

            return await q.Where(where).GetPageResultAsync(input);
        }
        public async Task<Base_Enum> GetByCodeAsync(string code)
        {
            return await this.GetIQueryable().Include(i => i.EnumItems).Where(w => w.Code == code).SingleOrDefaultAsync();
        }
        public async Task<Base_Enum> GetTheDataAsync(string id)
        {
            return await GetEntityAsync(id);
        }

        public async Task AddDataAsync(Base_Enum data)
        {
            await InsertAsync(data);
        }

        public async Task UpdateDataAsync(Base_Enum data)
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