using Coldairarrow.Entity.PB;
using Coldairarrow.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.PB
{
    public partial interface IPB_MaterialPointBusiness
    {
        Task<PageResult<PB_MaterialPoint>> GetDataListAsync(PageInput<ConditionDTO> input);
        Task<PB_MaterialPoint> GetTheDataAsync(string id);
        Task AddDataAsync(PB_MaterialPoint data);
        Task UpdateDataAsync(PB_MaterialPoint data);
        Task DeleteDataAsync(List<string> ids);
    }
}