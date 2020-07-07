using Coldairarrow.Entity.TD;
using Coldairarrow.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.TD
{
    public partial interface ITD_ReceivingBusiness
    {
        Task<PageResult<TD_Receiving>> GetDataListAsync(PageInput<ConditionDTO> input);
        Task<TD_Receiving> GetTheDataAsync(string id);
        Task AddDataAsync(TD_Receiving data);
        Task UpdateDataAsync(TD_Receiving data);
        Task DeleteDataAsync(List<string> ids);
    }
}