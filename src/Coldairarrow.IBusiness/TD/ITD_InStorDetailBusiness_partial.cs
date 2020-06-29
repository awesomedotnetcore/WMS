using Coldairarrow.Entity.TD;
using Coldairarrow.Util;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.TD
{
    public partial interface ITD_InStorDetailBusiness
    {
        Task AddDataAsync(List<TD_InStorDetail> list);
        Task UpdateDataAsync(List<TD_InStorDetail> list);
        Task<PageResult<TD_InStorDetail>> GetDataListAsync(TD_InStorDetailPageInput input);
    }

    public class TD_InStorDetailQM
    {
        public string Code { get; set; }
        public string InType { get; set; }
        public DateTime? InTimeStart { get; set; }
        public DateTime? InTimeEnd { get; set; }
        public string LocalName { get; set; }
        public string MaterialName { get; set; }

    }
    public class TD_InStorDetailPageInput : PageInput<TD_InStorDetailQM>
    {
        public string StorId { get; set; }
    }
}