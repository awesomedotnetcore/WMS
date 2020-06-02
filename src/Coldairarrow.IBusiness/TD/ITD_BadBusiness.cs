using Coldairarrow.Entity.TD;
using Coldairarrow.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.TD
{
    public partial interface ITD_BadBusiness
    {
        Task<PageResult<TD_Bad>> GetDataListAsync(PageInput<ConditionDTO> input);
        Task<TD_Bad> GetTheDataAsync(string id);
        Task AddDataAsync(TD_Bad data);
        Task UpdateDataAsync(TD_Bad data);
        Task DeleteDataAsync(List<string> ids);
    }
}