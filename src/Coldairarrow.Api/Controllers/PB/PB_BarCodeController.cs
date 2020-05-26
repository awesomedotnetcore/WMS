using Coldairarrow.Business.PB;
using Coldairarrow.Entity.PB;
using Coldairarrow.Util;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Api.Controllers.PB
{
    [Route("/PB/[controller]/[action]")]
    public class PB_BarCodeController : BaseApiController
    {
        #region DI

        public PB_BarCodeController(IPB_BarCodeBusiness pB_BarCodeBus)
        {
            _pB_BarCodeBus = pB_BarCodeBus;
        }

        IPB_BarCodeBusiness _pB_BarCodeBus { get; }

        #endregion

        #region 获取

        [HttpPost]
        public async Task<PageResult<PB_BarCode>> GetDataList(PageInput<PB_BarCodeQM> input)
        {
            return await _pB_BarCodeBus.GetDataListAsync(input);
        }

        [HttpPost]
        public async Task<PB_BarCode> GetTheData(IdInputDTO input)
        {
            return await _pB_BarCodeBus.GetTheDataAsync(input.id);
        }

        #endregion

        #region 提交

        [HttpPost]
        public async Task SaveData(PB_BarCode data)
        {
            if (data.Id.IsNullOrEmpty())
            {
                InitEntity(data);

                await _pB_BarCodeBus.AddDataAsync(data);
            }
            else
            {
                await _pB_BarCodeBus.UpdateDataAsync(data);
            }
        }

        [HttpPost]
        public async Task DeleteData(List<string> ids)
        {
            await _pB_BarCodeBus.DeleteDataAsync(ids);
        }

        #endregion
    }
}