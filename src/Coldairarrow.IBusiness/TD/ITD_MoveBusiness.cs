using Coldairarrow.Entity.TD;
using Coldairarrow.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.TD
{
    public partial interface ITD_MoveBusiness
    {
        Task<PageResult<TD_Move>> GetDataListAsync(PageInput<ConditionDTO> input);
        Task<TD_Move> GetTheDataAsync(string id);
        Task AddDataAsync(TD_Move data);
        Task UpdateDataAsync(TD_Move data);
        Task DeleteDataAsync(List<string> ids);
    }
}