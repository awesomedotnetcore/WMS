using Coldairarrow.Entity.PB;
using Coldairarrow.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.PB
{
    public interface IPB_StorAreaBusiness
    {
        Task<PageResult<PB_StorArea>> GetDataListAsync(PageInput<ConditionDTO> input);
        Task<PB_StorArea> GetTheDataAsync(string id);
        Task<List<PB_StorArea>> QueryStorAreaDataAsync();
        Task AddDataAsync(PB_StorArea data);
        Task UpdateDataAsync(PB_StorArea data);
        Task DeleteDataAsync(List<string> ids);
    }
}