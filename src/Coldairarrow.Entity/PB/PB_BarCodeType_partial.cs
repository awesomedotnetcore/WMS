using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coldairarrow.Entity.PB
{
    /// <summary>
    /// 条码类型管理
    /// </summary>
    public partial class PB_BarCodeType
    {
        public List<PB_BarCodeRule> BarCodeRules { get; set; }
        public List<PB_BarCodeSerial> BarCodeSerials { get; set; }
    }
}