using Coldairarrow.Entity.TD;
using Coldairarrow.IBusiness.DTO;
using Coldairarrow.Util;
using System;
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
    public class TD_SendQM
    {
        public string Keyword { get; set; }
        public DateTime? OrderTimeStart { get; set; }
        public DateTime? OrderTimeEnd { get; set; }
        public string Type { get; set; }
        public int? Status { get; set; }
    }
    public class TD_SendPageInput : PageInput<TD_SendQM>
    {
        public string StorId { get; set; }
    }
}