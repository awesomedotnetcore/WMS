using Coldairarrow.Entity.TD;
using Coldairarrow.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.TD
{
    public partial interface ITD_OutStorageBusiness
    {
        Task<PageResult<TD_OutStorage>> GetDataListAsync(PageInput<ConditionDTO> input);
        Task<TD_OutStorage> GetTheDataAsync(string id);
        Task AddDataAsync(TD_OutStorage data);
        Task UpdateDataAsync(TD_OutStorage data);
        Task DeleteDataAsync(List<string> ids);
    }
}