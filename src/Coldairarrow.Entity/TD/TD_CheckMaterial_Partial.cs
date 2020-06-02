using Coldairarrow.Entity.PB;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coldairarrow.Entity.TD
{
    /// <summary>
    /// 盘点信息表
    /// </summary>
    public partial class TD_CheckMaterial
    {
        [ForeignKey(nameof(MaterialId))]
        public PB_Material Material { set; get; }
    }

    public class TD_CheckMaterialEntityTypeConfig : IEntityTypeConfiguration<TD_CheckMaterial>
    {
        public void Configure(EntityTypeBuilder<TD_CheckMaterial> builder)
        {
            builder
                .HasKey(t => new { t.MaterialId, t.CheckId });
        }
    }
}