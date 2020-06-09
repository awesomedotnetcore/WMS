﻿using Coldairarrow.Entity.PB;
using Coldairarrow.Util;
using EFCore.Sharding;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Quartz.Util;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Coldairarrow.Business.PB
{
    public partial class PB_StorAreaBusiness : BaseBusiness<PB_StorArea>, IPB_StorAreaBusiness, ITransientDependency
    {
        public PB_StorAreaBusiness(IRepository repository)
            : base(repository)
        {
        }

        #region 外部接口

        public async Task<PageResult<PB_StorArea>> GetDataListAsync(PageInput<ConditionDTO> input)
        {
            var q = GetIQueryable();
            var where = LinqHelper.True<PB_StorArea>();
            var search = input.Search;
            q = q.Include(i => i.PB_Storage);

            //筛选
            if (!search.Keyword.IsNullOrEmpty())
            {
                where = where.And(w=>w.Name.Contains(search.Keyword) || w.Code.Contains(search.Keyword));
            }
            if (!search.StorageId.IsNullOrWhiteSpace()) where = where.And(p => p.StorId == search.StorageId);
            if (!search.AreaType.IsNullOrWhiteSpace()) where = where.And(p => p.Type == search.AreaType);

            return await q.Where(where).GetPageResultAsync(input);
        }

        public async Task<PB_StorArea> GetTheDataAsync(string id)
        {
            return await GetEntityAsync(id);
        }

        public async Task AddDataAsync(PB_StorArea data)
        {
            await InsertAsync(data);
        }

        public async Task UpdateDataAsync(PB_StorArea data)
        {
            await UpdateAsync(data);
        }

        public async Task DeleteDataAsync(List<string> ids)
        {
            await DeleteAsync(ids);
        }

        public async Task<List<PB_StorArea>> QueryStorAreaDataAsync()
        {
            var q = GetIQueryable();

            return await q.ToListAsync();
        }

        public async Task<List<PB_StorArea>> GetDataListAsync(string storId)
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