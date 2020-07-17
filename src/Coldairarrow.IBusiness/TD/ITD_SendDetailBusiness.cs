using Coldairarrow.Entity.TD;
using Coldairarrow.Util;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.TD
{
    public partial interface ITD_SendDetailBusiness
    {
        Task<PageResult<TD_SendDetail>> GetDataListAsync(PageInput<ConditionDTO> input);
        Task<TD_SendDetail> GetTheDataAsync(string id);
        Task AddDataAsync(TD_SendDetail data);
        Task UpdateDataAsync(TD_SendDetail data);
        Task DeleteDataAsync(List<string> ids);
    }
    
}