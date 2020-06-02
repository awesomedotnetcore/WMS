using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coldairarrow.Entity.PB
{
    /// <summary>
    /// 物料目录
    /// </summary>
    public partial class PB_Material
    {
        /// <summary>
        /// 单位
        /// </summary>
        [ForeignKey(nameof(MeasureId))]
        public PB_Measure Measure { set; get; }

        public List<PB_TrayMaterial> PB_TrayMaterials { get; set; }

        public List<PB_AreaMaterial> PB_AreaMaterials { get; set; }

    }
}