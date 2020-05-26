using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coldairarrow.Entity.PB
{
    /// <summary>
    /// 条码
    /// </summary>
    [Table("PB_BarCode")]
    public class PB_BarCode
    {

        /// <summary>
        /// Id
        /// </summary>
        [Key, Column(Order = 1)]
        public String Id { get; set; }

        /// <summary>
        /// BarCode
        /// </summary>
        public String BarCode { get; set; }

        /// <summary>
        /// 条码类型ID
        /// </summary>
        public String BarCodeTypeId { get; set; }

        [ForeignKey(nameof(BarCodeTypeId))]
        public PB_BarCodeType BarCodeType { get; set; }

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