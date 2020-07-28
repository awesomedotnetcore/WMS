using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coldairarrow.Entity.Base
{
    /// <summary>
    /// 数据字典
    /// </summary>
    public partial class Base_Enum
    {
        public List<Base_EnumItem> EnumItems { get; set; }

    }
}