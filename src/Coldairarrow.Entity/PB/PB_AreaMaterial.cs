using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coldairarrow.Entity.PB
{
    /// <summary>
    /// 货区物料关系表
    /// </summary>
    [Table("PB_AreaMaterial")]
    public partial class PB_AreaMaterial
    {

        /// <summary>
        /// 货区ID
        /// </summary>
        [Key, Column(Order = 1)]
        public String AreaId { get; set; }

        /// <summary>
        /// 物料ID
        /// </summary>
        [Key, Column(Order = 2)]
        public String MaterialId { get; set; }

    }
}