using Coldairarrow.Entity.PB;
using Coldairarrow.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.PB
{
    public partial interface IPB_MaterialBusiness
    {
        Task<PageResult<PB_Material>> GetDataListAsync(PageInput<ConditionDTO> input);
        Task<List<PB_Material>> GetDataListAsync();
        Task<PB_Material> GetTheDataAsync(string id);
        Task AddDataAsync(PB_Material data);
        Task UpdateDataAsync(PB_Material data);
        Task DeleteDataAsync(List<string> ids);
        Task AddDataExlAsync(List<PB_Material> list);


    }
}