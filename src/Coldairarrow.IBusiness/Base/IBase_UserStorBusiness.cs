using Coldairarrow.Entity.Base;
using Coldairarrow.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.Base
{
    public interface IBase_UserStorBusiness
    {
        Task<PageResult<Base_UserStor>> GetDataListAsync(PageInput<ConditionDTO> input);
        Task<Base_UserStor> GetTheDataAsync(string id);
        Task AddDataAsync(Base_UserStor data);
        Task UpdateDataAsync(Base_UserStor data);
        Task DeleteDataAsync(List<string> ids);
    }
}