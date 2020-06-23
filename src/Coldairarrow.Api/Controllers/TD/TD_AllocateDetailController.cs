using Coldairarrow.Business.TD;
using Coldairarrow.Entity.TD;
using Coldairarrow.Util;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Api.Controllers.TD
{
    [Route("/TD/[controller]/[action]")]
    public class TD_AllocateDetailController : BaseApiController
    {
        #region DI

        public TD_AllocateDetailController(ITD_AllocateDetailBusiness tD_AllocateDetailBus)
        {
            _tD_AllocateDetailBus = tD_AllocateDetailBus;
        }

        ITD_AllocateDetailBusiness _tD_AllocateDetailBus { get; }

        #endregion

        #region 获取

        [HttpPost]
        public async Task<PageResult<TD_AllocateDetail>> GetDataList(PageInput<ConditionDTO> input)
        {
            return await _tD_AllocateDetailBus.GetDataListAsync(input);
        }

        [HttpPost]
        public async Task<TD_AllocateDetail> GetTheData(IdInputDTO input)
        {
            return await _tD_AllocateDetailBus.GetTheDataAsync(input.id);
        }

        #endregion

        #region 提交

        [HttpPost]
        public async Task SaveData(TD_AllocateDetail data)
        {
            if (data.Id.IsNullOrEmpty())
            {
                InitEntity(data);

                await _tD_AllocateDetailBus.AddDataAsync(data);
            }
            else
            {
                await _tD_AllocateDetailBus.UpdateDataAsync(data);
            }
        }

        [HttpPost]
        public async Task DeleteData(List<string> ids)
        {
            await _tD_AllocateDetailBus.DeleteDataAsync(ids);
        }

        #endregion
    }
}