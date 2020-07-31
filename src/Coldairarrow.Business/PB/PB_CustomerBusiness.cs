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
    public partial class PB_CustomerBusiness : BaseBusiness<PB_Customer>, IPB_CustomerBusiness, ITransientDependency
    {
        public PB_CustomerBusiness(IDbAccessor db)
            : base(db)
        {
        }

        #region 外部接口

        public async Task<PageResult<PB_Customer>> GetDataListAsync(PageInput<ConditionDTO> input)
        {
            var q = GetIQueryable();
            var where = LinqHelper.True<PB_Customer>();
            var search = input.Search;

            //筛选
            if (!search.Condition.IsNullOrEmpty() && !search.Keyword.IsNullOrEmpty())
            {
                var newWhere = DynamicExpressionParser.ParseLambda<PB_Customer, bool>(
                    ParsingConfig.Default, false, $@"{search.Condition}.Contains(@0)", search.Keyword);
                where = where.And(newWhere);
            }

            return await q.Where(where).GetPageResultAsync(input);
        }

        public async Task<PB_Customer> GetTheDataAsync(string id)
        {
            return await GetEntityAsync(id);
        }

        [DataAddLog(UserLogType.客户管理, "Name", "客户")]
        [DataRepeatValidate(new string[] { "Name", "Code" }, new string[] { "客户", "编码" })]
        public async Task AddDataAsync(PB_Customer data)
        {
            await InsertAsync(data);
        }

        [DataEditLog(UserLogType.客户管理, "Name", "客户")]
        [DataRepeatValidate(new string[] { "Name", "Code" }, new string[] { "客户", "编码" })]
        public async Task UpdateDataAsync(PB_Customer data)
        {
            await UpdateAsync(data);
        }

        [DataDeleteLog(UserLogType.客户管理, "Name", "客户")]
        public async Task DeleteDataAsync(List<string> ids)
        {
            await DeleteAsync(ids);
        }

        public async Task AddDataExlAsync(List<PB_Customer> list)
        {
            await InsertAsync(list);   

        }

        #endregion

        #region 私有成员

        #endregion
    }
}