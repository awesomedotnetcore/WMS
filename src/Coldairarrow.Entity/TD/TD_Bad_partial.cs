using Coldairarrow.Entity.Base_Manage;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coldairarrow.Entity.TD
{
    /// <summary>
    /// 报损表
    /// </summary>
    public partial class TD_Bad
    {
        [ForeignKey(nameof(CreatorId))]
        public Base_User CreateUser { get; set; }

        [ForeignKey(nameof(AuditUserId))]
        public Base_User AuditUser { get; set; }

        public List<TD_BadDetail> BadDetails { get; set; }
    }
}