using Coldairarrow.Entity.TD;
using Coldairarrow.Util;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.TD
{
    public partial interface ITD_SendDetailBusiness
    {
        Task AddDataAsync(List<TD_SendDetail> list);
        Task UpdateDataAsync(List<TD_SendDetail> list);

        Task<PageResult<TD_SendDetail>> GetDataListAsync(TD_SendDetailPageInput input);

    }
    public class TD_SendDetailQM
    {
        public string Code { get; set; }
        public string Type { get; set; }
        public DateTime? OutTimeStart { get; set; }
        public DateTime? OutTimeEnd { get; set; }
        public string MaterialName { get; set; }
        public string MeasureName { get; set; }

    }
    public class TD_SendDetailPageInput : PageInput<TD_SendDetailQM>
    {
        public string StorId { get; set; }
    }
}