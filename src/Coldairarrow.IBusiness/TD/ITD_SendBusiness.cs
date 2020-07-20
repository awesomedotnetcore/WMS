using Coldairarrow.Entity.TD;
using Coldairarrow.Util;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.TD
{
    public partial interface ITD_SendBusiness
    {
        //Task<PageResult<TD_Send>> GetDataListAsync(PageInput<ConditionDTO> input);
        Task<TD_Send> GetTheDataAsync(string id);
        Task AddDataAsync(TD_Send data);
        Task UpdateDataAsync(TD_Send data);
        Task DeleteDataAsync(List<string> ids);
    }

    public class TD_SendQM
    {
        public string Code { get; set; }
        public int? Status { get; set; }
        public string OutType { get; set; }
        public DateTime? OutStorTimeStart { get; set; }
        public DateTime? OutStorTimeEnd { get; set; }
    }
    public class TD_SendPageInput : PageInput<TD_SendQM>
    {
        public string StorId { get; set; }
    }
}