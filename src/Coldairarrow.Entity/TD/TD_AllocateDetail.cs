using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coldairarrow.Entity.TD
{
    /// <summary>
    /// 调拨明细
    /// </summary>
    [Table("TD_AllocateDetail")]
    public partial class TD_AllocateDetail
    {

        /// <summary>
        /// Id
        /// </summary>
        [Key, Column(Order = 1)]
        public String Id { get; set; }

        /// <summary>
        /// 调拨ID
        /// </summary>
        public String AllocateId { get; set; }

        /// <summary>
        /// 原仓库ID
        /// </summary>
        public String FromStorId { get; set; }

        /// <summary>
        /// 原货位ID
        /// </summary>
        public String FromLocalId { get; set; }

        /// <summary>
        /// 原托盘ID
        /// </summary>
        public String FromTrayId { get; set; }

        /// <summary>
        /// 原托盘分区ID
        /// </summary>
        public String FromZoneId { get; set; }

        /// <summary>
        /// 目标仓库ID
        /// </summary>
        public String ToStorId { get; set; }

        /// <summary>
        /// 目标货位ID
        /// </summary>
        public String TolocalId { get; set; }

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
        public Double Amount { get; set; }

        /// <summary>
        /// 库存数量
        /// </summary>
        public Double LocalNum { get; set; }

        /// <summary>
        /// 调拨数量
        /// </summary>
        public Double AllocateNum { get; set; }

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