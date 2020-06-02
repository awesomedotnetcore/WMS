using System;
using System.Collections.Generic;
using System.Text;

namespace Coldairarrow.IBusiness.DTO
{
    public class TDCheckDataDTO
    {
        /// <summary>
        /// ID
        /// </summary>
        public String Id { get; set; }

        /// <summary>
        /// 盘点ID
        /// </summary>
        public String CheckId { get; set; }

        /// <summary>
        /// 货位编码
        /// </summary>
        public string LocationCode { set; get; }

        /// <summary>
        /// 货位名称
        /// </summary>
        public string LocationName { set; get; }

        /// <summary>
        /// 库区
        /// </summary>
        public string AreaName { set; get; }

        /// <summary>
        /// 巷道
        /// </summary>
        public string LanewayName { set; get; }

        /// <summary>
        /// 货架
        /// </summary>
        public string RackName { set; get; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string MaterialName { set; get; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialCode { set; get; }

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
        /// 备注
        /// </summary>
        public string Remarks { get; set; }
    }
}
