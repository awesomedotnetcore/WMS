using Coldairarrow.Business.TD;
using Coldairarrow.Entity.TD;
using Coldairarrow.Util;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Api.Controllers.TD
{
    [Route("/TD/[controller]/[action]")]
    public partial class TD_CheckAreaController : BaseApiController
    {
        #region DI

        public TD_CheckAreaController(ITD_CheckAreaBusiness tD_CheckAreaBus)
        {
            _tD_CheckAreaBus = tD_CheckAreaBus;
        }

        ITD_CheckAreaBusiness _tD_CheckAreaBus { get; }

        #endregion

        #region 获取

        [HttpPost]
        public async Task<PageResult<TD_CheckArea>> GetDataList(PageInput<ConditionDTO> input)
        {
            return await _tD_CheckAreaBus.GetDataListAsync(input);
        }

        [HttpPost]
        public async Task<TD_CheckArea> GetTheData(IdInputDTO input)
        {
            return await _tD_CheckAreaBus.GetTheDataAsync(input.id);
        }

        #endregion

        #region 提交

        [HttpPost]
        public async Task SaveData(TD_CheckArea data)
        {
            await _tD_CheckAreaBus.UpdateDataAsync(data);
        }

        [HttpPost]
        public async Task DeleteData(List<string> ids)
        {
            await _tD_CheckAreaBus.DeleteDataAsync(ids);
        }

        #endregion
    }
}