using Coldairarrow.Entity.PB;
using Coldairarrow.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.PB
{
    public interface IPB_TrayMaterialBusiness
    {
        Task<PageResult<PB_TrayMaterial>> GetDataListAsync(PageInput<ConditionDTO> input);
        Task<PB_TrayMaterial> GetTheDataAsync(string id);
        Task AddDataAsync(PB_TrayMaterial data);
        Task UpdateDataAsync(PB_TrayMaterial data);
        Task DeleteDataAsync(string trayTypeId, List<string> materialIds);
    }
}