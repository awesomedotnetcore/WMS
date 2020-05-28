using Coldairarrow.Business.TD;
using Coldairarrow.Entity.TD;
using Coldairarrow.Util;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Api.Controllers.TD
{
    [Route("/TD/[controller]/[action]")]
    public class TD_OutStorageController : BaseApiController
    {
        #region DI

        public TD_OutStorageController(ITD_OutStorageBusiness tD_OutStorageBus)
        {
            _tD_OutStorageBus = tD_OutStorageBus;
        }

        ITD_OutStorageBusiness _tD_OutStorageBus { get; }

        #endregion

        #region 获取

        [HttpPost]
        public async Task<PageResult<TD_OutStorage>> GetDataList(PageInput<ConditionDTO> input)
        {
            return await _tD_OutStorageBus.GetDataListAsync(input);
        }

        [HttpPost]
        public async Task<TD_OutStorage> GetTheData(IdInputDTO input)
        {
            return await _tD_OutStorageBus.GetTheDataAsync(input.id);
        }

        #endregion

        #region 提交

        [HttpPost]
        public async Task SaveData(TD_OutStorage data)
        {
            if (data.Id.IsNullOrEmpty())
            {
                InitEntity(data);

                await _tD_OutStorageBus.AddDataAsync(data);
            }
            else
            {
                await _tD_OutStorageBus.UpdateDataAsync(data);
            }
        }

        [HttpPost]
        public async Task DeleteData(List<string> ids)
        {
            await _tD_OutStorageBus.DeleteDataAsync(ids);
        }

        #endregion
    }
}