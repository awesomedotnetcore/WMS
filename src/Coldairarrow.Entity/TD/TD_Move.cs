using Coldairarrow.Entity.Base_Manage;
using Coldairarrow.Entity.PB;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coldairarrow.Entity.TD
{
    /// <summary>
    /// 移库表
    /// </summary>
    [Table("TD_Move")]
    public partial class TD_Move
    {

        /// <summary>
        /// Id
        /// </summary>
        [Key, Column(Order = 1)]
        public String Id { get; set; }

        /// <summary>
        /// 移库单号
        /// </summary>
        public String Code { get; set; }

        /// <summary>
        /// 移库时间
        /// </summary>
        public DateTime MoveTime { get; set; }

        /// <summary>
        /// 移库类型(枚举)
        /// </summary>
        public String Type { get; set; }

        /// <summary>
        /// 关联单号
        /// </summary>
        public String RefCode { get; set; }

        /// <summary>
        /// 仓库ID
        /// </summary>
        public String StorId { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public double MoveNum { get; set; }

        /// <summary>
        /// 总额
        /// </summary>
        public double TotalAmt { get; set; }

        /// <summary>
        /// 设备ID
        /// </summary>
        public String EquId { get; set; }

        /// <summary>
        /// 状态(0待审核;1审核通过;2审核失败)待移库；已移库
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