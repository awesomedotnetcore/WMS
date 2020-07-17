using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coldairarrow.Entity.TD
{
    /// <summary>
    /// 发货表
    /// </summary>
    [Table("TD_Send")]
    public partial class TD_Send
    {

        /// <summary>
        /// Id
        /// </summary>
        [Key, Column(Order = 1)]
        public String Id { get; set; }

        /// <summary>
        /// 仓库ID
        /// </summary>
        public String StorId { get; set; }

        /// <summary>
        /// 发货编号
        /// </summary>
        public String Code { get; set; }

        /// <summary>
        /// 单据日期
        /// </summary>
        public DateTime OrderTime { get; set; }

        /// <summary>
        /// 发货日期
        /// </summary>
        public DateTime SendTime { get; set; }

        /// <summary>
        /// 发货类型
        /// </summary>
        public String Type { get; set; }

        /// <summary>
        /// 关联单号/出库单号
        /// </summary>
        public String RefCode { get; set; }

        /// <summary>
        /// 发货状态：0编制中；1确认；2审批通过；3驳回；4部分出库；5全部出库
        /// </summary>
        public Int32 Status { get; set; }

        /// <summary>
        /// 客户ID
        /// </summary>
        public String CusId { get; set; }

        /// <summary>
        /// 客户地址
        /// </summary>
        public String AddrId { get; set; }

        /// <summary>
        /// 总共数量
        /// </summary>
        public Double TotalNum { get; set; }

        /// <summary>
        /// 发货数量
        /// </summary>
        public Double SendNum { get; set; }

        /// <summary>
        /// 发货金额
        /// </summary>
        public Double TotalAmt { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public String Remarks { get; set; }

        /// <summary>
        /// 确认ID
        /// </summary>
        public String ConfirmUserId { get; set; }

        /// <summary>
        /// 确认时间
        /// </summary>
        public DateTime? ConfirmTime { get; set; }

        /// <summary>
        /// 审核人ID
        /// </summary>
        public String AuditUserId { get; set; }

        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime? AuditeTime { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 创建人ID
        /// </summary>
        public String CreatorId { get; set; }

        /// <summary>
        /// 删除状态：0=未删除；1＝已删除；
        /// </summary>
        public Boolean Deleted { get; set; }

    }
}