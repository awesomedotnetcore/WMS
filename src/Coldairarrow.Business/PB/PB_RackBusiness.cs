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
    public class PB_RackBusiness : BaseBusiness<PB_Rack>, IPB_RackBusiness, ITransientDependency
    {
        public PB_RackBusiness(IDbAccessor db)
            : base(db)
        {
        }

        #region 外部接口

        public async Task<PageResult<PB_Rack>> GetDataListAsync(PB_RackPageInput input)
        {
            var q = GetIQueryable().Where(w => w.StorId == input.StorId);
            var where = LinqHelper.True<PB_Rack>();
            var search = input.Search;
            q = q.Include(i => i.PB_Storage);

            if (!search.Keyword.IsNullOrEmpty())
            {
                search.Keyword = search.Keyword.Trim();
                //if (!search.Keyword.IsNullOrEmpty())
                where = where.And(w => w.Name.Contains(search.Keyword) || w.Code.Contains(search.Keyword));
            }
            return await q.Where(where).GetPageResultAsync(input);
        }

        public async Task<PB_Rack> GetTheDataAsync(string id)
        {
            return await GetEntityAsync(id);
        }

        [DataAddLog(UserLogType.仓库管理, "Name", "货架")]
        [DataRepeatValidate(new string[] { "StorId", "Code" }, new string[] { "仓库", "货架" }, allData: false, matchOr: false)]
        public async Task AddDataAsync(PB_Rack data)
        {
            await InsertAsync(data);
        }

        [DataEditLog(UserLogType.仓库管理, "Name", "货架")]
        [DataRepeatValidate(new string[] { "StorId", "Code" }, new string[] { "仓库", "货架" }, allData: false, matchOr: false)]
        public async Task UpdateDataAsync(PB_Rack data)
        {
            await UpdateAsync(data);
        }

        [DataDeleteLog(UserLogType.仓库管理, "Name", "货架")]
        [DataRepeatValidate(new string[] { "StorId", "Code" }, new string[] { "仓库", "货架" }, allData: false, matchOr: false)]
        public async Task DeleteDataAsync(List<string> ids)
        {
            await DeleteAsync(ids);
        }

        public async Task<List<PB_Rack>> QueryRackDataAsync()
        {
            var q = GetIQueryable();

            return await q.ToListAsync();
        }

        public async Task<List<PB_Rack>> GetDataListAsync(string storId)
        {
            var q = GetIQueryable();
            q = q.Where(w => w.StorId == storId);

            return await q.ToListAsync();
        }

        #endregion

        #region 私有成员

        #endregion
    }
}