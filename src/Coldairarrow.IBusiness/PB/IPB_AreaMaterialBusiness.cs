using Coldairarrow.Entity.PB;
using Coldairarrow.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.PB
{
    public interface IPB_AreaMaterialBusiness
    {
        Task<PageResult<PB_AreaMaterial>> GetDataListAsync(PageInput<ConditionDTO> input);
        Task<PB_AreaMaterial> GetTheDataAsync(string id);
        Task AddDataAsync(PB_AreaMaterial data);
        Task UpdateDataAsync(PB_AreaMaterial data);
        Task DeleteDataAsync(List<string> ids);
    }
}