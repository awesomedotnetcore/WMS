using Coldairarrow.Entity.TD;
using Coldairarrow.Util;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.TD
{
    public partial interface ITD_SendBusiness
    {
        //Task<PageResult<TD_Send>> GetDataListAsync(PageInput<ConditionDTO> input);
        Task<TD_Send> GetTheDataAsync(string id);
        Task AddDataAsync(TD_Send data);
        Task UpdateDataAsync(TD_Send data);
        Task DeleteDataAsync(List<string> ids);
    }

}