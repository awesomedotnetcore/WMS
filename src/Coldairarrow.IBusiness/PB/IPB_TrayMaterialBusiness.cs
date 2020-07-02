using Coldairarrow.Entity.PB;
using Coldairarrow.IBusiness.DTO;
using Coldairarrow.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.PB
{
    public interface IPB_TrayMaterialBusiness
    {
        Task<PageResult<PB_TrayMaterial>> GetDataListAsync(PageInput<ConditionDTO> input);
        Task<List<PB_TrayMaterial>> GetDataListAsync(string typeId);
        Task<PB_TrayMaterial> GetTheDataAsync(string id);
        Task AddDataAsync(PB_TrayMaterial data);
        Task AddDataAsync(PBTrayMateriaConditionDTO data);
        Task UpdateDataAsync(PB_TrayMaterial data);
        Task<int> AddDataAsync(List<PB_TrayMaterial> datas);
        Task<int> UpdateDataAsync(List<PB_TrayMaterial> datas);
        Task DeleteDataAsync(string trayTypeId, List<string> materialIds);
    }
}