using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coldairarrow.Entity.TD
{
    /// <summary>
    /// 盘点货区关系表
    /// </summary>
    [Table("TD_CheckArea")]
    public class TD_CheckArea
    {

        /// <summary>
        /// CherkId
        /// </summary>
        [Key, Column(Order = 1)]
        public String CherkId { get; set; }

        /// <summary>
        /// StoarAreaId
        /// </summary>
        [Key, Column(Order = 2)]
        public String StoarAreaId { get; set; }

    }

    public class TD_CheckAreaEntityTypeConfig : IEntityTypeConfiguration<TD_CheckArea>
    {
        public void Configure(EntityTypeBuilder<TD_CheckArea> builder)
        {
            builder
                .HasKey(t => new { t.CherkId, t.StoarAreaId });
        }
    }
}