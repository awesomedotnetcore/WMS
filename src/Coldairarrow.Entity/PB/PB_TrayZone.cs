using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coldairarrow.Entity.PB
{
    /// <summary>
    /// 托盘类型分区
    /// </summary>
    [Table("PB_TrayZone")]
    public class PB_TrayZone
    {

        /// <summary>
        /// Id
        /// </summary>
        [Key, Column(Order = 1)]
        public String Id { get; set; }

        /// <summary>
        /// 托盘类型ID
        /// </summary>
        public String TrayTypeId { get; set; }

        /// <summary>
        /// 托盘分区编号
        /// </summary>
        public String Code { get; set; }

        /// <summary>
        /// 托盘分区名称
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// X
        /// </summary>
        public Int32? X { get; set; }

        /// <summary>
        /// Y
        /// </summary>
        public Int32? Y { get; set; }

        /// <summary>
        /// 是否默认托盘分区
        /// </summary>
        public Boolean? IsDefault { get; set; }

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