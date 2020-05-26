using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coldairarrow.Entity.TD
{
    /// <summary>
    /// 入库明细
    /// </summary>
    public partial class TD_InStorDetail
    {
        [ForeignKey(nameof(InStorId))]
        public TD_InStorage InStorage { get; set; }
    }
}