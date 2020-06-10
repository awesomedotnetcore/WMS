using Coldairarrow.Entity.Base_Manage;
using Coldairarrow.Entity.PB;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coldairarrow.Entity.TD
{
    /// <summary>
    /// 调拨表
    /// </summary>
    [Table("TD_Allocate")]
    public partial class TD_Allocate
    {

        /// <summary>
        /// Id
        /// </summary>
        [Key, Column(Order = 1)]
        public String Id { get; set; }

        /// <summary>
        /// 调拨单号
        /// </summary>
        public String Code { get; set; }

        /// <summary>
        /// 调拨时间
        /// </summary>
        public DateTime AllocateTime { get; set; }

        /// <summary>
        /// 调拨类型
        /// </summary>
        public String Type { get; set; }

        /// <summary>
        /// 仓库ID
        /// </summary>
        public String StorId { get; set; }
        [ForeignKey(nameof(StorId))]
        public PB_Storage Src_Storage { get; set; }

        /// <summary>
        /// 目标仓库ID
        /// </summary>
        public String ToStorId { get; set; }
        [ForeignKey(nameof(ToStorId))]
        public PB_Storage Tar_Storage { get; set; }

        /// <summary>
        /// 关联单号
        /// </summary>
        public String RefCode { get; set; }

        /// <summary>
        /// 总额
        /// </summary>
        public Double? Amount { get; set; }

        /// <summary>
        /// 调拨数量
        /// </summary>
        public Double? AllocateNum { get; set; }

        /// <summary>
        /// 设备ID
        /// </summary>
        public String EquId { get; set; }
        [ForeignKey(nameof(EquId))]
        public PB_Equipment PB_Equipment { get; set; }

        /// <summary>
        /// 状态(0待审核;1审核通过;2审核失败)
        /// </summary>
        public Int32 Status { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public String Remarks { get; set; }

        /// <summary>
        /// 审核人ID
        /// </summary>
        public String AuditUserId { get; set; }
        [ForeignKey(nameof(AuditUserId))]
        public Base_User AuditUser { get; set; }

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

    public enum AllocateStatus
    {
        待审核 = 0,
        审核通过,
        审核失败,
        待调拨,
        已调拨
    }
}