using Coldairarrow.Entity.PB;
using Coldairarrow.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.PB
{
    public interface IPB_EquipmentBusiness
    {
        Task<PageResult<PB_Equipment>> GetDataListAsync(PageInput<ConditionDTO> input);
        Task<PB_Equipment> GetTheDataAsync(string id);
        Task AddDataAsync(PB_Equipment data);
        Task UpdateDataAsync(PB_Equipment data);
        Task DeleteDataAsync(List<string> ids);
    }
}