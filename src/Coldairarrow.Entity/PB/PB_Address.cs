using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coldairarrow.Entity.PB
{
    /// <summary>
    /// 客户/投料点/供应商地址表
    /// </summary>
    [Table("PB_Address")]
    public class PB_Address
    {

        /// <summary>
        /// 主键ID
        /// </summary>
        [Key, Column(Order = 1)]
        public String Id { get; set; }

        /// <summary>
        /// 客户ID
        /// </summary>
        public string CusId { get; set; }

        /// <summary>
        /// 供应商ID
        /// </summary>
        public string SupId { get; set; }

        /// <summary>
        /// 电话/投料点编号
        /// </summary>
        public String Code { get; set; }

        /// <summary>
        /// 联系人/投料点名称
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public String Address { get; set; }

        /// <summary>
        /// 是否启用 
        /// </summary>
        public Boolean IsEnable { get; set; }

        /// <summary>
        /// 是否默认
        /// </summary>
        public Boolean IsDefault { get; set; }

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