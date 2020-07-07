using Coldairarrow.Business.TD;
using Coldairarrow.Entity.TD;
using Coldairarrow.Util;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Api.Controllers.TD
{
    [Route("/TD/[controller]/[action]")]
    public class TD_ReceivingController : BaseApiController
    {
        #region DI

        public TD_ReceivingController(ITD_ReceivingBusiness tD_ReceivingBus)
        {
            _tD_ReceivingBus = tD_ReceivingBus;
        }

        ITD_ReceivingBusiness _tD_ReceivingBus { get; }

        #endregion

        #region 获取

        [HttpPost]
        public async Task<PageResult<TD_Receiving>> GetDataList(PageInput<ConditionDTO> input)
        {
            return await _tD_ReceivingBus.GetDataListAsync(input);
        }

        [HttpPost]
        public async Task<TD_Receiving> GetTheData(IdInputDTO input)
        {
            return await _tD_ReceivingBus.GetTheDataAsync(input.id);
        }

        #endregion

        #region 提交

        [HttpPost]
        public async Task SaveData(TD_Receiving data)
        {
            if (data.Id.IsNullOrEmpty())
            {
                InitEntity(data);

                await _tD_ReceivingBus.AddDataAsync(data);
            }
            else
            {
                await _tD_ReceivingBus.UpdateDataAsync(data);
            }
        }

        [HttpPost]
        public async Task DeleteData(List<string> ids)
        {
            await _tD_ReceivingBus.DeleteDataAsync(ids);
        }

        #endregion
    }
}