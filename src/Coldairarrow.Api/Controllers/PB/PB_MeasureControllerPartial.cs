using Coldairarrow.Business.PB;
using Coldairarrow.Entity.PB;
using Coldairarrow.Util;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Api.Controllers.PB
{
    public partial class PB_MeasureController
    {
        [HttpPost]
        public async Task<List<PB_Measure>> QueryAllData()
        {
            return await _pB_MeasureBus.QueryAllDataAsync();
        }
    }
}