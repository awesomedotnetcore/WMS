using Coldairarrow.Entity.PB;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coldairarrow.Entity.TD
{
    /// <summary>
    /// 发货明细
    /// </summary>
    public partial class TD_SendDetail
    {

        [ForeignKey(nameof(SendId))]
        public TD_Send Send { get; set; }

        [ForeignKey(nameof(LocalId))]
        public PB_Location Location { get; set; }

        [ForeignKey(nameof(MaterialId))]
        public PB_Material Material { get; set; }

        [ForeignKey(nameof(MeasureId))]
        public PB_Measure Measure { get; set; }


    }

    public class TD_SendDetailEntityTypeConfig : IEntityTypeConfiguration<TD_SendDetail>
    {
        public void Configure(EntityTypeBuilder<TD_SendDetail> builder)
        {
            builder.HasQueryFilter(w => w.Deleted == false);
        }
    }
}