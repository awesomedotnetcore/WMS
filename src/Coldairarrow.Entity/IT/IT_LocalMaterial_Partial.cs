using Coldairarrow.Entity.PB;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coldairarrow.Entity.IT
{
    /// <summary>
    /// 库存表
    /// </summary>
    public partial class IT_LocalMaterial
    {
        [ForeignKey(nameof(LocalId))]
        public PB_Location Location { set; get; }

        [ForeignKey(nameof(MaterialId))]
        public PB_Material Material { set; get; }

        [ForeignKey(nameof(StorId))]
        public PB_Storage Storage { set; get; }

        [ForeignKey(nameof(ZoneId))]
        public PB_TrayZone TrayZone { set; get; }

        [ForeignKey(nameof(TrayId))]
        public PB_Tray Tray { set; get; }

        [ForeignKey(nameof(MeasureId))]
        public PB_Measure Measure { set; get; }
    }
}