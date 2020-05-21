using Coldairarrow.Business.PB;
using Coldairarrow.Entity.PB;
using Coldairarrow.Util;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Api.Controllers.PB
{
    [Route("/PB/[controller]/[action]")]
    public class PB_AreaMaterialController : BaseApiController
    {
        #region DI

        public PB_AreaMaterialController(IPB_AreaMaterialBusiness pB_AreaMaterialBus)
        {
            _pB_AreaMaterialBus = pB_AreaMaterialBus;
        }

        IPB_AreaMaterialBusiness _pB_AreaMaterialBus { get; }

        #endregion

        #region 获取

        [HttpPost]
        public async Task<PageResult<PB_AreaMaterial>> GetDataList(PageInput<ConditionDTO> input)
        {
            return await _pB_AreaMaterialBus.GetDataListAsync(input);
        }

        [HttpPost]
        public async Task<PB_AreaMaterial> GetTheData(IdInputDTO input)
        {
            return await _pB_AreaMaterialBus.GetTheDataAsync(input.id);
        }

        #endregion

        #region 提交

        [HttpPost]
        public async Task SaveData(PB_AreaMaterial data)
        {
            if (data.Id.IsNullOrEmpty())
            {
                InitEntity(data);

                await _pB_AreaMaterialBus.AddDataAsync(data);
            }
            else
            {
                await _pB_AreaMaterialBus.UpdateDataAsync(data);
            }
        }

        [HttpPost]
        public async Task DeleteData(List<string> ids)
        {
            await _pB_AreaMaterialBus.DeleteDataAsync(ids);
        }

        #endregion
    }
}