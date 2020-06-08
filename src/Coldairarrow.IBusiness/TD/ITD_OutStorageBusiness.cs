using Coldairarrow.Entity.TD;
using Coldairarrow.Util;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.TD
{
    public partial interface ITD_OutStorageBusiness
    {
        Task<TD_OutStorage> GetTheDataAsync(string id);
        Task AddDataAsync(TD_OutStorage data);
        Task UpdateDataAsync(TD_OutStorage data);
        Task DeleteDataAsync(List<string> ids);
        
    }

    public class TD_OutStorageQM
    {
        public string Code { get; set; }
        public int? Status { get; set; }
        public string OutType { get; set; }
        public DateTime? OutStorTimeStart { get; set; }
        public DateTime? OutStorTimeEnd { get; set; }
    }
    public class TD_OutStoragePageInput : PageInput<TD_OutStorageQM>
    {
        public string StorId { get; set; }
    }
}