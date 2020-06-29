using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coldairarrow.Entity.PB
{
    /// <summary>
    /// 货位管理
    /// </summary>
    public partial class PB_Location
    {
        /// <summary>
        /// 仓库
        /// </summary>
        [ForeignKey(nameof(StorId))]
        public PB_Storage PB_Storage { get; set; }

        /// <summary>
        /// 区域
        /// </summary>

        [ForeignKey(nameof(AreaId))]
        public PB_StorArea PB_StorArea { get; set; }

        /// <summary>
        /// 巷道
        /// </summary>
        [ForeignKey(nameof(LanewayId))]
        public PB_Laneway PB_Laneway { get; set; }

        /// <summary>
        /// 货架
        /// </summary>
        [ForeignKey(nameof(RackId))]
        public PB_Rack PB_Rack { get; set; }

        /// <summary>
        /// 托盘类型与货位对应关系
        /// </summary>
        public List<PB_LocalTray> PB_LocalTrays { get; set; }

    }
}