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
    public partial class TD_AllocateDetailBusiness : BaseBusiness<TD_AllocateDetail>, ITD_AllocateDetailBusiness, ITransientDependency
    {
        public TD_AllocateDetailBusiness(IRepository repository)
            : base(repository)
        {
        }

        #region 外部接口

        public async Task<PageResult<TD_AllocateDetail>> GetDataListAsync(PageInput<ConditionDTO> input)
        {
            var q = GetIQueryable();
            var search = input.Search;
            q = q.Include(i => i.Src_Location).Include(i => i.Src_Tray).Include(i => i.Src_TrayZone)
                .Include(i => i.Tar_Storage).Include(i => i.Tar_Location).Include(i => i.PB_Material);

            //筛选
            if (!search.Keyword.IsNullOrEmpty())
            {
                q = q.Where(w => w.AllocateId == search.Keyword);
            }

            return await q.GetPageResultAsync(input);
        }

        public async Task<List<TD_AllocateDetail>> GetDataListAsync(List<string> ids)
        {
            var q = GetIQueryable();

            //筛选
            if (ids.Count > 0)
            {
                q = q.Where(w => ids.Contains(w.Id));
            }

            return await q.ToListAsync();
        }

        public async Task<List<TD_AllocateDetail>> GetDataListByAllocateIdsAsync(List<string> allocateIds)
        {
            var q = GetIQueryable();
            q = q.Include(i => i.PB_Material);

            //筛选
            if (allocateIds.Count > 0)
            {
                q = q.Where(w => allocateIds.Contains(w.AllocateId));
            }

            return await q.ToListAsync();
        }

        public async Task<TD_AllocateDetail> GetTheDataAsync(string id)
        {
            return await GetEntityAsync(id);
        }

        public async Task AddDataAsync(TD_AllocateDetail data)
        {
            await InsertAsync(data);
        }

        public async Task AddDatasAsync(List<TD_AllocateDetail> datas)
        {
            await InsertAsync(datas);
        }


        public async Task UpdateDataAsync(TD_AllocateDetail data)
        {
            await UpdateAsync(data);
        }

        public async Task UpdateDatasAsync(List<TD_AllocateDetail> datas)
        {
            await InsertAsync(datas);
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