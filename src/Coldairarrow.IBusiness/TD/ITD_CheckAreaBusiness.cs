using Coldairarrow.Entity.TD;
using Coldairarrow.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.TD
{
    public partial interface ITD_CheckAreaBusiness
    {
        Task<PageResult<TD_CheckArea>> GetDataListAsync(PageInput<ConditionDTO> input);
        Task<TD_CheckArea> GetTheDataAsync(string id);
        Task AddDataAsync(TD_CheckArea data);
        Task UpdateDataAsync(TD_CheckArea data);
        Task DeleteDataAsync(List<string> ids);
    }
}