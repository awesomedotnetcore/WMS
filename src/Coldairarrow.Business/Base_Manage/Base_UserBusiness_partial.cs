using AutoMapper;
using Coldairarrow.Business.Cache;
using Coldairarrow.Entity.Base_Manage;
using Coldairarrow.Util;
using EFCore.Sharding;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Coldairarrow.Business.Base_Manage
{
    public partial class Base_UserBusiness : BaseBusiness<Base_User>, IBase_UserBusiness, ITransientDependency
    {
        public async Task<List<Base_User>> GetSelectUser(string keyword, string selected)
        {
            var queryable = this.GetIQueryable();
            var where = LinqHelper.True<Base_User>();
            if (!selected.IsNullOrEmpty())
                where = where.And(w => w.Id == selected);
            if (!keyword.IsNullOrEmpty())
                where = where.Or(w => w.RealName.Contains(keyword) || w.UserName.Contains(keyword));
            return await queryable.Where(where).Take(20).ToListAsync();
        }
        public async Task<Base_User> GetCurUser()
        {
            var userId = _operator.UserId;
            return await this.GetTheDataAsync(userId);
        }
    }
}