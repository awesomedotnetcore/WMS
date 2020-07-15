using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coldairarrow.Entity.PB
{
    /// <summary>
    /// 条码规则
    /// </summary>
    [Table("PB_BarCodeRule")]
    public partial class PB_BarCodeRule
    {

        /// <summary>
        /// Id
        /// </summary>
        [Key, Column(Order = 1)]
        public String Id { get; set; }

        /// <summary>
        /// BarCodeId
        /// </summary>
        public String TypeId { get; set; }

        /// <summary>
        /// 类型
        /// 常量 日期 流水号 引用 每日流水号 随机数(GUID) 参数
        /// </summary>
        public String Type { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public String Sort { get; set; }

        /// <summary>
        /// 规则
        /// </summary>
        public String Rule { get; set; }

        /// <summary>
        /// 长度
        /// </summary>
        public Int32? length { get; set; }

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