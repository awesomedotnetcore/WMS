using Coldairarrow.Entity.Base;
using Coldairarrow.Util;
using EFCore.Sharding;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Coldairarrow.Business.Base
{
    public class Base_UserStorBusiness : BaseBusiness<Base_UserStor>, IBase_UserStorBusiness, ITransientDependency
    {
        readonly IOperator _operator;
        readonly IServiceProvider _serviceProvider;
        public Base_UserStorBusiness(IRepository repository, IOperator @operator, IServiceProvider serviceProvider)
            : base(repository)
        {
            _operator = @operator;
            _serviceProvider = serviceProvider;
        }

        #region 外部接口

        public async Task<PageResult<Base_UserStor>> GetDataListAsync(PageInput<Base_UserStorQM> input)
        {
            var q = GetIQueryable().Include(i => i.User).Include(i => i.Storage);
            var where = LinqHelper.True<Base_UserStor>();
            var search = input.Search;

            if (!search.UserName.IsNullOrEmpty())
                where = where.And(w => w.User.RealName.Contains(search.UserName) || w.User.UserName.Contains(search.UserName));
            if (!search.StorageName.IsNullOrEmpty())
                where = where.And(w => w.Storage.Name.Contains(search.StorageName) || w.Storage.Code.Contains(search.StorageName));

            return await q.Where(where).GetPageResultAsync(input);
        }

        public async Task<Base_UserStor> GetTheDataAsync(string id)
        {
            return await GetEntityAsync(id);
        }

        public async Task AddDataAsync(Base_UserStor data)
        {
            await InsertAsync(data);
        }

        public async Task UpdateDataAsync(Base_UserStor data)
        {
            await UpdateAsync(data);
        }

        public async Task DeleteDataAsync(List<string> ids)
        {
            await DeleteAsync(ids);
        }
        public async Task<List<Entity.PB.PB_Storage>> GetStorage()
        {
            var storSvc = _serviceProvider.GetRequiredService<PB.IPB_StorageBusiness>();
            var listStor = await storSvc.GetListAsync();
            if (_operator.IsAdmin())
                return listStor;
            var userId = _operator.UserId;
            var userStorIds = await this.GetIQueryable().Where(w => w.UserId == userId).Select(s => s.StorId).ToListAsync();
            return listStor.Where(w => userStorIds.Contains(w.Id)).ToList();
        }
        #endregion

        #region 私有成员

        #endregion
    }
}