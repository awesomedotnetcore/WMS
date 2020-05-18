using Coldairarrow.Entity.Base;
using Coldairarrow.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.Base
{
    public interface IBase_ParameterBusiness
    {
        Task<PageResult<Base_Parameter>> GetDataListAsync(PageInput<ConditionDTO> input);
        Task<Base_Parameter> GetTheDataAsync(string id);
        Task AddDataAsync(Base_Parameter data);
        Task UpdateDataAsync(Base_Parameter data);
        Task DeleteDataAsync(List<string> ids);
    }
}