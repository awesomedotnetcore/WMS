using Coldairarrow.Entity.IT;
using Coldairarrow.Util;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.IT
{
    public partial interface IIT_LocalDetailBusiness
    {
        Task AddDataAsync(List<IT_LocalDetail> list);
        Task UpdateDataAsync(List<IT_LocalDetail> list);
        Task DeleteDataAsync(List<IT_LocalDetail> list);
        Task<PageResult<IT_LocalDetail>> GetDataListAsync(IT_LocalDetailPageInput input);
        Task UpdataDatasByBussiness(List<BusinessInfo> list, string userId);
    }

    public class IT_LocalDetailQM
    {
        public string LocalName { get; set; }
        public string TrayName { get; set; }
        public string MaterialName { get; set; }
        public string Code { get; set; }
        public DateTime? InTimeStart { get; set; }
        public DateTime? InTimeEnd { get; set; }
    }
    public class IT_LocalDetailPageInput : PageInput<IT_LocalDetailQM>
    {
        public string StorId { get; set; }
    }
}