using Dynamitey.DynamicObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coldairarrow.Entity.PB
{
    /// <summary>
    /// 进料点/出料点
    /// </summary>
    public partial class PB_FeedPoint
    {
        [ForeignKey(nameof(StorId))]
        public PB_Storage Storage { get; set; }

        [ForeignKey(nameof(LaneId))]
        public PB_Laneway Laneway { get; set; }
        public List<PB_MaterialPoint> MaterialPoints { get; set; }
    }
}