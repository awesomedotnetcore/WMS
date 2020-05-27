using Coldairarrow.Entity.IT;
using Coldairarrow.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.IT
{
    public partial interface IIT_LocalMaterialBusiness
    {
        Task<PageResult<IT_LocalMaterial>> GetDataListAsync(PageInput<ConditionDTO> input);
        Task<IT_LocalMaterial> GetTheDataAsync(string id);
        Task AddDataAsync(IT_LocalMaterial data);
        Task UpdateDataAsync(IT_LocalMaterial data);
        Task DeleteDataAsync(List<string> ids);
    }
}