using Coldairarrow.Business.Base;
using Coldairarrow.Entity.Base;
using Coldairarrow.Util;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Api.Controllers.Base
{
    [Route("/Base/[controller]/[action]")]
    public class Base_ParameterController : BaseApiController
    {
        #region DI

        public Base_ParameterController(IBase_ParameterBusiness base_ParameterBus)
        {
            _base_ParameterBus = base_ParameterBus;
        }

        IBase_ParameterBusiness _base_ParameterBus { get; }

        #endregion

        #region 获取

        [HttpPost]
        public async Task<PageResult<Base_Parameter>> GetDataList(PB_ParameterPageInput input)
        {
            return await _base_ParameterBus.GetDataListAsync(input);
        }

        [HttpPost]
        public async Task<Base_Parameter> GetTheData(IdInputDTO input)
        {
            return await _base_ParameterBus.GetTheDataAsync(input.id);
        }

        [HttpGet]
        public async Task<Dictionary<string, string>> GetConfig()
        {
            return await _base_ParameterBus.GetConfig();
        }

        #endregion

        #region 提交

        [HttpPost]
        public async Task SaveData(Base_Parameter data)
        {
            if (data.Id.IsNullOrEmpty())
            {
                InitEntity(data);

                await _base_ParameterBus.AddDataAsync(data);
            }
            else
            {
                await _base_ParameterBus.UpdateDataAsync(data);
            }
        }

        [HttpPost]
        public async Task DeleteData(List<string> ids)
        {
            await _base_ParameterBus.DeleteDataAsync(ids);
        }

        #endregion
    }
}