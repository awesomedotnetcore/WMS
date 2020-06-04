using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coldairarrow.Entity.TD
{
    /// <summary>
    /// 出库明细
    /// </summary>
    [Table("TD_OutStorDetail")]
    public partial class TD_OutStorDetail
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
        /// 出库ID
        /// </summary>
        public String OutStorId { get; set; }

        /// <summary>
        /// 货位ID
        /// </summary>
        public String LocalId { get; set; }

        /// <summary>
        /// 托盘ID
        /// </summary>
        public String TrayId { get; set; }

        /// <summary>
        /// 托盘分区ID
        /// </summary>
        public String ZoneId { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public String BarCode { get; set; }

        /// <summary>
        /// 物料ID
        /// </summary>
        public String MaterialId { get; set; }

        /// <summary>
        /// 批次号
        /// </summary>
        public String BatchNo { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        public Double Price { get; set; }

        /// <summary>
        /// 总额
        /// </summary>
        public Double TotalAmt { get; set; }

        /// <summary>
        /// 库存数量
        /// </summary>
        public Double LocalNum { get; set; }

        /// <summary>
        /// 出库数量
        /// </summary>
        public Double OutNum { get; set; }

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