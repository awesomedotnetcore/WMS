using Coldairarrow.Business.PB;
using Coldairarrow.Entity.PB;
using Coldairarrow.Util;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Api.Controllers.PB
{
    [Route("/PB/[controller]/[action]")]
    public class PB_LocationController : BaseApiController
    {
        #region DI

        public PB_LocationController(IPB_LocationBusiness pB_LocationBus)
        {
            _pB_LocationBus = pB_LocationBus;
        }

        IPB_LocationBusiness _pB_LocationBus { get; }

        #endregion

        #region 获取

        [HttpPost]
        public async Task<PageResult<PB_Location>> GetDataList(PageInput<ConditionDTO> input)
        {
            return await _pB_LocationBus.GetDataListAsync(input);
        }

        [HttpPost]
        public async Task<PB_Location> GetTheData(IdInputDTO input)
        {
            return await _pB_LocationBus.GetTheDataAsync(input.id);
        }

        #endregion

        #region 提交

        [HttpPost]
        public async Task SaveData(PB_Location data)
        {
            if (data.Id.IsNullOrEmpty())
            {
                InitEntity(data);

                await _pB_LocationBus.AddDataAsync(data);
            }
            else
            {
                await _pB_LocationBus.UpdateDataAsync(data);
            }
        }

        [HttpPost]
        public async Task DeleteData(List<string> ids)
        {
            await _pB_LocationBus.DeleteDataAsync(ids);
        }

        #endregion
    }
}