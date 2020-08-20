using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coldairarrow.Entity.PB
{
    /// <summary>
    /// 托盘表
    /// </summary>
    [Table("PB_Tray")]
    public partial class PB_Tray
    {

        /// <summary>
        /// Id
        /// </summary>
        [Key, Column(Order = 1)]
        public String Id { get; set; }

        /// <summary>
        /// 货位ID(空托盘出库情况)
        /// </summary>
        public String LocalId { get; set; }

        [ForeignKey(nameof(LocalId))]
        public PB_Location PB_Location { get; set; }

        /// <summary>
        /// 托盘号
        /// </summary>
        public String Code { get; set; }

        /// <summary>
        /// 托盘名称
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// 托盘类型ID
        /// </summary>
        public String TrayTypeId { get; set; }

        [ForeignKey(nameof(TrayTypeId))]
        public PB_TrayType PB_TrayType { get; set; }

        /// <summary>
        /// 启用日期
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 托盘状态:0启用 1停用
        /// </summary>
        public Int32 Status { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public String Remarks { get; set; }

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