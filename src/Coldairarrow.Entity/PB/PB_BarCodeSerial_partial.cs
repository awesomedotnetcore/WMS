using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coldairarrow.Entity.PB
{
    /// <summary>
    /// 条码参数流水
    /// </summary>
    public partial class PB_BarCodeSerial
    {
        [ForeignKey(nameof(TypeId))]
        public PB_BarCodeType BarCodeType { get; set; }
    }
}