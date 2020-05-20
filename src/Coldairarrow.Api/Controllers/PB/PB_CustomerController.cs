using Coldairarrow.Business.PB;
using Coldairarrow.Entity.PB;
using Coldairarrow.Util;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Api.Controllers.PB
{
    [Route("/PB/[controller]/[action]")]
    public class PB_CustomerController : BaseApiController
    {
        #region DI

        public PB_CustomerController(IPB_CustomerBusiness pB_CustomerBus)
        {
            _pB_CustomerBus = pB_CustomerBus;
        }

        IPB_CustomerBusiness _pB_CustomerBus { get; }

        #endregion

        #region 获取

        [HttpPost]
        public async Task<PageResult<PB_Customer>> GetDataList(PageInput<ConditionDTO> input)
        {
            return await _pB_CustomerBus.GetDataListAsync(input);
        }

        [HttpPost]
        public async Task<PB_Customer> GetTheData(IdInputDTO input)
        {
            return await _pB_CustomerBus.GetTheDataAsync(input.id);
        }

        #endregion

        #region 提交

        [HttpPost]
        public async Task SaveData(PB_Customer data)
        {
            if (data.Id.IsNullOrEmpty())
            {
                InitEntity(data);

                await _pB_CustomerBus.AddDataAsync(data);
            }
            else
            {
                await _pB_CustomerBus.UpdateDataAsync(data);
            }
        }

        [HttpPost]
        public async Task DeleteData(List<string> ids)
        {
            await _pB_CustomerBus.DeleteDataAsync(ids);
        }

        #endregion
    }
}