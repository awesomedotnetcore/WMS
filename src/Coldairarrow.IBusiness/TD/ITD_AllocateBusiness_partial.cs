using Coldairarrow.Entity.TD;
using Coldairarrow.IBusiness.DTO;
using Coldairarrow.Util;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.TD
{
    public partial interface ITD_AllocateBusiness
    {
        Task<PageResult<TD_Allocate>> GetDataListAsync(TD_AllocatePageInput input);
        Task Approve(AuditDTO audit);
        Task Reject(AuditDTO audit);
    }

    public class TD_AllocateQM
    {
        public string Type { get; set; }
        public string Code { get; set; }
        public string ToStorId { get; set; }
        public DateTime? AllocateTimeStart { get; set; }
        public DateTime? AllocateTimeEnd { get; set; }
        public int? Status { get; set; }
    }

    public class TD_AllocatePageInput : PageInput<TD_AllocateQM>
    {
        public string StorId { get; set; }
    }
}