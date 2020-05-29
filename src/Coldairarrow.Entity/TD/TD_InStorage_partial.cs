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
    /// 入库表
    /// </summary>
    public partial class TD_InStorage
    {
        public List<TD_InStorDetail> InStorDetails { get; set; }

        [ForeignKey(nameof(SupId))]
        public PB_Supplier Supplier { get; set; }

        [ForeignKey(nameof(CreatorId))]
        public Base_User CreateUser { get; set; }

        [ForeignKey(nameof(AuditUserId))]
        public Base_User AuditUser { get; set; }
    }
}