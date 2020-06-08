using Coldairarrow.Entity.TD;
using Coldairarrow.Util;
using EFCore.Sharding;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Coldairarrow.Business.TD
{
    public class TD_MoveBusiness : BaseBusiness<TD_Move>, ITD_MoveBusiness, ITransientDependency
    {
        public TD_MoveBusiness(IRepository repository)
            : base(repository)
        {
        }

        #region 外部接口

        public async Task<PageResult<TD_Move>> GetDataListAsync(PageInput<SearchCondition> input, string storageId)
        {
            var q = GetIQueryable();
            var search = input.Search;
            q = q.Include(i => i.PB_Equipment).Include(i => i.AuditUser);

            //筛选
            if (!search.Type.IsNullOrEmpty())
            {
                q = q.Where(w => w.Type == search.Type);
            }
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

        public async Task<TD_Move> GetTheDataAsync(string id)
        {
            return await GetEntityAsync(id);
        }

        public async Task AddDataAsync(TD_Move data)
        {
            await InsertAsync(data);
        }

        public async Task UpdateDataAsync(TD_Move data)
        {
            await UpdateAsync(data);
        }

        public async Task DeleteDataAsync(List<string> ids)
        {
            await DeleteAsync(ids);
        }

        public async Task ApproveDataAsync(string id, string userId)
        {
            await UpdateWhereAsync( w => w.Id == id, 
                e => { e.Status = (int)MoveStatus.审核通过; e.AuditUserId = userId; e.AuditeTime = DateTime.Now; });
        }

        public async Task RejectDataAsync(string id, string userId)
        {
            await UpdateWhereAsync(w => w.Id == id, 
                e => { e.Status = (int)MoveStatus.审核失败; e.AuditUserId = userId; e.AuditeTime = DateTime.Now; });
        }

        public async Task ApproveDatasAsync(List<string> ids, string userId)
        {
            await UpdateWhereAsync(w => ids.Contains(w.Id), 
                e => { e.Status = (int)MoveStatus.审核通过; e.AuditUserId = userId; e.AuditeTime = DateTime.Now; });
        }

        public async Task RejectDatasAsync(List<string> ids, string userId)
        {
            await UpdateWhereAsync(w => ids.Contains(w.Id), 
                e => { e.Status = (int)MoveStatus.审核失败; e.AuditUserId = userId; e.AuditeTime = DateTime.Now; });
        }
        #endregion

        #region 私有成员

        #endregion
    }
}