using Coldairarrow.Entity.TD;
using Coldairarrow.IBusiness.DTO;
using Coldairarrow.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.TD
{
    public partial interface ITD_SendBusiness
    {
        Task<PageResult<TD_Send>> GetDataListAsync(TD_SendPageInput input);

       // Task<TD_Send> GetTheDataAsync(string id);
        Task Approval(AuditDTO audit);

        Task UpdateByOutStorage(string id);
    }
}