using Coldairarrow.Business.Base;
using Coldairarrow.Entity.Base;
using Coldairarrow.Util;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Api.Controllers.Base
{
    [Route("/Base/[controller]/[action]")]
    public class Base_UserStorController : BaseApiController
    {
        #region DI

        public Base_UserStorController(IBase_UserStorBusiness base_UserStorBus)
        {
            _base_UserStorBus = base_UserStorBus;
        }

        IBase_UserStorBusiness _base_UserStorBus { get; }

        #endregion

        #region 获取

        [HttpPost]
        public async Task<PageResult<Base_UserStor>> GetDataList(PageInput<Base_UserStorQM> input)
        {
            return await _base_UserStorBus.GetDataListAsync(input);
        }

        [HttpPost]
        public async Task<Base_UserStor> GetTheData(IdInputDTO input)
        {
            return await _base_UserStorBus.GetTheDataAsync(input.id);
        }

        [HttpGet]
        public async Task<List<Entity.PB.PB_Storage>> GetStorage()
        {
            return await _base_UserStorBus.GetStorage();
        }
        #endregion

        #region 提交

        [HttpPost]
        public async Task SaveData(Base_UserStor data)
        {
            if (data.Id.IsNullOrEmpty())
            {
                InitEntity(data);

                await _base_UserStorBus.AddDataAsync(data);
            }
            else
            {
                await _base_UserStorBus.UpdateDataAsync(data);
            }
        }

        [HttpPost]
        public async Task DeleteData(List<string> ids)
        {
            await _base_UserStorBus.DeleteDataAsync(ids);
        }

        #endregion
    }
}