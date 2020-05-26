using Coldairarrow.Entity.TD;
using Coldairarrow.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.TD
{
    public partial interface ITD_CheckDataBusiness
    {
        Task<PageResult<TD_CheckData>> GetDataListAsync(PageInput<ConditionDTO> input);
        Task<TD_CheckData> GetTheDataAsync(string id);
        Task AddDataAsync(TD_CheckData data);
        Task UpdateDataAsync(TD_CheckData data);
        Task DeleteDataAsync(List<string> ids);
    }
}