using Coldairarrow.Entity.PB;
using Coldairarrow.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.PB
{
    public interface IPB_RackBusiness
    {
        Task<PageResult<PB_Rack>> GetDataListAsync(PB_RackPageInput input);
        Task<PB_Rack> GetTheDataAsync(string id);
        Task<List<PB_Rack>> GetDataListAsync(string storId);
        Task<List<PB_Rack>> QueryRackDataAsync();
        Task AddDataAsync(PB_Rack data);
        Task UpdateDataAsync(PB_Rack data);
        Task DeleteDataAsync(List<string> ids);
    }
    public class PB_RackQM
    {
        //public string Name { get; set; }
        //public string Code { get; set; }
        public string Keyword { get; set; }
    }
    public class PB_RackPageInput : PageInput<PB_RackQM>
    {
        public string StorId { get; set; }
    }
}