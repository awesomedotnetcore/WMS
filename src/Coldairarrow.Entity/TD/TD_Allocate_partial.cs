using Coldairarrow.Entity.Base_Manage;
using Coldairarrow.Entity.PB;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coldairarrow.Entity.TD
{
    /// <summary>
    /// 调拨表
    /// </summary>
    public partial class TD_Allocate
    {
        [ForeignKey(nameof(ToStorId))]
        public PB_Storage ToStorage { get; set; }
        [ForeignKey(nameof(CreatorId))]
        public Base_User CreateUser { get; set; }

        [ForeignKey(nameof(AuditUserId))]
        public Base_User AuditUser { get; set; }

        public List<TD_AllocateDetail> AllocateDetails { get; set; }
    }
}