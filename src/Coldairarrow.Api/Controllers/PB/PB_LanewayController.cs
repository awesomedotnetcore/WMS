using Coldairarrow.Business.PB;
using Coldairarrow.Entity.PB;
using Coldairarrow.Util;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Api.Controllers.PB
{
    [Route("/PB/[controller]/[action]")]
    public class PB_LanewayController : BaseApiController
    {
        #region DI

        public PB_LanewayController(IPB_LanewayBusiness pB_LanewayBus)
        {
            _pB_LanewayBus = pB_LanewayBus;
        }

        IPB_LanewayBusiness _pB_LanewayBus { get; }

        #endregion

        #region 获取

        [HttpPost]
        public async Task<PageResult<PB_Laneway>> GetDataList(PB_LanewayPageInput input)
        {
            return await _pB_LanewayBus.GetDataListAsync(input);
        }

        [HttpPost]
        public async Task<PB_Laneway> GetTheData(IdInputDTO input)
        {
            return await _pB_LanewayBus.GetTheDataAsync(input.id);
        }

        #endregion

        #region 提交

        [HttpPost]
        public async Task SaveData(PB_Laneway data)
        {
            if (data.Id.IsNullOrEmpty())
            {
                InitEntity(data);

                await _pB_LanewayBus.AddDataAsync(data);
            }
            else
            {
                await _pB_LanewayBus.UpdateDataAsync(data);
            }
        }

        [HttpPost]
        public async Task DeleteData(List<string> ids)
        {
            await _pB_LanewayBus.DeleteDataAsync(ids);
        }

        #endregion
    }
}