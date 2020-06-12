using Coldairarrow.Entity.TD;
using Coldairarrow.Util;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.TD
{
    public partial interface ITD_InStorageBusiness
    {
        
        Task<TD_InStorage> GetTheDataAsync(string id);
        Task AddDataAsync(TD_InStorage data);
        Task UpdateDataAsync(TD_InStorage data);
        Task DeleteDataAsync(List<string> ids);
    }

    public class TD_InStorageQM
    {
        public string Code { get; set; }
        public string InType { get; set; }
        public int? Status { get; set; }
        public DateTime? InStorTimeStart { get; set; }
        public DateTime? InStorTimeEnd { get; set; }
    }
    public class TD_InStoragePageInput : PageInput<TD_InStorageQM>
    {
        public string StorId { get; set; }
    }
}