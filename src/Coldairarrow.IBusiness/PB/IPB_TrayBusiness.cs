using Coldairarrow.Entity.PB;
using Coldairarrow.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.PB
{
    public interface IPB_TrayBusiness
    {
        Task<PageResult<PB_Tray>> GetDataListAsync(PageInput<ConditionDTO> input);
        Task<PB_Tray> GetTheDataAsync(string id);
        Task AddDataAsync(PB_Tray data);
        Task UpdateDataAsync(PB_Tray data);
        Task EnableTheData(string id);
        Task DisableTheData(string id);
        Task DeleteDataAsync(List<string> ids);
    }
}