using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coldairarrow.Entity.Base
{
    /// <summary>
    /// 系统参数表
    /// </summary>
    [Table("Base_Parameter")]
    public class Base_Parameter
    {

        /// <summary>
        /// ID主键
        /// </summary>
        [Key, Column(Order = 1)]
        public String Id { get; set; }

        /// <summary>
        /// 参数类型
        /// </summary>
        public String Type { get; set; }

        /// <summary>
        /// 参数编号
        /// </summary>
        public String Code { get; set; }

        /// <summary>
        /// 参数名称
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// 参数值配置
        /// </summary>
        public String ValConfig { get; set; }

        /// <summary>
        /// 参数值
        /// </summary>
        public string Val { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public String Remarks { get; set; }

        /// <summary>
        /// 是否系统必须
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