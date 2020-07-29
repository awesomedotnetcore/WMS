using Coldairarrow.Entity.PB;
using Coldairarrow.IBusiness.DTO;
using Coldairarrow.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.PB
{
    public partial interface IPB_SupplierBusiness
    {
        Task<PageResult<PB_Supplier>> QueryDataListAsync(PageInput<PBSupplierCoditionDTO> input);

        Task<List<PB_Supplier>> QueryAllDataAsync();

        Task<List<PB_Supplier>> GetQueryData(SelectQueryDTO search);

        Task<PB_Supplier> GetByCode(string code);
    }
}