using Coldairarrow.Entity.PB;
using Coldairarrow.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.PB
{
    public partial interface IPB_MeasureBusiness
    {
        Task<PageResult<PB_Measure>> GetDataListAsync(PageInput<ConditionDTO> input);
        Task<PB_Measure> GetTheDataAsync(string id);
        Task AddDataAsync(PB_Measure data);
        Task UpdateDataAsync(PB_Measure data);
        Task DeleteDataAsync(List<string> ids);
    }
}