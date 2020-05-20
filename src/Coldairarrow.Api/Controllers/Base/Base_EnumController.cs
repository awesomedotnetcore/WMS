using Coldairarrow.Business.Base;
using Coldairarrow.Entity.Base;
using Coldairarrow.Util;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Api.Controllers.Base
{
    [Route("/Base/[controller]/[action]")]
    public class Base_EnumController : BaseApiController
    {
        #region DI

        public Base_EnumController(IBase_EnumBusiness base_EnumBus)
        {
            _base_EnumBus = base_EnumBus;
        }

        IBase_EnumBusiness _base_EnumBus { get; }

        #endregion

        #region 获取

        [HttpPost]
        public async Task<PageResult<Base_Enum>> GetDataList(PageInput<Base_EnumQM> input)
        {
            return await _base_EnumBus.GetDataListAsync(input);
        }

        [HttpPost]
        public async Task<Base_Enum> GetTheData(IdInputDTO input)
        {
            return await _base_EnumBus.GetTheDataAsync(input.id);
        }
        [HttpGet]
        public async Task<Base_Enum> GetByCode(string code)
        {
            return await _base_EnumBus.GetByCodeAsync(code);
        }
        #endregion

        #region 提交

        [HttpPost]
        public async Task SaveData(Base_Enum data)
        {
            data.IsSystem = false;
            if (data.Id.IsNullOrEmpty())
            {
                InitEntity(data);

                await _base_EnumBus.AddDataAsync(data);
            }
            else
            {
                await _base_EnumBus.UpdateDataAsync(data);
            }
        }

        [HttpPost]
        public async Task DeleteData(List<string> ids)
        {
            await _base_EnumBus.DeleteDataAsync(ids);
        }

        #endregion
    }
}