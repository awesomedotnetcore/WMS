using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coldairarrow.Entity.PB
{
    /// <summary>
    /// 客户/投料点表
    /// </summary>
    [Table("PB_Customer")]
    public class PB_Customer
    {

        /// <summary>
        /// 客户ID
        /// </summary>
        [Key, Column(Order = 1)]
        public String Id { get; set; }

        /// <summary>
        /// 客户编号
        /// </summary>
        public String Code { get; set; }

        /// <summary>
        /// 客户名称
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// 客户类型
        /// </summary>
        public String Type { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public String Phone { get; set; }

        /// <summary>
        /// 传真
        /// </summary>
        public String Fax { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public String Email { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public String Remarks { get; set; }

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