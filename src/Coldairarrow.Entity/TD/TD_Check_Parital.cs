using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Coldairarrow.Entity.Base_Manage;

namespace Coldairarrow.Entity.TD
{
    public partial class TD_Check
    {
        [ForeignKey(nameof(AuditUserId))]
        public Base_User AuditUser { get; set; }
    }
}