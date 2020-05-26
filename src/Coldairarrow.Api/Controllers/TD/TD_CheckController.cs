using Coldairarrow.Business.TD;
using Coldairarrow.Entity.TD;
using Coldairarrow.Util;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Api.Controllers.TD
{
    [Route("/TD/[controller]/[action]")]
    public class TD_CheckController : BaseApiController
    {
        #region DI

        public TD_CheckController(ITD_CheckBusiness tD_CheckBus)
        {
            _tD_CheckBus = tD_CheckBus;
        }

        ITD_CheckBusiness _tD_CheckBus { get; }

        #endregion

        #region 获取

        [HttpPost]
        public async Task<PageResult<TD_Check>> GetDataList(PageInput<ConditionDTO> input)
        {
            return await _tD_CheckBus.GetDataListAsync(input);
        }

        [HttpPost]
        public async Task<TD_Check> GetTheData(IdInputDTO input)
        {
            return await _tD_CheckBus.GetTheDataAsync(input.id);
        }

        #endregion

        #region 提交

        [HttpPost]
        public async Task SaveData(TD_Check data)
        {
            if (data.Id.IsNullOrEmpty())
            {
                InitEntity(data);

                await _tD_CheckBus.AddDataAsync(data);
            }
            else
            {
                await _tD_CheckBus.UpdateDataAsync(data);
            }
        }

        [HttpPost]
        public async Task DeleteData(List<string> ids)
        {
            await _tD_CheckBus.DeleteDataAsync(ids);
        }

        #endregion
    }
}