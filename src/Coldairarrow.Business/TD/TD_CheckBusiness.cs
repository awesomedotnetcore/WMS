using Coldairarrow.Entity.TD;
using Coldairarrow.Util;
using EFCore.Sharding;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Coldairarrow.Business.TD
{
    public partial class TD_CheckBusiness : BaseBusiness<TD_Check>, ITD_CheckBusiness, ITransientDependency
    {
        public TD_CheckBusiness(IDbAccessor db, IServiceProvider svcProvider)
            : base(db)
        {
            _ServiceProvider = svcProvider;
        }

        readonly IServiceProvider _ServiceProvider;

        #region 外部接口

        public async Task<PageResult<TD_Check>> GetDataListAsync(PageInput<ConditionDTO> input)
        {
            var q = GetIQueryable();
            var where = LinqHelper.True<TD_Check>();
            var search = input.Search;

            //筛选
            if (!search.Condition.IsNullOrEmpty() && !search.Keyword.IsNullOrEmpty())
            {
                var newWhere = DynamicExpressionParser.ParseLambda<TD_Check, bool>(
                    ParsingConfig.Default, false, $@"{search.Condition}.Contains(@0)", search.Keyword);
                where = where.And(newWhere);
            }

            return await q.Where(where).GetPageResultAsync(input);
        }

        public async Task<TD_Check> GetTheDataAsync(string id)
        {
            return await GetEntityAsync(id);
        }

        [DataAddLog(UserLogType.盘点管理, "Code", "盘点单")]
        public async Task AddDataAsync(TD_Check data)
        {
            await InsertAsync(data);
        }

        [DataEditLog(UserLogType.盘点管理, "Code", "盘点单")]
        public async Task UpdateDataAsync(TD_Check data)
        {
            await UpdateAsync(data);
        }

        [DataDeleteLog(UserLogType.盘点管理, "Code", "盘点单")]
        public async Task DeleteDataAsync(List<string> ids)
        {
            await DeleteAsync(ids);
        }

        #endregion

        #region 私有成员

        #endregion
    }
}