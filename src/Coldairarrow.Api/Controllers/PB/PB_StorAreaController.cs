using Coldairarrow.Business.PB;
using Coldairarrow.Entity.PB;
using Coldairarrow.Util;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Api.Controllers.PB
{
    [Route("/PB/[controller]/[action]")]
    public class PB_StorAreaController : BaseApiController
    {
        #region DI

        public PB_StorAreaController(IPB_StorAreaBusiness pB_StorAreaBus)
        {
            _pB_StorAreaBus = pB_StorAreaBus;
        }

        IPB_StorAreaBusiness _pB_StorAreaBus { get; }

        #endregion

        #region 获取

        [HttpPost]
        public async Task<PageResult<PB_StorArea>> GetDataList(PageInput<ConditionDTO> input)
        {
            return await _pB_StorAreaBus.GetDataListAsync(input);
        }

        [HttpPost]
        public async Task<PB_StorArea> GetTheData(IdInputDTO input)
        {
            return await _pB_StorAreaBus.GetTheDataAsync(input.id);
        }

        #endregion

        #region 提交

        [HttpPost]
        public async Task SaveData(PB_StorArea data)
        {
            if (data.Id.IsNullOrEmpty())
            {
                InitEntity(data);

                await _pB_StorAreaBus.AddDataAsync(data);
            }
            else
            {
                await _pB_StorAreaBus.UpdateDataAsync(data);
            }
        }

        [HttpPost]
        public async Task DeleteData(List<string> ids)
        {
            await _pB_StorAreaBus.DeleteDataAsync(ids);
        }

        #endregion
    }
}