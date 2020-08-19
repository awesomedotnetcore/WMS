using Coldairarrow.Entity.IT;
using Coldairarrow.Entity.PB;
using Coldairarrow.Entity.TD;
using Coldairarrow.IBusiness.DTO;
using Coldairarrow.Util;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.IT
{
    public partial interface IIT_LocalMaterialBusiness
    {
        Task<List<IT_LocalMaterial>> LoadCheckDataByAreaIdAsync(string id);

        Task<List<IT_LocalMaterial>> LoadCheckDataByMaterialAsync(string storId, List<string> ids);

        Task<List<PB_Material>> LoadMaterialByRandomAsync(string storId, int per);

        Task AddDataAsync(List<IT_LocalMaterial> list);
        Task UpdateDataAsync(List<IT_LocalMaterial> list);

        Task UpdataDatasByBussiness(List<BusinessInfo> list);

        Task<IT_LocalMaterial> GetDataByBussiness(BusinessInfo businessInfo);

        Task<PageResult<IT_LocalMaterial>> GetDataListAsync(IT_LocalMaterialPageInput input);

        Task<List<IT_LocalMaterial>> GetQueryData(SelectQueryDTO search, string storId);

        Task DeleteDataAsync(List<IT_LocalMaterial> list);
    }

    public class BusinessInfo
    {
        /// <summary>
        /// 仓库ID
        /// </summary>
        public String StorId { get; set; }

        /// <summary>
        /// 货位ID
        /// </summary>
        public String LocalId { get; set; }

        /// <summary>
        /// 托盘号ID
        /// </summary>
        public String TrayId { get; set; }

        /// <summary>
        /// 托盘分区ID
        /// </summary>
        public String ZoneId { get; set; }

        /// <summary>
        /// 物料ID
        /// </summary>
        public String MaterialId { get; set; }

        /// <summary>
        /// 批次号
        /// </summary>
        public String BatchNo { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public String BarCode { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public Double Num { get; set; }

        /// <summary>
        /// 操作（1 出库； 2 入库）
        /// </summary>
        public int ActionType { get; set; }

        /// <summary>
        /// 单位ID
        /// </summary>
        public String MeasureId { get; set; }
    }

    public enum ActionTypeEnum
    {
        出库 = 1,
        入库
    }
    public class IT_LocalMaterialQM
    {
        #region 特定条件查询
        public string LocalId { get; set; }
        public string TrayId { get; set; }
        public string MaterialId { get; set; }
        #endregion

        public string LocalName { get; set; }
        public string TrayName { get; set; }
        public string MaterialName { get; set; }
        public string Code { get; set; }
    }
    public class IT_LocalMaterialPageInput:PageInput<IT_LocalMaterialQM>
    {
        public string StorId { get; set; }
    }
}