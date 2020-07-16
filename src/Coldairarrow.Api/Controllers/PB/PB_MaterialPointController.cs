using Coldairarrow.Business.PB;
using Coldairarrow.Entity.PB;
using Coldairarrow.Util;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Api.Controllers.PB
{
    [Route("/PB/[controller]/[action]")]
    public class PB_MaterialPointController : BaseApiController
    {
        #region DI

        public PB_MaterialPointController(IPB_MaterialPointBusiness pB_MaterialPointBus)
        {
            _pB_MaterialPointBus = pB_MaterialPointBus;
        }

        IPB_MaterialPointBusiness _pB_MaterialPointBus { get; }

        #endregion

        #region 获取

        [HttpPost]
        public async Task<PageResult<PB_MaterialPoint>> GetDataList(PageInput<ConditionDTO> input)
        {
            return await _pB_MaterialPointBus.GetDataListAsync(input);
        }

        [HttpPost]
        public async Task<PB_MaterialPoint> GetTheData(IdInputDTO input)
        {
            return await _pB_MaterialPointBus.GetTheDataAsync(input.id);
        }

        #endregion

        #region 提交

        [HttpPost]
        public async Task SaveData(PB_MaterialPoint data)
        {
            if (data.Id.IsNullOrEmpty())
            {
                InitEntity(data);

                await _pB_MaterialPointBus.AddDataAsync(data);
            }
            else
            {
                await _pB_MaterialPointBus.UpdateDataAsync(data);
            }
        }

        [HttpPost]
        public async Task DeleteData(List<string> ids)
        {
            await _pB_MaterialPointBus.DeleteDataAsync(ids);
        }

        #endregion
    }
}