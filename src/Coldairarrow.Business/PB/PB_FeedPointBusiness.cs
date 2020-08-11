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
    public partial class PB_FeedPointBusiness : BaseBusiness<PB_FeedPoint>, IPB_FeedPointBusiness, ITransientDependency
    {
        public PB_FeedPointBusiness(IDbAccessor db)
            : base(db)
        {
        }

        #region 外部接口

        public async Task<PageResult<PB_FeedPoint>> GetDataListAsync(PageInput<ConditionDTO> input)
        {
            var q = GetIQueryable();
            var where = LinqHelper.True<PB_FeedPoint>();
            var search = input.Search;

            //筛选
            if (!search.Condition.IsNullOrEmpty() && !search.Keyword.IsNullOrEmpty())
            {
                var newWhere = DynamicExpressionParser.ParseLambda<PB_FeedPoint, bool>(
                    ParsingConfig.Default, false, $@"{search.Condition}.Contains(@0)", search.Keyword);
                where = where.And(newWhere);
            }

            return await q.Where(where).GetPageResultAsync(input);
        }

        public async Task<PB_FeedPoint> GetTheDataAsync(string id)
        {
            return await GetEntityAsync(id);
        }

        public async Task AddDataAsync(PB_FeedPoint data)
        {
            await InsertAsync(data);
        }

        public async Task UpdateDataAsync(PB_FeedPoint data)
        {
            await UpdateAsync(data);
        }

        public async Task DeleteDataAsync(List<string> ids)
        {
            await DeleteAsync(ids);
        }

        public async Task AddDataExlAsync(List<PB_FeedPoint> list)
        {
            await InsertAsync(list);   

        }

        public IQueryable<T> GetQueryable<T>() where T : class, new()
        {
            return Db.GetIQueryable<T>();
        }
        #endregion

        #region 私有成员

        #endregion
    }
}