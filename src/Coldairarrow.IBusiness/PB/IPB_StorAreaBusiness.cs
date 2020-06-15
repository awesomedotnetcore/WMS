using Coldairarrow.Entity.PB;
using Coldairarrow.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;

namespace Coldairarrow.Business.PB
{
    public partial interface IPB_StorAreaBusiness
    {
        Task<PageResult<PB_StorArea>> GetDataListAsync(PB_StorAreaPageInput input);
        Task<List<PB_StorArea>> GetDataListAsync(string storId);
        Task<PB_StorArea> GetTheDataAsync(string id);
        Task<List<PB_StorArea>> QueryStorAreaDataAsync();
        Task AddDataAsync(PB_StorArea data);
        Task UpdateDataAsync(PB_StorArea data);
        Task DeleteDataAsync(List<string> ids);
    }
    public class PB_StorAreaQM
    {
        public string Keyword { get; set; }

        public string StorageId { set; get; }

        public string AreaType { set; get; }
    }
    public class PB_StorAreaPageInput : PageInput<PB_StorAreaQM>
    {
        public string Id { get; set; }
    }

    [Map(typeof(PB_StorArea))]
    public class PB_StorAreaDTO
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public String Id { get; set; }

        /// <summary>
        /// 仓库ID
        /// </summary>
        public String StorId { get; set; }
        [ForeignKey(nameof(StorId))]
        public PB_Storage PB_Storage { get; set; }

        /// <summary>
        /// 货区编号
        /// </summary>
        public String Code { get; set; }

        /// <summary>
        /// 货区名称
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// 货区类型
        /// </summary>
        public String Type { get; set; }

        ///// <summary>
        ///// 是否缓存区
        ///// </summary>
        //public Boolean IsCache { get; set; }

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

        public List<PB_AreaMaterial> PB_AreaMaterials { get; set; }
    }
}