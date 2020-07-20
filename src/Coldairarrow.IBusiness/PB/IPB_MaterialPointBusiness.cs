using Coldairarrow.Entity.PB;
using Coldairarrow.Util;
using System.Collections.Generic;
using System.Threading.Tasks;
using Coldairarrow.IBusiness.DTO;

namespace Coldairarrow.Business.PB
{
    public partial interface IPB_MaterialPointBusiness
    {
        Task<PageResult<PB_MaterialPoint>> GetDataListAsync(PageInput<ConditionDTO> input);
        Task<List<PB_MaterialPoint>> GetDataListAsync(string pointId);
        Task<PB_MaterialPoint> GetTheDataAsync(string id);
        Task AddDataAsync(PB_MaterialPoint data);
        Task AddDataAsync(PBMaterialPointConditionDTO data);
        Task UpdateDataAsync(PB_MaterialPoint data);

        Task<int> AddDataAsync(List<PB_MaterialPoint> datas);
        Task<int> UpdateDataAsync(List<PB_MaterialPoint> datas);
        Task DeleteDataAsync(string PointId, List<string> materialIds);
    }
}