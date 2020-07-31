using Coldairarrow.Entity.PB;
using Coldairarrow.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.PB
{
    public partial interface IPB_MaterialTypeBusiness
    {
        Task<PageResult<PB_MaterialType>> GetDataListAsync(PageInput<ConditionDTO> input);
        Task<PB_MaterialType> GetTheDataAsync(string id);
        Task AddDataAsync(PB_MaterialType data);
        Task UpdateDataAsync(PB_MaterialType data);
        Task DeleteDataAsync(List<string> ids);
        Task AddDataExlAsync(List<PB_MaterialType> list);
    }
}