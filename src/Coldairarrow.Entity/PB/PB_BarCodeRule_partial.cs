using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coldairarrow.Entity.PB
{
    /// <summary>
    /// 条码规则
    /// </summary>
    public partial class PB_BarCodeRule
    {
        [ForeignKey(nameof(TypeId))]
        public PB_BarCodeType BarCodeType { get; set; }
    }

    public class PB_BarCodeRuleEntityTypeConfig : IEntityTypeConfiguration<PB_BarCodeRule>
    {
        public void Configure(EntityTypeBuilder<PB_BarCodeRule> builder)
        {
            builder.HasQueryFilter(w => w.Deleted == false);
        }
    }
}