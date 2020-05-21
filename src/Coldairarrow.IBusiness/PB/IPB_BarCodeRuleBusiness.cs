using Coldairarrow.Entity.PB;
using Coldairarrow.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.PB
{
    public interface IPB_BarCodeRuleBusiness
    {
        Task<PageResult<PB_BarCodeRule>> GetDataListAsync(PageInput<ConditionDTO> input);
        Task<PB_BarCodeRule> GetTheDataAsync(string id);
        Task AddDataAsync(PB_BarCodeRule data);
        Task UpdateDataAsync(PB_BarCodeRule data);
        Task DeleteDataAsync(List<string> ids);
    }
}