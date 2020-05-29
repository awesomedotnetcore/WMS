using Coldairarrow.Entity.PB;
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
}