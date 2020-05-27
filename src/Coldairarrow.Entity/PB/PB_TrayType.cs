using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coldairarrow.Entity.PB
{
    /// <summary>
    /// 托盘类型
    /// </summary>
    [Table("PB_TrayType")]
    public class PB_TrayType
    {

        /// <summary>
        /// Id
        /// </summary>
        [Key, Column(Order = 1)]
        public String Id { get; set; }

        /// <summary>
        /// 编号
        /// </summary>
        public String Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// 长
        /// </summary>
        public Double? Length { get; set; }

        /// <summary>
        /// 宽
        /// </summary>
        public Double? Width { get; set; }

        /// <summary>
        /// 高
        /// </summary>
        public Double? High { get; set; }

        /// <summary>
        /// 是否有分区
        /// </summary>
        public Boolean? IsZone { get; set; }

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

        public List<PB_TrayMaterial> PB_TrayMaterials { get; set; }

        public List<PB_LocalTray> PB_LocalTrays { get; set; }
    }
}