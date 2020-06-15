using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coldairarrow.Entity.PB
{
    /// <summary>
    /// 货架管理
    /// </summary>
    [Table("PB_Rack")]
    public class PB_Rack
    {

        /// <summary>
        /// Id
        /// </summary>
        [Key, Column(Order = 1)]
        public String Id { get; set; }

        /// <summary>
        /// 货架编号
        /// </summary>
        public String Code { get; set; }

        /// <summary>
        /// 货架名称
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// 仓库ID
        /// </summary>
        public String StorId { get; set; }
        [ForeignKey(nameof(StorId))]
        public PB_Storage PB_Storage { get; set; }

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