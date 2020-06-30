using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coldairarrow.Entity.TD
{
    /// <summary>
    /// 盘点表
    /// </summary>
    [Table("TD_Check")]
    public partial class TD_Check
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
        /// 盘点时间
        /// </summary>
        public DateTime CheckTime { get; set; }

        /// <summary>
        /// 盘点类型 整体盘点 区域盘点 特定物料盘点 随机物料盘点(百分比) 
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Code
        /// </summary>
        public string Code { set; get; }

        /// <summary>
        /// 关联单号
        /// </summary>
        public String RefCode { get; set; }

        /// <summary>
        /// 设备ID
        /// </summary>
        public String EquId { get; set; }

        /// <summary>
        /// 是否盘差状态(0待盘 1已盘)
        /// </summary>
        public Boolean? IsComplete { get; set; }

        /// <summary>
        /// 状态(0待审核;1审核通过;2审核失败;3退回)
        /// </summary>
        public Int32? Status { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }

        /// <summary>
        /// 随机百分比
        /// </summary>
        public Int32? RandomPer { get; set; }

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