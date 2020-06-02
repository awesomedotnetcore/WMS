using Coldairarrow.Entity.TD;
using Coldairarrow.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.TD
{
    public partial interface ITD_BadDetailBusiness
    {
        Task<PageResult<TD_BadDetail>> GetDataListAsync(PageInput<ConditionDTO> input);
        Task<TD_BadDetail> GetTheDataAsync(string id);
        Task AddDataAsync(TD_BadDetail data);
        Task UpdateDataAsync(TD_BadDetail data);
        Task DeleteDataAsync(List<string> ids);
    }
}