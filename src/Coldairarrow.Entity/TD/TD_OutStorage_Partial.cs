using Coldairarrow.Entity.Base_Manage;
using Coldairarrow.Entity.IT;
using Coldairarrow.Entity.PB;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coldairarrow.Entity.TD
{
    /// <summary>
    /// 出库表
    /// </summary>
    public partial class TD_OutStorage
    {
        public List<TD_OutStorDetail> OutStorDetails { get; set; }

        //public List<IT_LocalMaterial> LocalMaterias { get; set; }

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