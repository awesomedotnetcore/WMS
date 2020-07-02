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
    public partial class TD_OutStorageBusiness : BaseBusiness<TD_OutStorage>, ITD_OutStorageBusiness, ITransientDependency
    {
        public TD_OutStorageBusiness(IDbAccessor db, IServiceProvider svcProvider)
            : base(db)
        {
            _ServiceProvider = svcProvider;
        }
        readonly IServiceProvider _ServiceProvider;
        #region 外部接口

        //public async Task<PageResult<TD_OutStorage>> GetDataListAsync(TD_InStoragePageInput input)
        //{
        //    var q = GetIQueryable()
        //        .Include(i => i.Supplier)
        //        .Include(i => i.CreateUser)
        //        .Include(i => i.AuditUser)
        //        .Where(w => w.StorId == input.StorId);
        //    var where = LinqHelper.True<TD_OutStorage>();
        //    var search = input.Search;

        //    if (!search.Code.IsNullOrEmpty())
        //        where = where.And(w => w.Code.Contains(search.Code));
        //    if (!search.InType.IsNullOrEmpty())
        //        where = where.And(w => w.OutType == search.OutType);
        //    if (search.InStorTimeStart.HasValue)
        //        where = where.And(w => w.OutStorTime >= search.InStorTimeStart.Value);
        //    if (search.InStorTimeEnd.HasValue)
        //        where = where.And(w => w.OutStorTime <= search.InStorTimeEnd.Value);

        //    return await q.Where(where).GetPageResultAsync(input);
        //}

        //public async Task<TD_OutStorage> GetTheDataAsync(string id)
        //{
        //    return await GetEntityAsync(id);
        //}

        //public async Task AddDataAsync(TD_OutStorage data)
        //{
        //    await InsertAsync(data);
        //}

        //public async Task UpdateDataAsync(TD_OutStorage data)
        //{
        //    await UpdateAsync(data);
        //}

        [DataDeleteLog(UserLogType.出库管理, "Code", "出库单")]
        public async Task DeleteDataAsync(List<string> ids)
        {
            await DeleteAsync(ids);
        }

        #endregion

        #region 私有成员

        #endregion
    }
}