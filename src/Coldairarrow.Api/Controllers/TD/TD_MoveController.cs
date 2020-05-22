using Coldairarrow.Business.TD;
using Coldairarrow.Entity.TD;
using Coldairarrow.Util;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Api.Controllers.TD
{
    [Route("/TD/[controller]/[action]")]
    public class TD_MoveController : BaseApiController
    {
        #region DI

        public TD_MoveController(ITD_MoveBusiness tD_MoveBus)
        {
            _tD_MoveBus = tD_MoveBus;
        }

        ITD_MoveBusiness _tD_MoveBus { get; }

        #endregion

        #region 获取

        [HttpPost]
        public async Task<PageResult<TD_Move>> GetDataList(PageInput<ConditionDTO> input)
        {
            return await _tD_MoveBus.GetDataListAsync(input);
        }

        [HttpPost]
        public async Task<TD_Move> GetTheData(IdInputDTO input)
        {
            return await _tD_MoveBus.GetTheDataAsync(input.id);
        }

        #endregion

        #region 提交

        [HttpPost]
        public async Task SaveData(TD_Move data)
        {
            if (data.Id.IsNullOrEmpty())
            {
                InitEntity(data);

                await _tD_MoveBus.AddDataAsync(data);
            }
            else
            {
                await _tD_MoveBus.UpdateDataAsync(data);
            }
        }

        [HttpPost]
        public async Task DeleteData(List<string> ids)
        {
            await _tD_MoveBus.DeleteDataAsync(ids);
        }

        #endregion
    }
}