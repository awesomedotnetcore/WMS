using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coldairarrow.Entity.PB
{
    /// <summary>
    /// 进料点/出料点 与物料对应关系
    /// </summary>
    public partial class PB_MaterialPoint
    {
        [ForeignKey(nameof(PointId))]
        public PB_FeedPoint Point { get; set; }

        [ForeignKey(nameof(MaterialId))]
        public PB_Material Material { get; set; }
    }

    public class PB_MaterialPointEntityTypeConfig : IEntityTypeConfiguration<PB_MaterialPoint>
    {
        public void Configure(EntityTypeBuilder<PB_MaterialPoint> builder)
        {
            builder
                .HasKey(t => new { t.MaterialId, t.PointId });

            builder.HasOne(pt => pt.Material)
                .WithMany(p => p.MaterialPoints)
                .HasForeignKey(pt => pt.MaterialId);

            builder.HasOne(pt => pt.Point)
                .WithMany(p => p.MaterialPoints)
                .HasForeignKey(pt => pt.PointId);
        }
    }
}