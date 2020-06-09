using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coldairarrow.Entity.PB
{
    /// <summary>
    /// 条码参数流水
    /// </summary>
    [Table("PB_BarCodeSerial")]
    public partial class PB_BarCodeSerial
    {

        /// <summary>
        /// 主键Id
        /// </summary>
        [Key, Column(Order = 1)]
        public String Id { get; set; }

        /// <summary>
        /// 条码类型
        /// </summary>
        public String TypeId { get; set; }

        /// <summary>
        /// 参数名称
        /// </summary>
        public String ParaName { get; set; }

        /// <summary>
        /// 参数值
        /// </summary>
        public String ParaValue { get; set; }

        /// <summary>
        /// 流水号
        /// </summary>
        public Int32 SerialNum { get; set; }

    }
}