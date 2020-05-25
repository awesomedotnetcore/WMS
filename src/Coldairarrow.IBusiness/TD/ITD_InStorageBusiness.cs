using Coldairarrow.Entity.TD;
using Coldairarrow.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.TD
{
    public interface ITD_InStorageBusiness
    {
        Task<PageResult<TD_InStorage>> GetDataListAsync(PageInput<ConditionDTO> input);
        Task<TD_InStorage> GetTheDataAsync(string id);
        Task AddDataAsync(TD_InStorage data);
        Task UpdateDataAsync(TD_InStorage data);
        Task DeleteDataAsync(List<string> ids);
    }
}