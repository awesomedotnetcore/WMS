using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coldairarrow.Entity.Base
{
    /// <summary>
    /// 字典值
    /// </summary>
    [Table("Base_EnumItem")]
    public partial class Base_EnumItem
    {

        /// <summary>
        /// Id
        /// </summary>
        [Key, Column(Order = 1)]
        public String Id { get; set; }

        /// <summary>
        /// EnumId
        /// </summary>
        public String EnumId { get; set; }

        /// <summary>
        /// EnumCode
        /// </summary>
        public String EnumCode { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public String Name { get; set; }
        /// <summary>
        /// Code
        /// </summary>
        public String Code { get; set; }

        /// <summary>
        /// Value
        /// </summary>
        public String Value { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public String Remarks { get; set; }

        /// <summary>
        /// 系统必须
        /// </summary>
        public Boolean IsSystem { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 创建人ID
        /// </summary>
        public String CreatorId { get; set; }

        /// <summary>
        /// 删除状态：0=未删除；1＝已删除；
        /// </summary>
        public Boolean Deleted { get; set; }

    }
}