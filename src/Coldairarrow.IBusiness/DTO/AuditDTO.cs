using System;
using System.Collections.Generic;
using System.Text;

namespace Coldairarrow.IBusiness.DTO
{
    /// <summary>
    /// 审计参数
    /// </summary>
    public class AuditDTO
    {
        /// <summary>
        /// 仓库ID
        /// </summary>
        public string StorId { get; set; }
        /// <summary>
        /// 审批单据ID
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 审批人ID
        /// </summary>
        public string AuditUserId { get; set; }
        /// <summary>
        /// 审批时间
        /// </summary>
        public DateTime AuditTime { get; set; }
        /// <summary>
        /// 审批类型
        /// </summary>
        public AuditType AuditType { get; set; }
    }
    /// <summary>
    /// 审批类型
    /// </summary>
    public enum AuditType
    {
        /// <summary>
        /// 确认
        /// </summary>
        Confirm,
        /// <summary>
        /// 取消
        /// </summary>
        Cancel,
        /// <summary>
        /// 通过/同意
        /// </summary>
        Approve,
        /// <summary>
        /// 不通过/驳回
        /// </summary>
        Reject
    }
}
