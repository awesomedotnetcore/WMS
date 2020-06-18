using Coldairarrow.Entity.Base_Manage;
using Coldairarrow.Entity.PB;
using Dynamitey.DynamicObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coldairarrow.Entity.TD
{
    /// <summary>
    /// 移库表
    /// </summary>
    public partial class TD_Move
    {
        [ForeignKey(nameof(CreatorId))]
        public Base_User CreateUser { get; set; }

        [ForeignKey(nameof(AuditUserId))]
        public Base_User AuditUser { get; set; }

        public List<TD_MoveDetail> MoveDetails { get; set; }
    }
}