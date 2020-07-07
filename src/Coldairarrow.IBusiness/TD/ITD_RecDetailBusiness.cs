using Coldairarrow.Entity.TD;
using Coldairarrow.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.TD
{
    public partial interface ITD_RecDetailBusiness
    {
        Task<PageResult<TD_RecDetail>> GetDataListAsync(PageInput<ConditionDTO> input);
        Task<TD_RecDetail> GetTheDataAsync(string id);
        Task AddDataAsync(TD_RecDetail data);
        Task UpdateDataAsync(TD_RecDetail data);
        Task DeleteDataAsync(List<string> ids);
    }
}