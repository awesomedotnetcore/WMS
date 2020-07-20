using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coldairarrow.Entity.TD
{
    /// <summary>
    /// 发货明细
    /// </summary>
    [Table("TD_SendDetail")]
    public partial class TD_SendDetail
    {

        /// <summary>
        /// Id
        /// </summary>
        [Key, Column(Order = 1)]
        public String Id { get; set; }

        /// <summary>
        /// 发货Id
        /// </summary>
        public String SendId { get; set; }

        /// <summary>
        /// 仓库ID
        /// </summary>
        public String StorId { get; set; }

        /// <summary>
        /// 货位ID
        /// </summary>
        public String LocalId { get; set; }

        /// <summary>
        /// 物料ID
        /// </summary>
        public String MaterialId { get; set; }

        /// <summary>
        /// 单位ID
        /// </summary>
        public String MeasureId { get; set; }

        /// <summary>
        /// 批次号
        /// </summary>
        public String BatchNo { get; set; }

        /// <summary>
        /// 库存数量
        /// </summary>
        public Double LocalNum { get; set; }

        /// <summary>
        /// 计划数量
        /// </summary>
        public Double PlanNum { get; set; }

        /// <summary>
        /// 发货数量
        /// </summary>
        public Double SendNum { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        public Double Price { get; set; }

        /// <summary>
        /// 总价
        /// </summary>
        public Double Amount { get; set; }

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