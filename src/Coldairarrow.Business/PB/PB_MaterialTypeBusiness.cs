using Coldairarrow.Entity.PB;
using Coldairarrow.Util;
using EFCore.Sharding;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Quartz.Util;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Coldairarrow.Business.PB
{
    public partial class PB_MaterialTypeBusiness : BaseBusiness<PB_MaterialType>, IPB_MaterialTypeBusiness, ITransientDependency
    {
        public PB_MaterialTypeBusiness(IDbAccessor db)
            : base(db)
        {
        }

        #region 外部接口

        public async Task<PageResult<PB_MaterialType>> GetDataListAsync(PageInput<ConditionDTO> input)
        {
            var q = GetIQueryable();
            var where = LinqHelper.True<PB_MaterialType>();
            var search = input.Search;

            //筛选
            if (!search.Condition.IsNullOrEmpty() && !search.Keyword.IsNullOrEmpty())
            {
                var newWhere = DynamicExpressionParser.ParseLambda<PB_MaterialType, bool>(
                    ParsingConfig.Default, false, $@"{search.Condition}.Contains(@0)", search.Keyword);
                where = where.And(newWhere);
            }

            return await q.Where(where).GetPageResultAsync(input);
        }

        public async Task<PB_MaterialType> GetTheDataAsync(string id)
        {
            return await GetEntityAsync(id);
        }

        [DataAddLog(UserLogType.物料类型, "Code", "物料类型")]
        [DataRepeatValidate(new string[] { "Code", "Name" }, new string[] { "编号", "名称" })]
        public async Task AddDataAsync(PB_MaterialType data)
        {
            await SetPathAsync(data);
            await InsertAsync(data);
        }

        [DataEditLog(UserLogType.物料类型, "Code", "物料类型")]
        [DataRepeatValidate(new string[] { "Code", "Name" }, new string[] { "编号", "名称" })]
        public async Task UpdateDataAsync(PB_MaterialType data)
        {
            await SetPathAsync(data);
            await ModifyRefDataAsync(data);
            await UpdateAsync(data);
        }

        [DataDeleteLog(UserLogType.物料类型, "Code", "物料类型")]
        public async Task DeleteDataAsync(List<string> ids)
        {
            await DeleteAsync(ids);
        }

        public async Task AddDataExlAsync(List<PB_MaterialType> list)
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