using Coldairarrow.Entity.TD;
using Coldairarrow.Util;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.TD
{
    public partial interface ITD_SendDetailBusiness
    {
        Task AddDataAsync(List<TD_SendDetail> list);
        Task UpdateDataAsync(List<TD_SendDetail> list);

        Task<PageResult<TD_SendDetail>> GetDataListAsync(TD_SendDetailPageInput input);

    }
    
}