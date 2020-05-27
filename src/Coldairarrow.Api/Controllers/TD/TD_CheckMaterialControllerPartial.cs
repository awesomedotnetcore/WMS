using Coldairarrow.Business.TD;
using Coldairarrow.Entity.PB;
using Coldairarrow.Entity.TD;
using Coldairarrow.Util;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Api.Controllers.TD
{
    public partial class TD_CheckMaterialController
    {
        [HttpPost]
        public async Task<List<PB_Material>> Query(string checkId)
        {

            return await _tD_CheckMaterialBus.QueryAsync(checkId);
        }
    }
}