using Coldairarrow.Entity.TD;
using Coldairarrow.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.TD
{
    public partial interface ITD_AllocateBusiness
    {
        Task<PageResult<TD_Allocate>> GetDataListAsync(PageInput<ConditionDTO> input, string storageId);
        Task<List<TD_Allocate>> GetDataListAsync(List<string> ids);
        Task<TD_Allocate> GetTheDataAsync(string id);
        Task AddDataAsync(TD_Allocate data);
        Task UpdateDataAsync(TD_Allocate data);
        Task DeleteDataAsync(List<string> ids);
        Task ApproveDatasAsync(List<string> ids, string userId);
        Task RejectDatasAsync(List<string> ids, string userId);
    }
}