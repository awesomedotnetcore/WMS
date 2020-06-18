using Coldairarrow.Entity.TD;
using Coldairarrow.IBusiness.DTO;
using Coldairarrow.Util;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.TD
{
    public partial interface ITD_OutStorageBusiness
    {
        Task<PageResult<TD_OutStorage>> GetDataListAsync(TD_OutStoragePageInput input);

        Task Approve(AuditDTO audit);
        Task Reject(AuditDTO audit);

        Task<AjaxResult> OutBlankTray(List<KeyValuePair<string, string>> listTray, string storid);
    }
}