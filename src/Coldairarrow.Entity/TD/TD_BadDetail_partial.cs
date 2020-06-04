using Coldairarrow.Entity.PB;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coldairarrow.Entity.TD
{
    /// <summary>
    /// 报损明细
    /// </summary>
    public partial class TD_BadDetail
    {
        [ForeignKey(nameof(BadId))]
        public TD_Bad Bad { get; set; }

        [ForeignKey(nameof(FromLocalId))]
        public PB_Location FromLocal { get; set; }

        [ForeignKey(nameof(TrayId))]
        public PB_Tray Tray { get; set; }

        [ForeignKey(nameof(ZoneId))]
        public PB_TrayZone TrayZone { get; set; }

        [ForeignKey(nameof(MaterialId))]
        public PB_Material Material { get; set; }
    }
    public class TD_BadDetailEntityTypeConfig : IEntityTypeConfiguration<TD_BadDetail>
    {
        public void Configure(EntityTypeBuilder<TD_BadDetail> builder)
        {
            builder.HasQueryFilter(w => w.Deleted == false);
        }
    }
}