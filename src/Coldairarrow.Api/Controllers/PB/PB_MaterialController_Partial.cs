using Coldairarrow.Business.PB;
using Coldairarrow.Entity.PB;
using Coldairarrow.IBusiness.DTO;
using Coldairarrow.Util;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Api.Controllers.PB
{
    public partial class PB_MaterialController
    {
        [HttpPost]
        public async Task<PageResult<PB_Material>> QueryDataList(PageInput<PBMaterialConditionDTO> input)
        {
            return await _pB_MaterialBus.QueryDataListAsync(input);
        }
        [HttpGet]
        public async Task<PB_Material> GetByBarcode(string barcode)
        {
            return await _pB_MaterialBus.GetByBarcode(barcode);
        }
        [HttpGet]
        public async Task<PB_Material> GetByCode(string code)
        {
            return await _pB_MaterialBus.GetByCode(code);
        }
    }
}