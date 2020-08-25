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
        public Base_EnumItemBusiness(IDbAccessor db)
            : base(db)
        {
        }

        #region 外部接口

        public async Task<PageResult<Base_EnumItem>> GetDataListAsync(Base_EnumItemPageInput input)
        {
            var q = GetIQueryable().Where(w => w.EnumId == input.EnumId);
            var where = LinqHelper.True<Base_EnumItem>();
            var search = input.Search;

            if (!search.Keyword.IsNullOrEmpty())
                where = where.And(w => w.Name.Contains(search.Keyword) || w.Code.Contains(search.Keyword));

            return await q.Where(where).GetPageResultAsync(input);
        }
        public Task<List<Base_EnumItem>> GetDataListAsync(string enumCode)
        {
            return GetIQueryable().Where(w => w.EnumCode == enumCode).ToListAsync();
        }
        public async Task<Base_EnumItem> GetTheDataAsync(string id)
        {
            return await GetEntityAsync(id);
        }
        [DataAddLog(UserLogType.数据字典, "Name", "字典值")]
        [DataRepeatValidate(new string[] { "EnumId", "Code" }, new string[] { "数据字典", "值" }, allData: false, matchOr: false)]
        public async Task AddDataAsync(Base_EnumItem data)
        {
            await InsertAsync(data);
        }
        [DataEditLog(UserLogType.数据字典, "Name", "字典值")]
        [DataRepeatValidate(new string[] { "EnumId", "Code" }, new string[] { "数据字典", "值" }, allData: false, matchOr: false)]
        public async Task UpdateDataAsync(Base_EnumItem data)
        {
            await UpdateAsync(data);
        }
        [DataDeleteLog(UserLogType.数据字典, "Name", "字典值")]
        public async Task DeleteDataAsync(List<string> ids)
        {
            await DeleteAsync(ids);
        }

        #endregion

        #region 私有成员

        #endregion
    }
}