using Coldairarrow.Entity.PB;
using Coldairarrow.Util;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace Coldairarrow.Business.PB
{
    public partial interface IPB_BarCodeTypeBusiness
    {
        /// <summary>
        /// 生成条码
        /// </summary>
        /// <param name="typeCode">条码类型</param>
        /// <param name="para">参数</param>
        /// <returns></returns>
        Task<string> Generate(string typeCode, Dictionary<string, string> para = null);
        Task<List<PB_BarCodeType>> GetAllData();
    }
}