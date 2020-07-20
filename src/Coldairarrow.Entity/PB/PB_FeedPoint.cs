using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coldairarrow.Entity.PB
{
    /// <summary>
    /// 进料点/出料点
    /// </summary>
    [Table("PB_FeedPoint")]
    public partial class PB_FeedPoint
    {

        /// <summary>
        /// 主键ID
        /// </summary>
        [Key, Column(Order = 1)]
        public String Id { get; set; }

        /// <summary>
        /// 仓库Id
        /// </summary>
        public String StorId { get; set; }

        /// <summary>
        /// 巷道Id
        /// </summary>
        public String LaneId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public String Code { get; set; }

        /// <summary>
        /// 类型：只进/进出/只出/出回
        /// </summary>
        public String Type { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public Boolean IsEnable { get; set; }

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