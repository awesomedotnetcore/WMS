using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coldairarrow.Entity.PB
{
    /// <summary>
    /// 进料点/出料点 与物料对应关系
    /// </summary>
    [Table("PB_MaterialPoint")]
    public partial class PB_MaterialPoint
    {

        /// <summary>
        /// 料点Id
        /// </summary>
        [Key, Column(Order = 1)]
        public String PointId { get; set; }

        /// <summary>
        /// 物料Id
        /// </summary>
        [Key, Column(Order = 2)]
        public String MaterialId { get; set; }

    }
}