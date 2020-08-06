using Coldairarrow.Entity.TD;
using Coldairarrow.IBusiness.DTO;
using Coldairarrow.Util;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.TD
{
    public partial interface ITD_OutStorageBusiness
    {
        Task<PageResult<TD_OutStorage>> GetDataListAsync(TD_OutStoragePageInput input);

        Task Approve(AuditDTO audit);
        Task Reject(AuditDTO audit);

        Task<AjaxResult> OutBlankTray(List<KeyValuePair<string, string>> listTray, string storid);
        /// <summary>
        /// 申请物料出库，自动分拣
        /// </summary>
        /// <param name="data"></param>
        /// <returns>返回物料对应的货位与数量</returns>
        Task<AjaxResult<List<ReqMaterialResultDTO>>> ReqMaterial(ReqMaterialQM data);
    }
    /// <summary>
    /// 请求物料参数
    /// </summary>
    public class ReqMaterialQM
    {
        public string StorId { get; set; }
        public string MaterialId { get; set; }
        public string BatchNo { get; set; }
        public double Num { get; set; }
    }
    /// <summary>
    /// 返回请求物料结果
    /// </summary>
    public class ReqMaterialResultDTO
    {
        public string LocalId { get; set; }
        public string TrayId { get; set; }
        public string MaterialId { get; set; }
        public string BatchNo { get; set; }
        /// <summary>
        /// 库存数量
        /// </summary>
        public double LocalNum { get; set; }
        /// <summary>
        /// 实际出库数量
        /// </summary>
        public double OutNum { get; set; }
    }
    /// <summary>
    /// 生产物料出库
    /// </summary>
    public class ProduceOutStorageQM
    {
        public string MaterialCode { get; set; }
        public string BatchNo { get; set; }
        public double Num { get; set; }
    }
}