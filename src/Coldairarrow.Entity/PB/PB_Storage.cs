using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coldairarrow.Entity.PB
{
    /// <summary>
    /// 仓库表
    /// </summary>
    [Table("PB_Storage")]
    public class PB_Storage
    {

        /// <summary>
        /// 主键ID
        /// </summary>
        [Key, Column(Order = 1)]
        public String Id { get; set; }

        /// <summary>
        /// 仓库编号
        /// </summary>
        public String Code { get; set; }

        /// <summary>
        /// 仓库名称
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// 仓库类型（平库,立库）(枚举)
        /// </summary>
        public String Type { get; set; }

        /// <summary>
        /// 是否启用托盘管理 0  禁用：1启用
        /// </summary>
        public Boolean IsTray { get; set; }

        /// <summary>
        /// 事情启用分区管理 0  禁用：1启用
        /// </summary>
        public Boolean IsZone { get; set; }

        /// <summary>
        /// 启用：0  禁用：1启用
        /// </summary>
        public Boolean Disable { get; set; }

        /// <summary>
        /// 默认
        /// </summary>
        public Boolean IsDefault { get; set; }

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