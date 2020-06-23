using Coldairarrow.Entity.PB;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coldairarrow.Entity.TD
{
    /// <summary>
    /// 调拨明细
    /// </summary>
    public partial class TD_AllocateDetail
    {
        [ForeignKey(nameof(AllocateId))]
        public TD_Allocate Allocate { get; set; }

        [ForeignKey(nameof(FromLocalId))]
        public PB_Location FromLocal { get; set; }

        [ForeignKey(nameof(FromTrayId))]
        public PB_Tray FromTray { get; set; }

        [ForeignKey(nameof(FromZoneId))]
        public PB_TrayZone FromZone { get; set; }

        [ForeignKey(nameof(MaterialId))]
        public PB_Material Material { get; set; }
    }

    public class TD_AllocateDetailEntityTypeConfig : IEntityTypeConfiguration<TD_AllocateDetail>
    {
        public void Configure(EntityTypeBuilder<TD_AllocateDetail> builder)
        {
            builder.HasQueryFilter(w => w.Deleted == false);
        }
    }
}