using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coldairarrow.Entity.TD
{
    /// <summary>
    /// 收货表
    /// </summary>
    [Table("TD_Receiving")]
    public partial class TD_Receiving
    {

        /// <summary>
        /// 主键
        /// </summary>
        [Key, Column(Order = 1)]
        public String Id { get; set; }

        /// <summary>
        /// 仓库ID
        /// </summary>
        public String StorId { get; set; }

        /// <summary>
        /// 收货单号
        /// </summary>
        public String Code { get; set; }

        /// <summary>
        /// 订单日期
        /// </summary>
        public DateTime OrderTime { get; set; }

        /// <summary>
        /// 收货日期
        /// </summary>
        public DateTime RecTime { get; set; }

        /// <summary>
        /// 收货类型(枚举)
        /// </summary>
        public String Type { get; set; }

        /// <summary>
        /// 关联单号
        /// </summary>
        public String RefCode { get; set; }

        /// <summary>
        /// 状态(0待审核;1确认；2：取消;3审核通过;4审核失败;5部分入库；6全部入库)
        /// </summary>
        public Int32 Status { get; set; }

        /// <summary>
        /// 供应商ID
        /// </summary>
        public String SupId { get; set; }

        /// <summary>
        /// 收货数量
        /// </summary>
        public Double TotalNum { get; set; }

        /// <summary>
        /// 入库数量
        /// </summary>
        public Double InNum { get; set; }

        /// <summary>
        /// 总金额
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