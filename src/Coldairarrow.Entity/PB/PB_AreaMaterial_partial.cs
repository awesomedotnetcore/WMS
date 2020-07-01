using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coldairarrow.Entity.PB
{
    /// <summary>
    /// 货区物料关系表
    /// </summary>
    public partial class PB_AreaMaterial
    {

        [ForeignKey(nameof(AreaId))]
        public PB_StorArea PB_StorArea { get; set; }

        [ForeignKey(nameof(MaterialId))]
        public PB_Material PB_Material { get; set; }

    }

    public class PB_AreaMaterialEntityTypeConfig : IEntityTypeConfiguration<PB_AreaMaterial>
    {
        public void Configure(EntityTypeBuilder<PB_AreaMaterial> builder)
        {
            builder
                .HasKey(t => new { t.MaterialId, t.AreaId });

            builder.HasOne(pt => pt.PB_Material)
                .WithMany(p => p.PB_AreaMaterials)
                .HasForeignKey(pt => pt.MaterialId);

            builder.HasOne(pt => pt.PB_StorArea)
                .WithMany(p => p.PB_AreaMaterials)
                .HasForeignKey(pt => pt.AreaId);
        }
    }
}