using Coldairarrow.Entity.Base;
using Coldairarrow.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.Base
{
    public interface IBase_UserStorBusiness
    {
        Task<PageResult<Base_UserStor>> GetDataListAsync(PageInput<Base_UserStorQM> input);
        Task<Base_UserStor> GetTheDataAsync(string id);
        Task AddDataAsync(Base_UserStor data);
        Task UpdateDataAsync(Base_UserStor data);
        Task DeleteDataAsync(List<string> ids);

        /// <summary>
        /// 修改默认为False
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task UpdateDefault(string userId);
        /// <summary>
        /// 获取当前用户有权限的仓库
        /// </summary>
        /// <returns></returns>
        Task<List<PB.PB_StorageDTO>> GetStorage(string userId);


        Task SwitchDefault(string userId, string storageId);

        Task<string> GetDefaultStorageId(string userId);
    }
    public class Base_UserStorQM
    {
        public string UserName { get; set; }
        public string StorageName { get; set; }
    }
}