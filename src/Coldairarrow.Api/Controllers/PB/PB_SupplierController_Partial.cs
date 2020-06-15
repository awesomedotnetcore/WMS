using Coldairarrow.Business.PB;
using Coldairarrow.Entity.PB;
using Coldairarrow.IBusiness.DTO;
using Coldairarrow.Util;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Api.Controllers.PB
{
    public partial class PB_SupplierController
    {
        [HttpPost]
        public async Task<PageResult<PB_Supplier>> QueryDataList(PageInput<PBSupplierCoditionDTO> input)
        {
            return await _pB_SupplierBus.QueryDataListAsync(input);
        }

        [HttpPost]
        public async Task<List<PB_Supplier>> QueryAllData()
        {
            return await _pB_SupplierBus.QueryAllDataAsync();
        }

        [HttpPost]
        public async Task<List<PB_Supplier>> GetQueryData(SelectQueryDTO search)
        {
            return await _pB_SupplierBus.GetQueryData(search);
        }
    }
}