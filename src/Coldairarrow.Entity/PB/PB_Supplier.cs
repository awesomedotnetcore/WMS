using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coldairarrow.Entity.PB
{
    /// <summary>
    /// 供应商/下料点表
    /// </summary>
    [Table("PB_Supplier")]
    public class PB_Supplier
    {

        /// <summary>
        /// 供应商ID
        /// </summary>
        [Key, Column(Order = 1)]
        public String Id { get; set; }

        /// <summary>
        /// 供应商编号
        /// </summary>
        public String Code { get; set; }

        /// <summary>
        /// 供应商名称
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// 供应商类型
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
        /// 联系人
        /// </summary>
        public String ContactName { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public String Address { get; set; }

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