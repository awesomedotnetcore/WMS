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
    public partial class PB_TrayTypeBusiness : BaseBusiness<PB_TrayType>, IPB_TrayTypeBusiness, ITransientDependency
    {
        

        #region 外部接口

        public async Task<PageResult<PB_TrayType>> GetDataListAsync(PageInput<ConditionDTO> input)
        {
            var q = GetIQueryable();
            var where = LinqHelper.True<PB_TrayType>();
            var search = input.Search;

            //筛选
            if (!search.Condition.IsNullOrEmpty() && !search.Keyword.IsNullOrEmpty())
            {
                var newWhere = DynamicExpressionParser.ParseLambda<PB_TrayType, bool>(
                    ParsingConfig.Default, false, $@"{search.Condition}.Contains(@0)", search.Keyword);
                where = where.And(newWhere);
            }

            return await q.Where(where).GetPageResultAsync(input);
        }

        public async Task<List<PB_TrayType>> GetDataListAsync()
        {
            var q = GetIQueryable();

            return await q.ToListAsync();
        }

        public async Task<PageResult<PB_TrayType>> GetDataListBySearch(PageInput<ConditionDTO> input)
        {
            var q = GetIQueryable();
            var search = input.Search;

            //筛选
            if (!search.Keyword.IsNullOrEmpty())
            {
                q = q.Where(w => w.Name.Contains(search.Keyword) || w.Code.Contains(search.Keyword));
            }

            return await q.GetPageResultAsync(input);
        }

        public async Task<PB_TrayType> GetTheDataAsync(string id)
        {
            return await GetEntityAsync(id);
        }


        [DataEditLog(UserLogType.托盘类型, "Name", "托盘类型")]
        [DataRepeatValidate(new string[] { "Code", "Name" }, new string[] { "编号", "名称" })]
        public async Task UpdateDataAsync(PB_TrayType data)
        {
            await UpdateAsync(data);
        }

        [DataDeleteLog(UserLogType.托盘类型, "Name", "托盘类型")]
        public async Task DeleteDataAsync(List<string> ids)
        {
            await DeleteAsync(ids);
        }

        public async Task AddDataExlAsync(List<PB_TrayType> list)
        {
            await InsertAsync(list);

        }

        #endregion

        #region 私有成员

        #endregion
    }
}