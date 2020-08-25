using Coldairarrow.Entity.PB;
using Coldairarrow.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.PB
{
    public partial interface IPB_TrayTypeBusiness
    {
        /// <summary>
        /// 根据物料，找到对应的托盘类型
        /// </summary>
        /// <param name="materialId">物料ID</param>
        /// <returns></returns>
        Task<List<string>> GetByMaterial(string materialId);

        /// <summary>
        /// 根据货位，找到对应的托盘类型
        /// </summary>
        /// <param name="locationId">货位ID</param>
        /// <returns></returns>
        Task<List<string>> GetByLocation(string locationId);

        Task<PB_TrayType> GetByTypeCode(string typecode);
    }
}