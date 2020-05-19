using Coldairarrow.Entity.PB;
using Coldairarrow.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.PB
{
    public interface IPB_LanewayBusiness
    {
        Task<PageResult<PB_Laneway>> GetDataListAsync(PageInput<PB_LanewayQM> input);
        Task<PB_Laneway> GetTheDataAsync(string id);
        Task AddDataAsync(PB_Laneway data);
        Task UpdateDataAsync(PB_Laneway data);
        Task DeleteDataAsync(List<string> ids);
    }
    public class PB_LanewayQM
    {
        public string Name { get; set; }
        public string Code { get; set; }
    }
}