using Coldairarrow.Entity.TD;
using Coldairarrow.IBusiness.DTO;
using Coldairarrow.Util;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.TD
{
    public partial interface ITD_MoveBusiness
    {
        Task<PageResult<TD_Move>> GetDataListAsync(TD_MovePageInput input);
        Task Approve(AuditDTO audit);
        Task Reject(AuditDTO audit);
    }

    public class TD_MoveQM
    {
        public string Type { get; set; }
        public string Code { get; set; }
        public DateTime? BadTimeStart { get; set; }
        public DateTime? BadTimeEnd { get; set; }
        public int? Status { get; set; }
    }

    public class TD_MovePageInput : PageInput<TD_MoveQM>
    {
        public string StorId { get; set; }
    }
}