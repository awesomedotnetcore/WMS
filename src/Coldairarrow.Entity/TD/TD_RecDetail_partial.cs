using Coldairarrow.Entity.PB;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coldairarrow.Entity.TD
{
    /// <summary>
    /// 收货明细
    /// </summary>
    public partial class TD_RecDetail
    {
        [ForeignKey(nameof(RecId))]
        public TD_Receiving Receiving { get; set; }

        [ForeignKey(nameof(MaterialId))]
        public PB_Material Material { get; set; }

        [ForeignKey(nameof(MeasureId))]
        public PB_Measure Measure { get; set; }
    }
}