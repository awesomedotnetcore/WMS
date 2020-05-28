using Coldairarrow.Entity.TD;
using Coldairarrow.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.TD
{
    public partial interface ITD_OutStorDetailBusiness
    {
        Task<PageResult<TD_OutStorDetail>> GetDataListAsync(PageInput<ConditionDTO> input);
        Task<TD_OutStorDetail> GetTheDataAsync(string id);
        Task AddDataAsync(TD_OutStorDetail data);
        Task UpdateDataAsync(TD_OutStorDetail data);
        Task DeleteDataAsync(List<string> ids);
    }
}