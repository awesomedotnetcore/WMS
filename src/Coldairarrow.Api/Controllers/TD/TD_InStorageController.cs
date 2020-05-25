using Coldairarrow.Business.TD;
using Coldairarrow.Entity.TD;
using Coldairarrow.Util;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Api.Controllers.TD
{
    [Route("/TD/[controller]/[action]")]
    public class TD_InStorageController : BaseApiController
    {
        #region DI

        public TD_InStorageController(ITD_InStorageBusiness tD_InStorageBus)
        {
            _tD_InStorageBus = tD_InStorageBus;
        }

        ITD_InStorageBusiness _tD_InStorageBus { get; }

        #endregion

        #region 获取

        [HttpPost]
        public async Task<PageResult<TD_InStorage>> GetDataList(PageInput<ConditionDTO> input)
        {
            return await _tD_InStorageBus.GetDataListAsync(input);
        }

        [HttpPost]
        public async Task<TD_InStorage> GetTheData(IdInputDTO input)
        {
            return await _tD_InStorageBus.GetTheDataAsync(input.id);
        }

        #endregion

        #region 提交

        [HttpPost]
        public async Task SaveData(TD_InStorage data)
        {
            if (data.Id.IsNullOrEmpty())
            {
                InitEntity(data);

                await _tD_InStorageBus.AddDataAsync(data);
            }
            else
            {
                await _tD_InStorageBus.UpdateDataAsync(data);
            }
        }

        [HttpPost]
        public async Task DeleteData(List<string> ids)
        {
            await _tD_InStorageBus.DeleteDataAsync(ids);
        }

        #endregion
    }
}