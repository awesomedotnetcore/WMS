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
    public partial class PB_MeasureBusiness : BaseBusiness<PB_Measure>, IPB_MeasureBusiness, ITransientDependency
    {
        public PB_MeasureBusiness(IRepository repository)
            : base(repository)
        {
        }

        #region 外部接口

        public async Task<PageResult<PB_Measure>> GetDataListAsync(PageInput<ConditionDTO> input)
        {
            var q = GetIQueryable();
            var where = LinqHelper.True<PB_Measure>();
            var search = input.Search;

            //筛选
            if (!search.Condition.IsNullOrEmpty() && !search.Keyword.IsNullOrEmpty())
            {
                var newWhere = DynamicExpressionParser.ParseLambda<PB_Measure, bool>(
                    ParsingConfig.Default, false, $@"{search.Condition}.Contains(@0)", search.Keyword);
                where = where.And(newWhere);
            }

            return await q.Where(where).GetPageResultAsync(input);
        }

        public async Task<PB_Measure> GetTheDataAsync(string id)
        {
            return await GetEntityAsync(id);
        }

        public async Task AddDataAsync(PB_Measure data)
        {
            await InsertAsync(data);
        }

        public async Task UpdateDataAsync(PB_Measure data)
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