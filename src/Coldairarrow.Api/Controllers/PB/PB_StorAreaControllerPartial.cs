using Coldairarrow.Business.PB;
using Coldairarrow.Entity.PB;
using Coldairarrow.Util;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Coldairarrow.Api.Controllers.PB
{
    public partial class PB_StorAreaController 
    {
        /// <summary>
        /// 查询货区
        /// </summary>
        [HttpPost]
        public async Task<List<PB_StorArea>> Query()
        {
            var storageId = (await _base_UserStorBus.GetStorage(_Op.UserId)).Where(p => p.IsDefault == true).FirstOrDefault().Id;

            return await _pB_StorAreaBus.QueryAsync(storageId);
        }
    }
}