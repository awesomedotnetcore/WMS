using Coldairarrow.Business.PB;
using Coldairarrow.Entity.PB;
using Coldairarrow.Util;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Api.Controllers.PB
{
    [Route("/PB/[controller]/[action]")]
    public class PB_BarCodeSerialController : BaseApiController
    {
        #region DI

        public PB_BarCodeSerialController(IPB_BarCodeSerialBusiness pB_BarCodeSerialBus)
        {
            _pB_BarCodeSerialBus = pB_BarCodeSerialBus;
        }

        IPB_BarCodeSerialBusiness _pB_BarCodeSerialBus { get; }

        #endregion

        #region 获取

        [HttpPost]
        public async Task<PageResult<PB_BarCodeSerial>> GetDataList(PageInput<ConditionDTO> input)
        {
            return await _pB_BarCodeSerialBus.GetDataListAsync(input);
        }

        [HttpPost]
        public async Task<PB_BarCodeSerial> GetTheData(IdInputDTO input)
        {
            return await _pB_BarCodeSerialBus.GetTheDataAsync(input.id);
        }

        #endregion

        #region 提交

        [HttpPost]
        public async Task SaveData(PB_BarCodeSerial data)
        {
            if (data.Id.IsNullOrEmpty())
            {
                InitEntity(data);

                await _pB_BarCodeSerialBus.AddDataAsync(data);
            }
            else
            {
                await _pB_BarCodeSerialBus.UpdateDataAsync(data);
            }
        }

        [HttpPost]
        public async Task DeleteData(List<string> ids)
        {
            await _pB_BarCodeSerialBus.DeleteDataAsync(ids);
        }

        #endregion
    }
}