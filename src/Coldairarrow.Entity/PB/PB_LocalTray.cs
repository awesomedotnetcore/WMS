using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coldairarrow.Entity.PB
{
    /// <summary>
    /// 托盘类型与货位对应关系
    /// </summary>
    [Table("PB_LocalTray")]
    public partial class PB_LocalTray
    {

        /// <summary>
        /// 货位ID
        /// </summary>
        [Key, Column(Order = 1)]
        public String LocalId { get; set; }

        [ForeignKey(nameof(LocalId))]
        public PB_Location PB_Location { get; set; }

        /// <summary>
        /// 托盘类型ID
        /// </summary>
        [Key, Column(Order = 2)]
        public String TrayTypeId { get; set; }
        [ForeignKey(nameof(TrayTypeId))]
        public PB_TrayType PB_TrayType { get; set; }

    }
    public class PB_LocalTrayEntityTypeConfig : IEntityTypeConfiguration<PB_LocalTray>
    {
        public void Configure(EntityTypeBuilder<PB_LocalTray> builder)
        {
            builder
                .HasKey(t => new { t.LocalId, t.TrayTypeId });

            builder.HasOne(pt => pt.PB_Location)
                .WithMany(p => p.PB_LocalTrays)
                .HasForeignKey(pt => pt.LocalId);

            builder.HasOne(pt => pt.PB_TrayType)
                .WithMany(p => p.PB_LocalTrays)
                .HasForeignKey(pt => pt.TrayTypeId);
        }
    }
}