using AutoMapper;
using Coldairarrow.Business.PB;
using Coldairarrow.Entity;
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
        readonly Cache.IBase_UserCache _userCache;
        readonly IServiceProvider _serviceProvider;
        readonly IMapper _mapper;
        public Base_UserStorBusiness(IDbAccessor db, Cache.IBase_UserCache userCache, IServiceProvider serviceProvider, IMapper mapper)
            : base(db)
        {
            _serviceProvider = serviceProvider;
            _userCache = userCache;
            _mapper = mapper;
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

        [DataAddLog(UserLogType.仓库权限, "Id", "仓库权限")]
        [DataRepeatValidate(new string[] { "UserId", "StorId" }, new string[] { "用户", "仓库" }, allData: false, matchOr: false)]
        public async Task AddDataAsync(Base_UserStor data)
        {
            await InsertAsync(data);
        }
        public async Task UpdateDefault(string userId)
        {
            await UpdateAsync(w => w.UserId == userId && w.IsDefault, entity => { entity.IsDefault = false; });
        }

        [DataEditLog(UserLogType.仓库权限, "Id", "仓库权限")]
        [DataRepeatValidate(new string[] { "UserId", "StorId" }, new string[] { "用户", "仓库" }, allData: false, matchOr: false)]
        public async Task UpdateDataAsync(Base_UserStor data)
        {
            await UpdateAsync(data);
        }

        [DataDeleteLog(UserLogType.仓库权限, "Id", "仓库权限")]
        public async Task DeleteDataAsync(List<string> ids)
        {
            await DeleteAsync(ids);
        }
        public async Task<List<PB_StorageDTO>> GetStorage(string userId)
        {
            var storSvc = _serviceProvider.GetRequiredService<PB.IPB_StorageBusiness>();
            var listStor = await storSvc.GetListAsync();
            var storDto = _mapper.Map<List<Entity.PB.PB_Storage>, List<PB_StorageDTO>>(listStor);
            storDto = storDto.OrderByDescending(o => o.IsDefault).ToList();
            var defaultStorId = storDto.SingleOrDefault(w => w.IsDefault)?.Id;//系统默认仓库Id
            var userStors = await this.GetIQueryable().Where(w => w.UserId == userId).OrderByDescending(o => o.IsDefault).ToListAsync();
            //过滤用户有权限的仓库
            var userSvc = _serviceProvider.GetRequiredService<Base_Manage.IBase_UserBusiness>();
            var user = await userSvc.GetTheDataAsync(userId);
            if (!this.IsAdmin(user) && userStors.Count > 0)
            {
                var userStorIds = userStors.Select(s => s.StorId).ToList();
                storDto = storDto.Where(w => userStorIds.Contains(w.Id)).ToList();
            }
            //设置用户的默认仓库
            var defaultUserStorId = userStors.SingleOrDefault(w => w.IsDefault)?.StorId;//用户默认仓库ID
            var defaultId = defaultUserStorId.IsNullOrEmpty() ? defaultStorId : defaultUserStorId;
            if (!defaultId.IsNullOrEmpty())
                storDto.ForEach(item => { item.IsDefault = item.Id == defaultId; });
            else
            {
                var first = storDto.FirstOrDefault();
                first.IsDefault = true;
            }
            return storDto;
        }
        private bool IsAdmin(Base_UserDTO user)
        {
            var role = user.RoleType;
            if (user.Id == GlobalData.ADMINID || role.HasFlag(RoleTypes.超级管理员))
                return true;
            else
                return false;
        }
        public async Task<string> GetDefaultStorageId(string userId)
        {
            var listStor = await this.GetStorage(userId);
            return listStor.SingleOrDefault(w => w.IsDefault)?.Id;
        }
        public async Task SwitchDefault(string userId, string storageId)
        {
            await UpdateAsync(w => w.UserId == userId && w.IsDefault, (entity) => { entity.IsDefault = false; });
            var userStorage = await this.GetIQueryable().SingleOrDefaultAsync(w => w.UserId == userId && w.StorId == storageId);
            if (userStorage != null)
            {
                userStorage.IsDefault = true;
                await UpdateDataAsync(userStorage);
                await _userCache.UpdateCacheAsync(userId);
            }
            else
            {
                var userSvc = _serviceProvider.GetRequiredService<Base_Manage.IBase_UserBusiness>();
                var user = await userSvc.GetTheDataAsync(userId);
                user.DefaultStorageId = storageId;
                await _userCache.UpdateCacheAsync(userId, user);
            }
        }
        #endregion

        #region 私有成员

        #endregion
    }
}