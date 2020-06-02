using Coldairarrow.Entity.PB;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coldairarrow.Entity.IT
{
    /// <summary>
    /// 库存明细表
    /// </summary>
    public partial class IT_LocalDetail
    {
        [ForeignKey(nameof(LocalId))]
        public PB_Location Location { get; set; }

        [ForeignKey(nameof(TrayId))]
        public PB_Tray Tray { get; set; }

        [ForeignKey(nameof(ZoneId))]
        public PB_TrayZone TrayZone { get; set; }

        [ForeignKey(nameof(MaterialId))]
        public PB_Material Material { get; set; }

        [ForeignKey(nameof(MeasureId))]
        public PB_Measure Measure { get; set; }
    }
}