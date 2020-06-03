using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coldairarrow.Entity.IT
{
    /// <summary>
    /// 台账表
    /// </summary>
    [Table("IT_RecordBook")]
    public partial class IT_RecordBook
    {

        /// <summary>
        /// Id
        /// </summary>
        [Key, Column(Order = 1)]
        public String Id { get; set; }

        /// <summary>
        /// 相关单号
        /// </summary>
        public String RefCode { get; set; }

        /// <summary>
        /// 台账类型
        /// 入库 出库 移库 调拨出 调拨入 报损 盘亏 盘盈
        /// </summary>
        public String Type { get; set; }

        /// <summary>
        /// 原仓库ID
        /// </summary>
        public String FromStorId { get; set; }

        /// <summary>
        /// 原货位ID
        /// </summary>
        public String FromLocalId { get; set; }

        /// <summary>
        /// 目标仓库
        /// </summary>
        public String ToStorId { get; set; }

        /// <summary>
        /// 目标货位ID
        /// </summary>
        public String ToLocalId { get; set; }

        /// <summary>
        /// 物料ID
        /// </summary>
        public String MaterialId { get; set; }

        /// <summary>
        /// 单位ID
        /// </summary>
        public String MeasureId { get; set; }

        /// <summary>
        /// 物料条码
        /// </summary>
        public String BarCode { get; set; }

        /// <summary>
        /// 批次号
        /// </summary>
        public String BatchNo { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public Double Num { get; set; }

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