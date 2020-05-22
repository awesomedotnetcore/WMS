using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coldairarrow.Entity.PB
{
    /// <summary>
    /// 条码类型管理
    /// </summary>
    [Table("PB_BarCodeType")]
    public class PB_BarCodeType
    {

        /// <summary>
        /// Id
        /// </summary>
        [Key, Column(Order = 1)]
        public String Id { get; set; }

        /// <summary>
        /// 编号（业务表名称）
        /// </summary>
        public String Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// 流水号
        /// </summary>
        public int? SeqNum { get; set; }

        /// <summary>
        /// 当前流水日期
        /// </summary>
        public DateTime? SeqDate { get; set; }

        /// <summary>
        /// 连接符
        /// </summary>
        public String JoinChar { get; set; }

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

        public List<PB_BarCodeRule> BarCodeRules { get; set; }

    }

    //public class Base_EnumEntityTypeConfig : IEntityTypeConfiguration<PB_BarCodeType>
    //{
    //    public void Configure(EntityTypeBuilder<PB_BarCodeType> builder)
    //    {
    //        builder
    //        .HasMany(b => b.BarCodeRules)
    //        .WithOne()
    //        .HasForeignKey(k => k.TypeId);
    //    }
    //}
}