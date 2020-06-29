using Coldairarrow.Business.Base_Manage;
using Coldairarrow.Entity;
using Coldairarrow.Util;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Coldairarrow.Business.Cache
{
    class Base_UserCache : BaseCache<Base_UserDTO>, IBase_UserCache, ITransientDependency
    {
        readonly IDistributedCache _cache;
        readonly IServiceProvider _serviceProvider;
        public Base_UserCache(IServiceProvider serviceProvider, IDistributedCache cache)
            : base(cache)
        {
            _serviceProvider = serviceProvider;
            _cache = cache;
        }

        protected override async Task<Base_UserDTO> GetDbDataAsync(string key)
        {
            PageInput<Base_UsersInputDTO> input = new PageInput<Base_UsersInputDTO>
            {
                Search = new Base_UsersInputDTO
                {
                    all = true,
                    userId = key
                }
            };
            var list = await _serviceProvider.GetService<IBase_UserBusiness>().GetDataListAsync(input);

            var result = list.Data.FirstOrDefault();
            var userStorSvc = _serviceProvider.GetRequiredService<Base.IBase_UserStorBusiness>();
            result.DefaultStorageId = await userStorSvc.GetDefaultStorageId(key);
            return result;
        }
        public async Task UpdateCacheAsync(string id, Base_UserDTO user)
        {
            if (id.IsNullOrEmpty()) return;
            string cacheKey = BuildKey(id);
            await _cache.SetStringAsync(cacheKey, user.ToJson());
        }
    }
}
