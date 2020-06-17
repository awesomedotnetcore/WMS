using Coldairarrow.Entity.TD;
using Coldairarrow.IBusiness.DTO;
using Coldairarrow.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.TD
{
    public partial interface ITD_InStorageBusiness
    {
        Task<PageResult<TD_InStorage>> GetDataListAsync(TD_InStoragePageInput input);

        Task Approve(AuditDTO audit);
        Task Reject(AuditDTO audit);

        /// <summary>
        /// 空托盘入库
        /// </summary>
        /// <param name="listTray"></param>
        /// <returns></returns>
        Task InBlankTray(List<KeyValuePair<string, string>> listTray);
    }
}