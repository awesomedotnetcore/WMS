using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coldairarrow.Entity.PD
{
    /// <summary>
    /// 计划表
    /// </summary>
    [Table("PD_Plan")]
    public partial class PD_Plan
    {

        /// <summary>
        /// Id
        /// </summary>
        [Key, Column(Order = 1)]
        public String Id { get; set; }

        /// <summary>
        /// 计划编号
        /// </summary>
        public String Code { get; set; }

        /// <summary>
        /// 物料Id
        /// </summary>
        public String MaterialId { get; set; }

        /// <summary>
        /// Bom版本
        /// </summary>
        public String BomVerId { get; set; }

        /// <summary>
        /// 批次号
        /// </summary>
        public String BatchNo { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public Double Num { get; set; }

        /// <summary>
        /// 计划日期
        /// </summary>
        public DateTime PlanDate { get; set; }

        /// <summary>
        /// 计划开始日期
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// 计划完成日期
        /// </summary>
        public DateTime FinishDate { get; set; }

        /// <summary>
        /// 生产单元
        /// </summary>
        public String UnitCode { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public Int32 Status { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public String Remark { get; set; }

        /// <summary>
        /// 订单编号
        /// </summary>
        public String OrderNo { get; set; }

        /// <summary>
        /// 上级系统Id
        /// </summary>
        public String RefId { get; set; }

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