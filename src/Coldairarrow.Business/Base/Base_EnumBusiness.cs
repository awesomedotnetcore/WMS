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

            //筛选
            where = where.AndIf(!search.Keyword.IsNullOrEmpty(), w => w.Name.Contains(search.Keyword) || w.Code.Contains(search.Keyword));
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

        [DataAddLog(UserLogType.数据字典, "Name", "字典名称")]
        [DataRepeatValidate(new string[] { "Code" }, new string[] { "数据字典" }, allData: false, matchOr: false)]
        public async Task AddDataAsync(Base_Enum data)
        {
            await InsertAsync(data);
        }
        [DataEditLog(UserLogType.数据字典, "Name", "字典名称")]
        [DataRepeatValidate(new string[] { "Code" }, new string[] { "数据字典" }, allData: false, matchOr: false)]
        public async Task UpdateDataAsync(Base_Enum data)
        {
            await UpdateAsync(data);
        }
        [DataDeleteLog(UserLogType.数据字典, "Name", "字典名称")]
        public async Task DeleteDataAsync(List<string> ids)
        {
            await DeleteAsync(ids);
        }

        #endregion

        #region 私有成员

        #endregion
    }
}