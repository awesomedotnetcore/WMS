using Coldairarrow.Entity.PB;
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
}