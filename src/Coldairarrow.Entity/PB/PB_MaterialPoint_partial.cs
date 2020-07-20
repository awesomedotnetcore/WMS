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
    public partial class PB_MaterialPoint
    {

        [ForeignKey(nameof(PointId))]
        public PB_FeedPoint PB_FeedPoint { get; set; }

        [ForeignKey(nameof(MaterialId))]
        public PB_Material PB_Material { get; set; }

    }

    public class PB_MaterialPointEntityTypeConfig : IEntityTypeConfiguration<PB_MaterialPoint>
    {
        public void Configure(EntityTypeBuilder<PB_MaterialPoint> builder)
        {
            builder
                .HasKey(t => new { t.MaterialId, t.PointId });

            builder.HasOne(pt => pt.PB_Material)
                .WithMany(p => p.PB_MaterialPoints)
                .HasForeignKey(pt => pt.MaterialId);

        }
    }
}