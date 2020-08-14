using System;
using System.Collections.Generic;
using System.Text;

namespace Coldairarrow.IBusiness.DTO
{
    /// <summary>
    /// 手动入库Pad传入数据
    /// </summary>
    public class ManualInStorage
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

        public string RecId { get; set; }

        /// <summary>
        /// 物料条码
        /// </summary>
        public string MaterialCode { get; set; }
        /// <summary>
        /// 货位编号
        /// </summary>
        public string LocalCode { get; set; }
        /// <summary>
        /// 托盘编号
        /// </summary>
        public string TrayCode { get; set; }

        /// <summary>
        /// 批次
        /// </summary>
        public string BatchNo { get; set; }
        
        /// <summary>
        /// 数量
        /// </summary>
        public double Num { get; set; }
    }
}
