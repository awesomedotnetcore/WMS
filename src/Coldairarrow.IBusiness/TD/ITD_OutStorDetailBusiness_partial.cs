using Coldairarrow.Entity.TD;
using Coldairarrow.Util;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.TD
{
    public partial interface ITD_OutStorDetailBusiness
    {
        Task AddDataAsync(List<TD_OutStorDetail> list);
        Task UpdateDataAsync(List<TD_OutStorDetail> list);

        Task<PageResult<TD_OutStorDetail>> GetDataListAsync(TD_OutStorDetailPageInput input);
    }

    public class TD_OutStorDetailQM
    {
        public string Code { get; set; }
        public string OutType { get; set; }
        public DateTime? OutTimeStart { get; set; }
        public DateTime? OutTimeEnd { get; set; }
        public string LocalName { get; set; }
        public string MaterialName { get; set; }
        
    }
    public class TD_OutStorDetailPageInput : PageInput<TD_OutStorDetailQM>
    {
        public string StorId { get; set; }
    }
}