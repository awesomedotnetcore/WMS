using Coldairarrow.Entity.PB;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coldairarrow.Entity.IT
{
    /// <summary>
    /// 台账表
    /// </summary>
    public partial class IT_RecordBook
    {
        [ForeignKey(nameof(FromLocalId))]
        public PB_Location FromLocation { get; set; }

        [ForeignKey(nameof(ToLocalId))]
        public PB_Location ToLocation { get; set; }

        [ForeignKey(nameof(FromStorId))]
        public PB_Storage FromStorage { get; set; }

        [ForeignKey(nameof(ToStorId))]
        public PB_Storage ToStorage { get; set; }

        [ForeignKey(nameof(MaterialId))]
        public PB_Material Material { get; set; }

        [ForeignKey(nameof(MeasureId))]
        public PB_Measure Measure { get; set; }



    }
}