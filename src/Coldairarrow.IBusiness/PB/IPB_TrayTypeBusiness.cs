using Coldairarrow.Entity.PB;
using Coldairarrow.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.PB
{
    public partial interface IPB_TrayTypeBusiness
    {
        Task<PageResult<PB_TrayType>> GetDataListAsync(PageInput<ConditionDTO> input);
        Task<List<PB_TrayType>> GetDataListAsync();
        Task<PageResult<PB_TrayType>> GetDataListBySearch(PageInput<ConditionDTO> input);
        Task<PB_TrayType> GetTheDataAsync(string id);
        Task AddDataAsync(PB_TrayType data);
        Task UpdateDataAsync(PB_TrayType data);
        Task DeleteDataAsync(List<string> ids);
        Task AddDataExlAsync(List<PB_TrayType> list);
    }
}