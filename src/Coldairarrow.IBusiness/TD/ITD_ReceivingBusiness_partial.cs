using Coldairarrow.Entity.TD;
using Coldairarrow.IBusiness.DTO;
using Coldairarrow.Util;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.TD
{
    public partial interface ITD_ReceivingBusiness
    {
        Task<PageResult<TD_Receiving>> GetDataListAsync(TD_ReceivingPageInput input);
        Task Approval(AuditDTO audit);

        Task UpdateByInStorage(string id);

        Task AutoInStorage(string id);
    }
    public class TD_ReceivingQM
    {
        public string Keyword { get; set; }
        public DateTime? OrderTimeStart { get; set; }
        public DateTime? OrderTimeEnd { get; set; }
        public string Type { get; set; }
        public int? Status { get; set; }
    }
    public class TD_ReceivingPageInput : PageInput<TD_ReceivingQM>
    {
        public string StorId { get; set; }
    }
}