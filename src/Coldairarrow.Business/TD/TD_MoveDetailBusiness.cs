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
    public class TD_MoveDetailBusiness : BaseBusiness<TD_MoveDetail>, ITD_MoveDetailBusiness, ITransientDependency
    {
        public TD_MoveDetailBusiness(IRepository repository)
            : base(repository)
        {
        }

        #region 外部接口

        public async Task<PageResult<TD_MoveDetail>> GetDataListAsync(PageInput<ConditionDTO> input)
        {
            var q = GetIQueryable();
            var search = input.Search;
            q = q.Include(i => i.Src_Location).Include(i => i.Src_Tray).Include(i => i.Src_TrayZone)
                .Include(i => i.Tar_Location).Include(i => i.Tar_Tray).Include(i => i.Tar_TrayZone)
                .Include(i => i.PB_Material);

            //筛选
            if (!search.Keyword.IsNullOrEmpty())
            {
                q = q.Where(w => w.MoveId == search.Keyword);
            }

            return await q.GetPageResultAsync(input);
        }

        public async Task<List<TD_MoveDetail>> GetDataListAsync(List<string> ids)
        {
            var q = GetIQueryable();

            //筛选
            if (ids.Count > 0)
            {
                q = q.Where(w => ids.Contains(w.Id));
            }

            return await q.ToListAsync();
        }

        public async Task<TD_MoveDetail> GetTheDataAsync(string id)
        {
            return await GetEntityAsync(id);
        }

        public async Task AddDataAsync(TD_MoveDetail data)
        {
            await InsertAsync(data);
        }

        public async Task UpdateDataAsync(TD_MoveDetail data)
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