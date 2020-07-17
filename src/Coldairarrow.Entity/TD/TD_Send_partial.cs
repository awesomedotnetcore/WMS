using Coldairarrow.Entity.Base_Manage;
using Coldairarrow.Entity.PB;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coldairarrow.Entity.TD
{
    /// <summary>
    /// 发货表
    /// </summary>
    public partial class TD_Send
    {
        public List<TD_SendDetail> SendDetails { get; set; }

        [ForeignKey(nameof(CusId))]
        public PB_Customer Customer { get; set; }

        [ForeignKey(nameof(AddrId))]
        public PB_Address Address { get; set; }

        [ForeignKey(nameof(CreatorId))]
        public Base_User CreateUser { get; set; }

        [ForeignKey(nameof(AuditUserId))]
        public Base_User AuditUser { get; set; }
}
}