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
    public class Base_ParameterBusiness : BaseBusiness<Base_Parameter>, IBase_ParameterBusiness, ITransientDependency
    {
        public Base_ParameterBusiness(IDbAccessor db)
            : base(db)
        {
        }

        #region 外部接口

        public async Task<PageResult<Base_Parameter>> GetDataListAsync(PB_ParameterPageInput input)
        {
            var q = GetIQueryable();
            var where = LinqHelper.True<Base_Parameter>();
            var search = input.Search;

            //筛选
            where = where.AndIf(!search.Keyword.IsNullOrEmpty(), w => w.Name.Contains(search.Keyword) || w.Code.Contains(search.Keyword));

            return await q.Where(where).GetPageResultAsync(input);
        }

        public async Task<Base_Parameter> GetTheDataAsync(string id)
        {
            return await GetEntityAsync(id);
        }

        public async Task<Dictionary<string, string>> GetConfig()
        {
            return await this.GetIQueryable().ToDictionaryAsync(k => k.Code, v => v.Val);
        }

        [DataAddLog(UserLogType.系统参数, "Code", "参数")]
        [DataRepeatValidate(new string[] { "Code" }, new string[] { "参数" }, allData: false, matchOr: false)]
        public async Task AddDataAsync(Base_Parameter data)
        {
            await InsertAsync(data);
        }
        [DataEditLog(UserLogType.系统参数, "Code", "参数")]
        [DataRepeatValidate(new string[] { "Code" }, new string[] { "参数" }, allData: false, matchOr: false)]
        public async Task UpdateDataAsync(Base_Parameter data)
        {
            //await UpdateAsync(data);
            await UpdateSqlAsync(w => w.Id == data.Id, ("Val", UpdateType.Equal, data.Val));
        }

        [DataDeleteLog(UserLogType.系统参数, "Code", "参数")]
        public async Task DeleteDataAsync(List<string> ids)
        {
            await DeleteAsync(ids);
        }

        #endregion

        #region 私有成员

        #endregion
    }
}