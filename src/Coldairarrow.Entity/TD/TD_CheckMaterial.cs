using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coldairarrow.Entity.TD
{
    /// <summary>
    /// 盘点信息表
    /// </summary>
    [Table("TD_CheckMaterial")]
    public partial class TD_CheckMaterial
    {

        /// <summary>
        /// 盘点ID
        /// </summary>
        [Key, Column(Order = 1)]
        public String CheckId { get; set; }

        /// <summary>
        /// 物料ID
        /// </summary>
        [Key, Column(Order = 2)]
        public String MaterialId { get; set; }

    }
}