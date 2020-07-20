using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coldairarrow.Entity.TD
{
    /// <summary>
    /// 出库表
    /// </summary>
    [Table("TD_OutStorage")]
    public partial class TD_OutStorage
    {

        /// <summary>
        /// Id
        /// </summary>
        [Key, Column(Order = 1)]
        public String Id { get; set; }

        /// <summary>
        /// 发货ID
        /// </summary>
        public String SendId { get; set; }

        /// <summary>
        /// 仓库ID
        /// </summary>
        public String StorageId { get; set; }

        /// <summary>
        /// 出库单号
        /// </summary>
        public String Code { get; set; }

        /// <summary>
        /// 出库时间
        /// </summary>
        public DateTime OutTime { get; set; }

        /// <summary>
        /// 出库类型(枚举)
        /// </summary>
        public String OutType { get; set; }

        /// <summary>
        /// 关联单号
        /// </summary>
        public String RefCode { get; set; }

        /// <summary>
        /// 出库数量
        /// </summary>
        public Double OutNum { get; set; }

        /// <summary>
        /// 总金额
        /// </summary>
        public Double TotalAmt { get; set; }

        /// <summary>
        /// 设备ID
        /// </summary>
        public String EquId { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public Int32 Status { get; set; }

        /// <summary>
        /// 客户ID
        /// </summary>
        public String CusId { get; set; }

        /// <summary>
        /// 目标地址ID
        /// </summary>
        public String AddrId { get; set; }

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