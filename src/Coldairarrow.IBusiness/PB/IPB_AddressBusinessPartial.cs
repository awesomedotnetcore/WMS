using Coldairarrow.Entity.PB;
using Coldairarrow.IBusiness.DTO;
using Coldairarrow.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.PB
{
    public partial interface IPB_AddressBusiness
    {
        Task<PageResult<PB_Address>> QueryDataListAsync(PageInput<PBAddressConditionDTO> input);

        Task ModifyDefaultAsync(string Id);

        Task ModifyEnableAsync(string Id);
    }
}