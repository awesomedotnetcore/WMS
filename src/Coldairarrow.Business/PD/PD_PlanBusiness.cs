using Coldairarrow.Entity.PD;
using Coldairarrow.Util;
using EFCore.Sharding;
using LinqKit;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Coldairarrow.Business.PD
{
    public partial class PD_PlanBusiness : BaseBusiness<PD_Plan>, IPD_PlanBusiness, ITransientDependency
    {
        public PD_PlanBusiness(IDbAccessor db)
            : base(db)
        {
        }

        #region 外部接口

        public async Task<PageResult<PD_Plan>> GetDataListAsync(PageInput<PD_PlanQM> input)
        {
            var q = GetIQueryable().Include(i => i.Material);
            var where = LinqHelper.True<PD_Plan>();
            var search = input.Search;

            //筛选
            if (!search.Keyword.IsNullOrEmpty())
                where = where.And(w => w.MaterialId.Contains(search.Keyword) || w.Code.Contains(search.Keyword));


            return await q.Where(where).GetPageResultAsync(input);
        }

        public async Task<PD_Plan> GetTheDataAsync(string id)
        {
            return await GetEntityAsync(id);
        }
        [DataAddLog(UserLogType.计划表, "Code", "计划编号")]
        [DataRepeatValidate(new string[] { "Code" }, new string[] { "编号" })]
        public async Task AddDataAsync(PD_Plan data)
        {
            await InsertAsync(data);
        }
        [DataEditLog(UserLogType.计划表, "Code", "计划编号")]
        [DataRepeatValidate(new string[] { "Code" }, new string[] { "编号" })]
        public async Task UpdateDataAsync(PD_Plan data)
        {
            await UpdateAsync(data);
        }
        [DataDeleteLog(UserLogType.计划表, "Code", "计划编号")]
        public async Task DeleteDataAsync(List<string> ids)
        {
            await DeleteAsync(ids);
        }

        public async Task Status(string id, int status)
        {
            await UpdateSqlAsync(w => w.Id == id, ("Status", UpdateType.Equal, status));
        }

        #endregion

        #region 私有成员

        #endregion
    }
}