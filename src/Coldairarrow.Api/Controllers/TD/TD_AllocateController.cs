using Coldairarrow.Business.TD;
using Coldairarrow.Entity.TD;
using Coldairarrow.Util;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Api.Controllers.TD
{
    [Route("/TD/[controller]/[action]")]
    public class TD_AllocateController : BaseApiController
    {
        #region DI

        public TD_AllocateController(ITD_AllocateBusiness tD_AllocateBus)
        {
            _tD_AllocateBus = tD_AllocateBus;
        }

        ITD_AllocateBusiness _tD_AllocateBus { get; }

        #endregion

        #region 获取

        [HttpPost]
        public async Task<PageResult<TD_Allocate>> GetDataList(PageInput<ConditionDTO> input)
        {
            return await _tD_AllocateBus.GetDataListAsync(input);
        }

        [HttpPost]
        public async Task<TD_Allocate> GetTheData(IdInputDTO input)
        {
            return await _tD_AllocateBus.GetTheDataAsync(input.id);
        }

        #endregion

        #region 提交

        [HttpPost]
        public async Task SaveData(TD_Allocate data)
        {
            if (data.Id.IsNullOrEmpty())
            {
                InitEntity(data);

                await _tD_AllocateBus.AddDataAsync(data);
            }
            else
            {
                await _tD_AllocateBus.UpdateDataAsync(data);
            }
        }

        [HttpPost]
        public async Task DeleteData(List<string> ids)
        {
            await _tD_AllocateBus.DeleteDataAsync(ids);
        }

        #endregion
    }
}