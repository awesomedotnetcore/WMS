using System;
using System.Collections.Generic;
using System.Text;

namespace Coldairarrow.IBusiness.DTO
{
    public class TDCheckNumModifyDTO
    {
        /// <summary>
        /// 盘点明细编号
        /// </summary>
        public string Id { set; get; }

        /// <summary>
        /// 盘点数量
        /// </summary>
        public Double? CheckNum { get; set; }
    }
}
