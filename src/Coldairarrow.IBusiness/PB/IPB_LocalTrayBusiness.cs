using Coldairarrow.Entity.PB;
using Coldairarrow.IBusiness.DTO;
using Coldairarrow.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.PB
{
    public partial interface IPB_LocalTrayBusiness
    {
        Task<PageResult<PB_LocalTray>> GetDataListAsync(PageInput<ConditionDTO> input);
        Task<List<PB_LocalTray>> GetDataListAsync(string typeId);
        Task<PB_LocalTray> GetTheDataAsync(string id);
        Task AddDataAsync(PB_LocalTray data);
        Task AddDataAsync(PBLocalTrayConditionDTO data);
        Task UpdateDataAsync(PB_LocalTray data);

        Task<int> AddDataAsync(List<PB_LocalTray> datas);
        Task<int> UpdateDataAsync(List<PB_LocalTray> datas);
        Task DeleteDataAsync(string typeId, List<string> localIds);
    }
}