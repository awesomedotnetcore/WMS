using Coldairarrow.Entity.Base_Manage;
using Coldairarrow.Entity.PB;
using System.Collections.Generic;

namespace Coldairarrow.Util
{
    public partial class Base_UserDTO : Base_User
    {
        /// <summary>
        /// 有权限的仓库
        /// </summary>
        public List<PB_Storage> ListStorage { get; set; }
        /// <summary>
        /// 默认仓库ID
        /// </summary>
        public string DefaultStorageId { get; set; }
    }
}
