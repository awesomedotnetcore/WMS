using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coldairarrow.Entity.PB
{
    /// <summary>
    /// 托盘类型与货位对应关系
    /// </summary>
    public partial class PB_LocalTray
    {
        //[ForeignKey(nameof(AreaId))]
        //public PB_StorArea PB_StorArea { get; set; }

    }
}