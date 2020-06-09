using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coldairarrow.Entity.PB
{
    /// <summary>
    /// 货区表
    /// </summary>
    [Table("PB_StorArea")]
    public class PB_StorArea
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
        [ForeignKey(nameof(StorId))]
        public PB_Storage PB_Storage { get; set; }

        /// <summary>
        /// 货区编号
        /// </summary>
        public String Code { get; set; }

        /// <summary>
        /// 货区名称
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// 货区类型
        /// </summary>
        public String Type { get; set; }

        ///// <summary>
        ///// 是否缓存区
        ///// </summary>
        //public Boolean IsCache { get; set; }

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

        public List<PB_AreaMaterial> PB_AreaMaterials { get; set; }

    }
}