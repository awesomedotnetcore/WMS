using Coldairarrow.Entity.TD;
using Coldairarrow.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.TD
{
    public partial interface ITD_InStorDetailBusiness
    {
        Task<PageResult<TD_InStorDetail>> GetDataListAsync(PageInput<ConditionDTO> input);
        Task<TD_InStorDetail> GetTheDataAsync(string id);
        Task AddDataAsync(TD_InStorDetail data);
        Task UpdateDataAsync(TD_InStorDetail data);
        Task DeleteDataAsync(List<string> ids);
    }
}