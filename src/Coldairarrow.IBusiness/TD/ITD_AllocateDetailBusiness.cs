using Coldairarrow.Entity.TD;
using Coldairarrow.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.TD
{
    public partial interface ITD_AllocateDetailBusiness
    {
        Task<PageResult<TD_AllocateDetail>> GetDataListAsync(PageInput<ConditionDTO> input);
        Task<TD_AllocateDetail> GetTheDataAsync(string id);
        Task AddDataAsync(TD_AllocateDetail data);
        Task UpdateDataAsync(TD_AllocateDetail data);
        Task DeleteDataAsync(List<string> ids);
    }
}