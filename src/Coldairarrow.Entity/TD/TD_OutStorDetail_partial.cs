using Coldairarrow.Entity.PB;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coldairarrow.Entity.TD
{
    /// <summary>
    /// 出库明细
    /// </summary>
    public partial class TD_OutStorDetail
    {
        [ForeignKey(nameof(OutStorId))]
        public TD_OutStorage OutStorage { get; set; }

        [ForeignKey(nameof(LocalId))]
        public PB_Location Location { get; set; }

        [ForeignKey(nameof(TrayId))]
        public PB_Tray Tray { get; set; }

        [ForeignKey(nameof(ZoneId))]
        public PB_TrayZone TrayZone { get; set; }

        [ForeignKey(nameof(MaterialId))]
        public PB_Material Material { get; set; }
    }

    public class TD_OutStorDetailEntityTypeConfig : IEntityTypeConfiguration<TD_OutStorDetail>
    {
        public void Configure(EntityTypeBuilder<TD_OutStorDetail> builder)
        {
            builder.HasQueryFilter(w => w.Deleted == false);
        }
    }
}