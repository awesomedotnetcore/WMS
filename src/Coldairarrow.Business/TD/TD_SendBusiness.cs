using Coldairarrow.Entity.TD;
using Coldairarrow.Util;
using EFCore.Sharding;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Coldairarrow.Business.TD
{
    public partial class TD_SendBusiness : BaseBusiness<TD_Send>, ITD_SendBusiness, ITransientDependency
    {
        public TD_SendBusiness(IDbAccessor db)
            : base(db)
        {
        }

        #region 外部接口

        //public async Task<PageResult<TD_Send>> GetDataListAsync(PageInput<ConditionDTO> input)
        //{
        //    var q = GetIQueryable();
        //    var where = LinqHelper.True<TD_Send>();
        //    var search = input.Search;

        //    //筛选
        //    if (!search.Condition.IsNullOrEmpty() && !search.Keyword.IsNullOrEmpty())
        //    {
        //        var newWhere = DynamicExpressionParser.ParseLambda<TD_Send, bool>(
        //            ParsingConfig.Default, false, $@"{search.Condition}.Contains(@0)", search.Keyword);
        //        where = where.And(newWhere);
        //    }

        //    return await q.Where(where).GetPageResultAsync(input);
        //}

        //public async Task<TD_Send> GetTheDataAsync(string id)
        //{
        //    return await GetEntityAsync(id);
        //}

        //public async Task AddDataAsync(TD_Send data)
        //{
        //    await InsertAsync(data);
        //}

        //public async Task UpdateDataAsync(TD_Send data)
        //{
        //    await UpdateAsync(data);
        //}

        [DataDeleteLog(UserLogType.发货管理, "Code", "发货单")]
        public async Task DeleteDataAsync(List<string> ids)
        {
            await DeleteAsync(ids);
        }

        #endregion

        #region 私有成员

        #endregion
    }
}