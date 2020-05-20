using Coldairarrow.Business.PB;
using Coldairarrow.Entity.PB;
using Coldairarrow.Util;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Api.Controllers.PB
{
    [Route("/PB/[controller]/[action]")]
    public class PB_MeasureController : BaseApiController
    {
        #region DI

        public PB_MeasureController(IPB_MeasureBusiness pB_MeasureBus)
        {
            _pB_MeasureBus = pB_MeasureBus;
        }

        IPB_MeasureBusiness _pB_MeasureBus { get; }

        #endregion

        #region 获取

        [HttpPost]
        public async Task<PageResult<PB_Measure>> GetDataList(PageInput<ConditionDTO> input)
        {
            return await _pB_MeasureBus.GetDataListAsync(input);
        }

        [HttpPost]
        public async Task<PB_Measure> GetTheData(IdInputDTO input)
        {
            return await _pB_MeasureBus.GetTheDataAsync(input.id);
        }

        #endregion

        #region 提交

        [HttpPost]
        public async Task SaveData(PB_Measure data)
        {
            if (data.Id.IsNullOrEmpty())
            {
                InitEntity(data);

                await _pB_MeasureBus.AddDataAsync(data);
            }
            else
            {
                await _pB_MeasureBus.UpdateDataAsync(data);
            }
        }

        [HttpPost]
        public async Task DeleteData(List<string> ids)
        {
            await _pB_MeasureBus.DeleteDataAsync(ids);
        }

        #endregion
    }
}