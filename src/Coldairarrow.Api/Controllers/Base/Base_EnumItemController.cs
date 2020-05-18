using Coldairarrow.Business.Base;
using Coldairarrow.Entity.Base;
using Coldairarrow.Util;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Api.Controllers.Base
{
    [Route("/Base/[controller]/[action]")]
    public class Base_EnumItemController : BaseApiController
    {
        #region DI

        public Base_EnumItemController(IBase_EnumItemBusiness base_EnumItemBus)
        {
            _base_EnumItemBus = base_EnumItemBus;
        }

        IBase_EnumItemBusiness _base_EnumItemBus { get; }

        #endregion

        #region 获取

        [HttpPost]
        public async Task<PageResult<Base_EnumItem>> GetDataList(PageInput<ConditionDTO> input)
        {
            return await _base_EnumItemBus.GetDataListAsync(input);
        }

        [HttpPost]
        public async Task<Base_EnumItem> GetTheData(IdInputDTO input)
        {
            return await _base_EnumItemBus.GetTheDataAsync(input.id);
        }

        #endregion

        #region 提交

        [HttpPost]
        public async Task SaveData(Base_EnumItem data)
        {
            if (data.Id.IsNullOrEmpty())
            {
                InitEntity(data);

                await _base_EnumItemBus.AddDataAsync(data);
            }
            else
            {
                await _base_EnumItemBus.UpdateDataAsync(data);
            }
        }

        [HttpPost]
        public async Task DeleteData(List<string> ids)
        {
            await _base_EnumItemBus.DeleteDataAsync(ids);
        }

        #endregion
    }
}