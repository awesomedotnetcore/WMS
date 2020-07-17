using Coldairarrow.Entity.PB;
using Coldairarrow.Util;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Coldairarrow.IBusiness.Report
{
    /// <summary>
    /// 货品统计
    /// </summary>
    public interface IReport_MaterialSummaryBussiness
    {
        Task<PageResult<Report_MaterialSummaryVM>> GetDataListAsync(PageInput<Report_MaterialSummaryQM> input);
    }
    public class Report_MaterialSummaryQM
    {
        public string MaterialTypeId { get; set; }
        public string MaterialName { get; set; }
        public string BatchNo { get; set; }
        public bool MinAlert { get; set; }
        public bool MaxAlert { get; set; }
    }
    public class Report_MaterialSummaryVM
    {
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
        /// 批次号
        /// </summary>
        public String BatchNo { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public String BarCode { get; set; }

        /// <summary>
        /// 物料简称
        /// </summary>
        public String SimpleName { get; set; }

        public string MaterialTypeId { get; set; }
        /// <summary>
        /// 物料类型ID
        /// </summary>
        public String MaterialTypeName { get; set; }

        public string MaterialId { get; set; }
        /// <summary>
        /// 物料ID
        /// </summary>
        public String MaterialName { get; set; }

        public string MeasureId { get; set; }
        /// <summary>
        /// 单位ID
        /// </summary>
        public String MeasureName { get; set; }

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
        /// 单价
        /// </summary>
        public Double? Price { get; set; }

        public double SumCount { get; set; }
    }
}
