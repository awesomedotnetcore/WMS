using Coldairarrow.Entity.PB;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coldairarrow.Entity.TD
{
    /// <summary>
    /// 移库明细
    /// </summary>
    public partial class TD_MoveDetail
    {
        [ForeignKey(nameof(MoveId))]
        public TD_Move Move { get; set; }

        [ForeignKey(nameof(FromLocalId))]
        public PB_Location FromLocal { get; set; }

        [ForeignKey(nameof(FromZoneId))]
        public PB_TrayZone FromZone { get; set; }

        [ForeignKey(nameof(FromTrayId))]
        public PB_Tray FromTray { get; set; }

        [ForeignKey(nameof(ToLocalId))]
        public PB_Location ToLocal { get; set; }

        [ForeignKey(nameof(ToTrayId))]
        public PB_Tray ToTray { get; set; }

        [ForeignKey(nameof(ToZoneId))]
        public PB_TrayZone ToZone { get; set; }

        [ForeignKey(nameof(MaterialId))]
        public PB_Material Material { get; set; }
    }
    public class TD_MoveDetailEntityTypeConfig : IEntityTypeConfiguration<TD_MoveDetail>
    {
        public void Configure(EntityTypeBuilder<TD_MoveDetail> builder)
        {
            builder.HasQueryFilter(w => w.Deleted == false);
        }
    }
}