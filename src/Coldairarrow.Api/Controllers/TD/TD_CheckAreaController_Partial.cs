using Coldairarrow.Business.TD;
using Coldairarrow.Entity.TD;
using Coldairarrow.Util;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Api.Controllers.TD
{
    public partial class TD_CheckAreaController
    {
        [HttpPost]
        public async Task<List<string>> Query(string checkId)
        {
            return await _tD_CheckAreaBus.QueryAsync(checkId);
        }
    }
}