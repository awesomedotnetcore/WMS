using Coldairarrow.Entity.PB;
using Coldairarrow.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.PB
{
    public partial interface IPB_BarCodeSerialBusiness
    {
        Task<PageResult<PB_BarCodeSerial>> GetDataListAsync(PageInput<ConditionDTO> input);
        Task<PB_BarCodeSerial> GetTheDataAsync(string id);
        Task AddDataAsync(PB_BarCodeSerial data);
        Task UpdateDataAsync(PB_BarCodeSerial data);
        Task DeleteDataAsync(List<string> ids);
    }
}