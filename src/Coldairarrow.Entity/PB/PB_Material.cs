using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coldairarrow.Entity.PB
{
    /// <summary>
    /// 物料目录
    /// </summary>
    [Table("PB_Material")]
    public partial class PB_Material
    {

        /// <summary>
        /// 主键ID
        /// </summary>
        [Key, Column(Order = 1)]
        public String Id { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public String Code { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public String BarCode { get; set; }

        /// <summary>
        /// 物料简称
        /// </summary>
        public String SimpleName { get; set; }

        /// <summary>
        /// 物料类型ID
        /// </summary>
        public String MaterialTypeId { get; set; }

        /// <summary>
        /// 单位ID
        /// </summary>
        public String MeasureId { get; set; }

        /// <summary>
        /// 物料规格
        /// </summary>
        public String Spec { get; set; }

        /// <summary>
        /// 上限数量
        /// </summary>
        public Double? Max { get; set; }

        /// <summary>
        /// 下限数量
        /// </summary>
        public Double? Min { get; set; }

        /// <summary>
        /// 客户ID
        /// </summary>
        public String CusId { get; set; }

        /// <summary>
        /// 供应商ID
        /// </summary>
        public String SupId { get; set; }

        /// <summary>
        /// 默认存储仓库
        /// </summary>
        public String StorId { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        public Double? Price { get; set; }

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