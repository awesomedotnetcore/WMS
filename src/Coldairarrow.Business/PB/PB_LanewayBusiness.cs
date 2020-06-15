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
    public class PB_LanewayBusiness : BaseBusiness<PB_Laneway>, IPB_LanewayBusiness, ITransientDependency
    {
        public PB_LanewayBusiness(IRepository repository)
            : base(repository)
        {
        }

        #region 外部接口

        public async Task<PageResult<PB_Laneway>> GetDataListAsync(PB_LanewayPageInput input)
        {
            var q = GetIQueryable().Where(w => w.StorId == input.StorId);
            var where = LinqHelper.True<PB_Laneway>();
            var search = input.Search;

            if (!search.Keyword.IsNullOrEmpty())
            {
                search.Keyword = search.Keyword.Trim();
                where = where.And(w => w.Name.Contains(search.Keyword) || w.Code.Contains(search.Keyword));
            }
                


            return await q.Where(where).GetPageResultAsync(input);
        }

        public async Task<PB_Laneway> GetTheDataAsync(string id)
        {
            return await GetEntityAsync(id);
        }

        public async Task AddDataAsync(PB_Laneway data)
        {
            await InsertAsync(data);
        }

        public async Task UpdateDataAsync(PB_Laneway data)
        {
            await UpdateAsync(data);
        }

        public async Task DeleteDataAsync(List<string> ids)
        {
            await DeleteAsync(ids);
        }

        public async Task<List<PB_Laneway>> QueryLanewayDataAsync()
        {
            var q = GetIQueryable();

            return await q.ToListAsync();
        }

        public async Task<List<PB_Laneway>> GetDataListAsync(string storId)
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