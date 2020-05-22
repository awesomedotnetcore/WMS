using Coldairarrow.Entity.PB;
using Coldairarrow.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.PB
{
    public partial interface IPB_AddressBusiness
    {
        Task<PageResult<PB_Address>> GetDataListAsync(PageInput<ConditionDTO> input);
        Task<PB_Address> GetTheDataAsync(string id);
        Task AddDataAsync(PB_Address data);
        Task UpdateDataAsync(PB_Address data);
        Task DeleteDataAsync(List<string> ids);
    }
}