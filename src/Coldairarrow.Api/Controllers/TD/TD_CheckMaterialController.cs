using Coldairarrow.Business.TD;
using Coldairarrow.Entity.TD;
using Coldairarrow.Util;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Api.Controllers.TD
{
    [Route("/TD/[controller]/[action]")]
    public partial class TD_CheckMaterialController : BaseApiController
    {
        #region DI

        public TD_CheckMaterialController(ITD_CheckMaterialBusiness tD_CheckMaterialBus)
        {
            _tD_CheckMaterialBus = tD_CheckMaterialBus;
        }

        ITD_CheckMaterialBusiness _tD_CheckMaterialBus { get; }

        #endregion

        #region 获取

        [HttpPost]
        public async Task<PageResult<TD_CheckMaterial>> GetDataList(PageInput<ConditionDTO> input)
        {
            return await _tD_CheckMaterialBus.GetDataListAsync(input);
        }

        [HttpPost]
        public async Task<TD_CheckMaterial> GetTheData(IdInputDTO input)
        {
            return await _tD_CheckMaterialBus.GetTheDataAsync(input.id);
        }

        #endregion

        #region 提交

        [HttpPost]
        public async Task SaveData(TD_CheckMaterial data)
        {
            await _tD_CheckMaterialBus.UpdateDataAsync(data);
        }

        [HttpPost]
        public async Task DeleteData(List<string> ids)
        {
            await _tD_CheckMaterialBus.DeleteDataAsync(ids);
        }

        #endregion
    }
}