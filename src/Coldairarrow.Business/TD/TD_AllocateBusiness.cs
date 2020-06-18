using Coldairarrow.Entity.TD;
using Coldairarrow.Util;
using EFCore.Sharding;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using org.apache.zookeeper.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Coldairarrow.Business.TD
{
    public partial class TD_AllocateBusiness : BaseBusiness<TD_Allocate>, ITD_AllocateBusiness, ITransientDependency
    {
        public TD_AllocateBusiness(IRepository repository)
            : base(repository)
        {
        }

        #region 外部接口

        public async Task<PageResult<TD_Allocate>> GetDataListAsync(PageInput<ConditionDTO> input, string storageId)
        {
            var q = GetIQueryable();
            var search = input.Search;
            q = q.Include(i => i.Tar_Storage).Include(i => i.PB_Equipment).Include(i => i.AuditUser);

            //筛选
            //if (!search.Type.IsNullOrEmpty())
            //{
            //    q = q.Where(w => w.Type == search.Type);
            //}
            if (!search.Keyword.IsNullOrEmpty())
            {
                q = q.Where(w => w.Code.Contains(search.Keyword) || w.RefCode.Contains(search.Keyword));
            }
            if (!storageId.IsNullOrEmpty())
            {
                q = q.Where(w => w.StorId == storageId);
            }

            return await q.GetPageResultAsync(input);
        }

        public async Task<List<TD_Allocate>> GetDataListAsync(List<string> ids)
        {
            var q = GetIQueryable();
            return await q.Where(w => ids.Contains(w.Id)).ToListAsync();
        }

        public async Task<TD_Allocate> GetTheDataAsync(string id)
        {
            return await GetEntityAsync(id);
        }

        public async Task AddDataAsync(TD_Allocate data)
        {
            await InsertAsync(data);
        }

        public async Task UpdateDataAsync(TD_Allocate data)
        {
            await UpdateAsync(data);
        }

        public async Task DeleteDataAsync(List<string> ids)
        {
            await DeleteAsync(ids);
        }

        public async Task ApproveDatasAsync(List<string> ids, string userId)
        {
            await UpdateWhereAsync(w => ids.Contains(w.Id), 
                e => { e.Status = (int)AllocateStatus.审核通过; e.AuditUserId = userId; e.AuditeTime = DateTime.Now; });
        }

        public async Task RejectDatasAsync(List<string> ids, string userId)
        {
            await UpdateWhereAsync(w => ids.Contains(w.Id), 
                e => { e.Status = (int)AllocateStatus.审核失败; e.AuditUserId = userId; e.AuditeTime = DateTime.Now; });
        }
        #endregion

        #region 私有成员

        #endregion
    }
}