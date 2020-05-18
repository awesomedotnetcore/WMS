using Coldairarrow.Entity.Base;
using Coldairarrow.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.Base
{
    public interface IBase_EnumItemBusiness
    {
        Task<PageResult<Base_EnumItem>> GetDataListAsync(PageInput<ConditionDTO> input);
        Task<Base_EnumItem> GetTheDataAsync(string id);
        Task AddDataAsync(Base_EnumItem data);
        Task UpdateDataAsync(Base_EnumItem data);
        Task DeleteDataAsync(List<string> ids);
    }
}