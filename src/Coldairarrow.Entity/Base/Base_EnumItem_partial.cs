using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coldairarrow.Entity.Base
{
    /// <summary>
    /// 字典值
    /// </summary>
    public partial class Base_EnumItem
    {
        [ForeignKey(nameof(EnumId))]
        public Base_Enum Enum { get; set; }
    }

    public class Base_EnumItemEntityTypeConfig : IEntityTypeConfiguration<Base_EnumItem>
    {
        public void Configure(EntityTypeBuilder<Base_EnumItem> builder)
        {
            builder.HasQueryFilter(w => w.Deleted == false);
        }
    }
}