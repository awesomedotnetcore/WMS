using Coldairarrow.Business.PB;
using Coldairarrow.Entity.PB;
using Coldairarrow.IBusiness.DTO;
using Coldairarrow.Util;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Api.Controllers.PB
{
    public partial class PB_MaterialTypeController
    {
        [HttpPost]
        public async Task<List<MaterialTypeTreeDTO>> GetTreeDataList(string parentId)
        {
            return await _pB_MaterialTypeBus.GetTreeDataListAsync(parentId);
        }
    }
}