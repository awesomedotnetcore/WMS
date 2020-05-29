using Coldairarrow.Entity.TD;
using Coldairarrow.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.TD
{
    public partial interface ITD_CheckBusiness
    {
        Task<PageResult<TD_Check>> GetDataListAsync(PageInput<ConditionDTO> input);
        Task<TD_Check> GetTheDataAsync(string id);
        Task AddDataAsync(TD_Check data);
        Task UpdateDataAsync(TD_Check data);
        Task DeleteDataAsync(List<string> ids);
    }
}