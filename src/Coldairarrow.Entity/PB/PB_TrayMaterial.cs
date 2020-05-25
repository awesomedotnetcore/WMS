using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Coldairarrow.Entity.PB
{
    /// <summary>
    /// 托盘类型与物料对应关系
    /// </summary>
    [Table("PB_TrayMaterial")]
    public class PB_TrayMaterial
    {

        /// <summary>
        /// 物料ID
        /// </summary>
        [Key, Column(Order = 1)]
        public String MaterialId { get; set; }

        [ForeignKey(nameof(MaterialId))]
        public PB_Material PB_Material { get; set; }

        /// <summary>
        /// 托盘类型ID
        /// </summary>
        [Key, Column(Order = 2)]
        public String TrayTypeId { get; set; }

        [ForeignKey(nameof(TrayTypeId))]
        public PB_TrayType PB_TrayType { get; set; }

    }

    public class PB_TrayMaterialEntityTypeConfig : IEntityTypeConfiguration<PB_TrayMaterial>
    {
        public void Configure(EntityTypeBuilder<PB_TrayMaterial> builder)
        {
            builder
                .HasKey(t => new { t.MaterialId, t.TrayTypeId });

            builder.HasOne(pt => pt.PB_Material)
                .WithMany(p => p.PB_TrayMaterials)
                .HasForeignKey(pt => pt.MaterialId);

            builder.HasOne(pt => pt.PB_TrayType)
                .WithMany(p => p.PB_TrayMaterials)
                .HasForeignKey(pt => pt.TrayTypeId);
        }
    }
}