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
    public class PB_TrayZoneBusiness : BaseBusiness<PB_TrayZone>, IPB_TrayZoneBusiness, ITransientDependency
    {
        public PB_TrayZoneBusiness(IDbAccessor db)
            : base(db)
        {
        }

        #region 外部接口

        public async Task<PageResult<PB_TrayZone>> GetDataListAsync(PageInput<ConditionDTO> input)
        {
            var q = GetIQueryable();
            var where = LinqHelper.True<PB_TrayZone>();
            var search = input.Search;

            //筛选
            if (!search.Condition.IsNullOrEmpty() && !search.Keyword.IsNullOrEmpty())
            {
                var newWhere = DynamicExpressionParser.ParseLambda<PB_TrayZone, bool>(
                    ParsingConfig.Default, false, $@"{search.Condition}.Contains(@0)", search.Keyword);
                where = where.And(newWhere);
            }

            return await q.Where(where).GetPageResultAsync(input);
        }

        public async Task<List<PB_TrayZone>> GetDataListAsync(string typeId)
        {
            var q = GetIQueryable();
            q = q.Where(w => w.TrayTypeId == typeId);

            return await q.ToListAsync();
        }

        public async Task<PB_TrayZone> GetTheDataAsync(string id)
        {
            return await GetEntityAsync(id);
        }

        [DataAddLog(UserLogType.托盘类型, "Name", "托盘分区")]
        [DataRepeatValidate(new string[] { "TrayTypeId", "Name" }, new string[] { "托盘类型", "分区" }, allData: false, matchOr: false)]
        public async Task AddDataAsync(PB_TrayZone data)
        {
            await InsertAsync(data);
        }

        [DataEditLog(UserLogType.托盘类型, "Name", "托盘分区")]
        [DataRepeatValidate(new string[] { "TrayTypeId", "Name" }, new string[] { "托盘类型", "分区" }, allData: false, matchOr: false)]
        public async Task UpdateDataAsync(PB_TrayZone data)
        {
            await UpdateAsync(data);
        }

        [DataDeleteLog(UserLogType.托盘类型, "Name", "托盘分区")]
        public async Task DeleteDataAsync(List<string> ids)
        {
            await DeleteAsync(ids);
        }

        #endregion

        #region 私有成员

        #endregion
    }
}