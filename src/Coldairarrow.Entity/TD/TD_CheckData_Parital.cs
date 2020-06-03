using Coldairarrow.Entity.PB;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coldairarrow.Entity.TD
{
    /// <summary>
    /// 盘差表
    /// </summary>
    public partial class TD_CheckData
    {
        [ForeignKey(nameof(StorId))]
        public PB_Storage Storage { set; get; }

        [ForeignKey(nameof(localId))]
        public PB_Location Location { set; get; }

        [ForeignKey(nameof(MaterialId))]
        public PB_Material Material { set; get; }

        [ForeignKey(nameof(ZoneId))]
        public PB_TrayZone Zone { set; get; }

        [ForeignKey(nameof(TrayId))]
        public PB_Tray Tray { set; get; }
    }
}