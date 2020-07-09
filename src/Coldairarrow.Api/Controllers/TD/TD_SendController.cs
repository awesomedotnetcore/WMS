using Coldairarrow.Business.TD;
using Coldairarrow.Entity.TD;
using Coldairarrow.Util;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Api.Controllers.TD
{
    [Route("/TD/[controller]/[action]")]
    public class TD_SendController : BaseApiController
    {
        #region DI

        public TD_SendController(ITD_SendBusiness tD_SendBus)
        {
            _tD_SendBus = tD_SendBus;
        }

        ITD_SendBusiness _tD_SendBus { get; }

        #endregion

        #region 获取

        [HttpPost]
        public async Task<PageResult<TD_Send>> GetDataList(PageInput<ConditionDTO> input)
        {
            return await _tD_SendBus.GetDataListAsync(input);
        }

        [HttpPost]
        public async Task<TD_Send> GetTheData(IdInputDTO input)
        {
            return await _tD_SendBus.GetTheDataAsync(input.id);
        }

        #endregion

        #region 提交

        [HttpPost]
        public async Task SaveData(TD_Send data)
        {
            if (data.Id.IsNullOrEmpty())
            {
                InitEntity(data);

                await _tD_SendBus.AddDataAsync(data);
            }
            else
            {
                await _tD_SendBus.UpdateDataAsync(data);
            }
        }

        [HttpPost]
        public async Task DeleteData(List<string> ids)
        {
            await _tD_SendBus.DeleteDataAsync(ids);
        }

        #endregion
    }
}