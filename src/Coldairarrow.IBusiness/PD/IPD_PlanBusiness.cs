using Coldairarrow.Entity.PD;
using Coldairarrow.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.PD
{
    public partial interface IPD_PlanBusiness
    {
        Task<PageResult<PD_Plan>> GetDataListAsync(PageInput<PD_PlanQM> input);
        Task<PD_Plan> GetTheDataAsync(string id);
        Task AddDataAsync(PD_Plan data);
        Task UpdateDataAsync(PD_Plan data);
        Task DeleteDataAsync(List<string> ids);
    }
    public class PD_PlanQM
    {
        public string Keyword { get; set; }
    }
}