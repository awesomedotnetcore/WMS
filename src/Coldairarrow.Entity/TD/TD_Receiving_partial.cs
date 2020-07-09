using Coldairarrow.Entity.PB;
using Dynamitey.DynamicObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coldairarrow.Entity.TD
{
    /// <summary>
    /// 收货表
    /// </summary>
    public partial class TD_Receiving
    {
        [ForeignKey(nameof(SupId))]
        public PB_Supplier Supplier { get; set; }

        public List<TD_RecDetail> RecDetails { get; set; }
    }
}