using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coldairarrow.Entity.TD
{
    /// <summary>
    /// 盘差表
    /// </summary>
    [Table("TD_CheckData")]
    public partial class TD_CheckData
    {

        /// <summary>
        /// ID
        /// </summary>
        [Key, Column(Order = 1)]
        public String Id { get; set; }

        /// <summary>
        /// 盘点ID
        /// </summary>
        public String CheckId { get; set; }

        /// <summary>
        /// 仓库ID
        /// </summary>
        public String StorId { get; set; }

        /// <summary>
        /// 货位ID
        /// </summary>
        public String localId { get; set; }

        /// <summary>
        /// 托盘ID
        /// </summary>
        public string TrayId { get; set; }

        /// <summary>
        /// 托盘分区ID
        /// </summary>
        public string ZoneId { set; get; }

        /// <summary>
        /// 物料ID
        /// </summary>
        public String MaterialId { get; set; }

        /// <summary>
        /// 批次号
        /// </summary>
        public String BatchNo { get; set; }

        /// <summary>
        /// 库存数量
        /// </summary>
        public Double? LocalNum { get; set; }

        /// <summary>
        /// 盘点数量
        /// </summary>
        public Double? CheckNum { get; set; }

        /// <summary>
        /// 盘差数量
        /// </summary>
        public Double? DisNum { get; set; }

        /// <summary>
        /// 盘点人ID
        /// </summary>
        public String CheckUserId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 创建人ID
        /// </summary>
        public String CreatorId { get; set; }

    }
}