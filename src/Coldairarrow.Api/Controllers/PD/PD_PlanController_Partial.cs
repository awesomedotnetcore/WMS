using Coldairarrow.Business.PD;
using Coldairarrow.Entity.PD;
using Coldairarrow.Util;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Api.Controllers.PD
{
    public partial class PD_PlanController
    {
        [HttpPost]
        public async Task<List<PD_Plan>> QueryAllData()
        {
            return await _pD_PlanBus.QueryAllDataAsync();
        }
    }
}
