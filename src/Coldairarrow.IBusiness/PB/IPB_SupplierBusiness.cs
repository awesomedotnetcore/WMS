using Coldairarrow.Entity.PB;
using Coldairarrow.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.PB
{
    public partial interface IPB_SupplierBusiness
    {
        Task<PageResult<PB_Supplier>> GetDataListAsync(PageInput<ConditionDTO> input);
        Task<PB_Supplier> GetTheDataAsync(string id);
        Task AddDataAsync(PB_Supplier data);
        Task UpdateDataAsync(PB_Supplier data);
        Task DeleteDataAsync(List<string> ids);
        Task AddDataExlAsync(List<PB_Supplier> list);
    }
}