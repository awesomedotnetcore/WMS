using Coldairarrow.Entity.PB;
using Coldairarrow.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.PB
{
    public interface IPB_RackBusiness
    {
        Task<PageResult<PB_Rack>> GetDataListAsync(PageInput<ConditionDTO> input);
        Task<PB_Rack> GetTheDataAsync(string id);
        Task AddDataAsync(PB_Rack data);
        Task UpdateDataAsync(PB_Rack data);
        Task DeleteDataAsync(List<string> ids);
    }
}