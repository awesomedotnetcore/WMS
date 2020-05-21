using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coldairarrow.Entity.PB
{
    /// <summary>
    /// 物料类型
    /// </summary>
    [Table("PB_MaterialType")]
    public class PB_MaterialType
    {

        /// <summary>
        /// 物料分类表ID
        /// </summary>
        [Key, Column(Order = 1)]
        public String Id { get; set; }

        /// <summary>
        /// 物料分类名称
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// 物料分类编码
        /// </summary>
        public String Code { get; set; }

        /// <summary>
        /// 父节点物料分类ID
        /// </summary>
        public String ParentId { get; set; }

        /// <summary>
        /// 是否是叶节点
        /// </summary>
        public bool IsLeaf { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public String Remarks { get; set; }

        /// <summary>
        /// Path
        /// </summary>
        public String Path { get; set; }

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