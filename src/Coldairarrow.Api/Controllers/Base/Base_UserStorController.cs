using Coldairarrow.Business.Base;
using Coldairarrow.Entity.Base;
using Coldairarrow.IBusiness;
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

        public Base_UserStorController(IBase_UserStorBusiness base_UserStorBus,IOperator @operator)
        {
            _base_UserStorBus = base_UserStorBus;
            _Operator = @operator;
        }

        IBase_UserStorBusiness _base_UserStorBus { get; }
        IOperator _Operator { get; }

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
        public async Task<List<Business.PB.PB_StorageDTO>> GetStorage()
        {
            return await _base_UserStorBus.GetStorage(_Operator.UserId);
        }
        #endregion

        #region 提交

        [HttpPost]
        public async Task SaveData(Base_UserStor data)
        {
            if (data.IsDefault)
                await _base_UserStorBus.UpdateDefault(_Operator.UserId);
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

        [HttpPost]
        public async Task SwitchStorage(IdInputDTO input)
        {
            await _base_UserStorBus.SwitchDefault(_Operator.UserId, input.id);
        }
        #endregion
    }
}