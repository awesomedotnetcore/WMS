using Coldairarrow.Entity.TD;
using Coldairarrow.Util;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.TD
{
    public partial interface ITD_AllocateDetailBusiness
    {
        Task<PageResult<TD_AllocateDetail>> GetDataListAsync(PageInput<ConditionDTO> input);
        Task<TD_AllocateDetail> GetTheDataAsync(string id);
        Task AddDataAsync(TD_AllocateDetail data);
        Task AddDatasAsync(List<TD_AllocateDetail> datas);
        Task UpdateDataAsync(TD_AllocateDetail data);
        Task UpdateDatasAsync(List<TD_AllocateDetail> datas);
        Task DeleteDataAsync(List<string> ids);
        Task<List<TD_AllocateDetail>> GetDataListByAllocateIdsAsync(List<string> allocateIds);
    }

    public class TD_AllocateDetailDTO
    {

        /// <summary>
        /// MoveDetailId
        /// </summary>=
        public String AllocateDetailId { get; set; }

        /// <summary>
        /// 移库ID
        /// </summary>
        public String AllocateId { get; set; }

        /// <summary>
        /// 仓库ID
        /// </summary>
        public String StorId { get; set; }

        /// <summary>
        /// 原货位ID
        /// </summary>
        public String FromLocalId { get; set; }

        /// <summary>
        /// 原托盘分区
        /// </summary>
        public String FromZoneId { get; set; }

        /// <summary>
        /// 原托盘ID
        /// </summary>
        public String FromTrayId { get; set; }

        /// <summary>
        /// 目标仓库ID
        /// </summary>
        public String ToStorId { get; set; }

        /// <summary>
        /// 目标货位ID
        /// </summary>
        public String ToLocalId { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public String BarCode { get; set; }

        /// <summary>
        /// 物料ID
        /// </summary>
        public String MaterialId { get; set; }

        /// <summary>
        /// 批次号
        /// </summary>
        public String BatchNo { get; set; }

        /// <summary>
        /// 移库数量
        /// </summary>
        public Double? LocalNum { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 创建人ID
        /// </summary>
        public String CreatorId { get; set; }

        /// <summary>
        /// 删除状态：0=未删除；1＝已删除；
        /// </summary>
        public Boolean Deleted { get; set; }
    }
}