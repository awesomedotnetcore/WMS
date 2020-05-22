using Coldairarrow.Business.PB;
using Coldairarrow.Entity.PB;
using Coldairarrow.IBusiness.DTO;
using Coldairarrow.Util;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Api.Controllers.PB
{
    public partial class PB_AddressController
    {
        [HttpPost]
        public async Task<PageResult<PB_Address>> QueryDataList(PageInput<PBAddressConditionDTO> input)
        {
            return await _pB_AddressBus.QueryDataListAsync(input);
        }

        [HttpPost]
        public async Task ModifyDefault(string id)
        {
            await _pB_AddressBus.ModifyDefaultAsync(id);
        }

        [HttpPost]
        public async Task ModifyEnable(string id)
        {
            await _pB_AddressBus.ModifyEnableAsync(id);
        }
    }
}