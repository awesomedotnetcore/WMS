using Coldairarrow.Entity.PB;
using Coldairarrow.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.PB
{
    public partial interface IPB_CustomerBusiness
    {
        Task<PageResult<PB_Customer>> GetDataListAsync(PageInput<ConditionDTO> input);
        Task<PB_Customer> GetTheDataAsync(string id);
        Task AddDataAsync(PB_Customer data);
        Task UpdateDataAsync(PB_Customer data);
        Task DeleteDataAsync(List<string> ids);
        Task AddDataExlAsync(List<PB_Customer> list);
    }
}