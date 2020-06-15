using Coldairarrow.Business.PB;
using Coldairarrow.Entity.PB;
using Coldairarrow.IBusiness.DTO;
using Coldairarrow.Util;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Api.Controllers.PB
{
    public partial class PB_CustomerController
    {
        [HttpPost]
        public async Task<PageResult<PB_Customer>> QueryDataList(PageInput<PBCustomerCoditionDTO> input)
        {
            return await _pB_CustomerBus.QueryDataListAsync(input);
        }

        [HttpPost]
        public async Task<List<PB_Customer>> QueryAllData()
        {
            return await _pB_CustomerBus.QueryAllDataAsync();
        }

        [HttpPost]
        public async Task<List<PB_Customer>> GetQueryData(SelectQueryDTO search)
        {
            return await _pB_CustomerBus.GetQueryData(search);
        }
    }
}