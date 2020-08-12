using System;
using System.Collections.Generic;
using System.Text;

namespace Coldairarrow.IBusiness.DTO
{
    /// <summary>
    /// 自动入库Pad传入数据
    /// </summary>
    public class AutoInStorageByTary
    {
        ///// <summary>
        ///// 生产线Id
        ///// 在控制台写固定值
        ///// </summary>
        //public string SupId { get; set; }
        ///// <summary>
        ///// 下料点ID
        ///// 在控制台写固定值
        ///// </summary>
        //public string AddrId { get; set; }


        //public string PlanCode { get; set; }
        /// <summary>
        /// 收货ID
        /// </summary>
        public string RecId { get; set; }
        public string MaterialCode { get; set; }
        public string BatchNo { get; set; }
        public string TrayCode { get; set; }
        public double Num { get; set; }
    }
}
