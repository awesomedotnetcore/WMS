using Coldairarrow.Entity.PB;
using Coldairarrow.IBusiness.DTO;
using Coldairarrow.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.PB
{
    public partial interface IPB_CustomerBusiness
    {
        Task<PageResult<PB_Customer>> QueryDataListAsync(PageInput<PBCustomerCoditionDTO> input);

        Task<List<PB_Customer>> QueryAllDataAsync();

        Task<List<PB_Customer>> GetQueryData(SelectQueryDTO search);
        Task<PB_Customer> GetByCode(string code);
    }
}