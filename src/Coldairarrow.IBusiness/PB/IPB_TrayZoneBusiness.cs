using Coldairarrow.Entity.PB;
using Coldairarrow.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.PB
{
    public interface IPB_TrayZoneBusiness
    {
        Task<PageResult<PB_TrayZone>> GetDataListAsync(PageInput<ConditionDTO> input);
        Task<List<PB_TrayZone>> GetDataListAsync(string typeId);
        Task<PB_TrayZone> GetTheDataAsync(string id);
        Task AddDataAsync(PB_TrayZone data);
        Task UpdateDataAsync(PB_TrayZone data);
        Task DeleteDataAsync(List<string> ids);
    }
}