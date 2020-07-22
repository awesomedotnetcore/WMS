using Coldairarrow.Business.Base_Manage;
using Coldairarrow.Entity.Base_Manage;
using Coldairarrow.Util;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Api.Controllers.Base_Manage
{
    public partial class Base_UserController : BaseApiController
    {
        [HttpGet]
        public async Task<List<Base_User>> GetSelectData(string keyword, string selected)
        {
            return await _userBus.GetSelectUser(keyword, selected);
        }

        [HttpGet]
        public async Task<Base_User> GetCurUser()
        {
            return await _userBus.GetCurUser();
        }
    }
}