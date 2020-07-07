using Coldairarrow.Business.TD;
using Coldairarrow.Entity.TD;
using Coldairarrow.Util;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Api.Controllers.TD
{
    [Route("/TD/[controller]/[action]")]
    public class TD_RecDetailController : BaseApiController
    {
        #region DI

        public TD_RecDetailController(ITD_RecDetailBusiness tD_RecDetailBus)
        {
            _tD_RecDetailBus = tD_RecDetailBus;
        }

        ITD_RecDetailBusiness _tD_RecDetailBus { get; }

        #endregion

        #region 获取

        [HttpPost]
        public async Task<PageResult<TD_RecDetail>> GetDataList(PageInput<ConditionDTO> input)
        {
            return await _tD_RecDetailBus.GetDataListAsync(input);
        }

        [HttpPost]
        public async Task<TD_RecDetail> GetTheData(IdInputDTO input)
        {
            return await _tD_RecDetailBus.GetTheDataAsync(input.id);
        }

        #endregion

        #region 提交

        [HttpPost]
        public async Task SaveData(TD_RecDetail data)
        {
            if (data.Id.IsNullOrEmpty())
            {
                InitEntity(data);

                await _tD_RecDetailBus.AddDataAsync(data);
            }
            else
            {
                await _tD_RecDetailBus.UpdateDataAsync(data);
            }
        }

        [HttpPost]
        public async Task DeleteData(List<string> ids)
        {
            await _tD_RecDetailBus.DeleteDataAsync(ids);
        }

        #endregion
    }
}