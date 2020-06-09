using Coldairarrow.Entity.TD;
using Coldairarrow.IBusiness.DTO;
using Coldairarrow.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.TD
{
    public partial interface ITD_CheckDataBusiness
    {
        Task ClearDataAsync(string checkId);

        Task PushDataAsync(List<TD_CheckData> data);

        Task<PageResult<TDCheckDataDTO>> QueryDataListAsync(PageInput<TDCheckDataConditionDTO> input);

        Task<List<TDCheckDataDTO>> QueryDataListAsync(string checkId);

        Task<List<TDCheckDataDTO>> AllCheckDataListAsync(string checkId);

        Task ModifyCheckNumAsync(string userId,TDCheckNumModifyDTO data);

        Task<bool> AllCompletedAsync(string CheckId);
    }
}