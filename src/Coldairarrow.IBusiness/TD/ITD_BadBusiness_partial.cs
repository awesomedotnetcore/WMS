using Coldairarrow.Entity.TD;
using Coldairarrow.IBusiness.DTO;
using Coldairarrow.Util;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.TD
{
    public partial interface ITD_BadBusiness
    {
        Task<PageResult<TD_Bad>> GetDataListAsync(TD_BadPageInput input);
        Task Approve(AuditDTO audit);
        Task Reject(AuditDTO audit);
    }

    public class TD_BadQM
    {
        public string Type { get; set; }
        public string Code { get; set; }
        public DateTime? BadTimeStart { get; set; }
        public DateTime? BadTimeEnd { get; set; }
        public int? Status { get; set; }
    }

    public class TD_BadPageInput : PageInput<TD_BadQM>
    {
        public string StorId { get; set; }
    }
}