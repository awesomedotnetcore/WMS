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
    }
}