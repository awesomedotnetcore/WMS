using Coldairarrow.Entity.PB;
using Coldairarrow.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.PB
{
    public interface IPB_StorageBusiness
    {
        Task<PageResult<PB_Storage>> GetDataListAsync(PageInput<ConditionDTO> input);
        Task<PB_Storage> GetTheDataAsync(string id);
        Task AddDataAsync(PB_Storage data);
        Task UpdateDataAsync(PB_Storage data);
        Task DeleteDataAsync(List<string> ids);
    }
}