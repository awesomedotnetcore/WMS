using Coldairarrow.Business.TD;
using Coldairarrow.Entity.TD;
using Coldairarrow.Util;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Api.Controllers.TD
{
    [Route("/TD/[controller]/[action]")]
    public class TD_BadController : BaseApiController
    {
        #region DI

        public TD_BadController(ITD_BadBusiness tD_BadBus)
        {
            _tD_BadBus = tD_BadBus;
        }

        ITD_BadBusiness _tD_BadBus { get; }

        #endregion

        #region 获取

        [HttpPost]
        public async Task<PageResult<TD_Bad>> GetDataList(PageInput<ConditionDTO> input)
        {
            return await _tD_BadBus.GetDataListAsync(input);
        }

        [HttpPost]
        public async Task<TD_Bad> GetTheData(IdInputDTO input)
        {
            return await _tD_BadBus.GetTheDataAsync(input.id);
        }

        #endregion

        #region 提交

        [HttpPost]
        public async Task SaveData(TD_Bad data)
        {
            if (data.Id.IsNullOrEmpty())
            {
                InitEntity(data);

                await _tD_BadBus.AddDataAsync(data);
            }
            else
            {
                await _tD_BadBus.UpdateDataAsync(data);
            }
        }

        [HttpPost]
        public async Task DeleteData(List<string> ids)
        {
            await _tD_BadBus.DeleteDataAsync(ids);
        }

        #endregion
    }
}