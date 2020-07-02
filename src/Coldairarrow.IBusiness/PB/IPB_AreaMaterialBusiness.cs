using Coldairarrow.Entity.PB;
using Coldairarrow.IBusiness.DTO;
using Coldairarrow.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.PB
{
    public partial interface IPB_AreaMaterialBusiness
    {
        Task<PageResult<PB_AreaMaterial>> GetDataListAsync(PageInput<ConditionDTO> input);
        Task<List<PB_AreaMaterial>> GetDataListAsync(string areaId);
        Task<PB_AreaMaterial> GetTheDataAsync(string id);
        Task AddDataAsync(PB_AreaMaterial data);
        Task AddDataAsync(PBAreaMateriaConditionDTO data);
        Task UpdateDataAsync(PB_AreaMaterial data);
        
        Task<int> AddDataAsync(List<PB_AreaMaterial> datas);
        Task<int> UpdateDataAsync(List<PB_AreaMaterial> datas);
        Task DeleteDataAsync(string AreaId, List<string> materialIds);
    }
}