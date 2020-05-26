using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coldairarrow.Entity.TD
{
    /// <summary>
    /// 入库表
    /// </summary>
    [Table("TD_InStorage")]
    public partial class TD_InStorage
    {

        /// <summary>
        /// Id
        /// </summary>
        [Key, Column(Order = 1)]
        public String Id { get; set; }

        /// <summary>
        /// 收货ID
        /// </summary>
        public String RecId { get; set; }

        /// <summary>
        /// 入库单号
        /// </summary>
        public String Code { get; set; }

        /// <summary>
        /// 入库时间
        /// </summary>
        public DateTime InStorTime { get; set; }

        /// <summary>
        /// 入库类型(枚举)
        /// </summary>
        public String InType { get; set; }

        /// <summary>
        /// 关联单号
        /// </summary>
        public String RefCode { get; set; }

        /// <summary>
        /// 状态(0待审核;1审核通过;2审核失败)
        /// </summary>
        public Int32 Status { get; set; }

        /// <summary>
        /// 仓库ID
        /// </summary>
        public String StorId { get; set; }

        /// <summary>
        /// 目标地址ID
        /// </summary>
        public String AddrId { get; set; }

        /// <summary>
        /// 供应商ID
        /// </summary>
        public String SupId { get; set; }

        public String Remarks { get; set; }

        /// <summary>
        /// 入库数量
        /// </summary>
        public Double TotalNum { get; set; }

        /// <summary>
        /// 总金额
        /// </summary>
        public Double TotalAmt { get; set; }

        /// <summary>
        /// 设备ID
        /// </summary>
        public String EqId { get; set; }

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