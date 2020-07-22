using Coldairarrow.Entity.Base_Manage;
using Coldairarrow.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.Base_Manage
{
    public partial interface IBase_UserBusiness
    {
        Task<List<Base_User>> GetSelectUser(string keyword, string selected);
        Task<Base_User> GetCurUser();
    }
}