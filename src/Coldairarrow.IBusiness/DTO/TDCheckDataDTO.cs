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
        /// 仓库ID
        /// </summary>
        public String StorId { get; set; }

        /// <summary>
        /// 货位ID
        /// </summary>
        public String localId { get; set; }

        /// <summary>
        /// 物料ID
        /// </summary>
        public String MaterialId { get; set; }

        /// <summary>
        /// 单位ID
        /// </summary>
        public string MeasureId { get; set; }

        /// <summary>
        /// 托盘号ID
        /// </summary>
        public string TrayId { get; set; }

        /// <summary>
        /// 托盘分区ID
        /// </summary>
        public string ZoneId { get; set; }

        /// <summary>
        /// 物料条码
        /// </summary>
        public string BarCode { get; set; }


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
        /// 托盘名称
        /// </summary>
        public string TrayName { set; get; }

        /// <summary>
        /// 托盘分区名称
        /// </summary>
        public string ZoneName { set; get; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string MaterialName { set; get; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialCode { set; get; }

        /// <summary>
        /// 单位
        /// </summary>
        public string MeasureName { set; get; }

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
