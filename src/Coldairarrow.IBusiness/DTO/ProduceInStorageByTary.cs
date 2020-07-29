using System;
using System.Collections.Generic;
using System.Text;

namespace Coldairarrow.IBusiness.DTO
{
    /// <summary>
    /// 自动入库Pad传入数据
    /// </summary>
    public class ProduceInStorageByTary
    {
        public string PlanCode { get; set; }
        public string TaryCode { get; set; }
        public double Num { get; set; }
    }
}
