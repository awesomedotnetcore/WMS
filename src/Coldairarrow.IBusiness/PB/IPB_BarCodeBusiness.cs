using Coldairarrow.Entity.PB;
using Coldairarrow.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.PB
{
    public interface IPB_BarCodeBusiness
    {
        Task<PageResult<PB_BarCode>> GetDataListAsync(PageInput<ConditionDTO> input);
        Task<PB_BarCode> GetTheDataAsync(string id);
        Task AddDataAsync(PB_BarCode data);
        Task UpdateDataAsync(PB_BarCode data);
        Task DeleteDataAsync(List<string> ids);
    }
}