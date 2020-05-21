using Coldairarrow.Business.PB;
using Coldairarrow.Entity.PB;
using Coldairarrow.Util;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Api.Controllers.PB
{
    [Route("/PB/[controller]/[action]")]
    public class PB_MaterialController : BaseApiController
    {
        #region DI

        public PB_MaterialController(IPB_MaterialBusiness pB_MaterialBus)
        {
            _pB_MaterialBus = pB_MaterialBus;
        }

        IPB_MaterialBusiness _pB_MaterialBus { get; }

        #endregion

        #region 获取

        [HttpPost]
        public async Task<PageResult<PB_Material>> GetDataList(PageInput<ConditionDTO> input)
        {
            return await _pB_MaterialBus.GetDataListAsync(input);
        }

        [HttpPost]
        public async Task<PB_Material> GetTheData(IdInputDTO input)
        {
            return await _pB_MaterialBus.GetTheDataAsync(input.id);
        }

        #endregion

        #region 提交

        [HttpPost]
        public async Task SaveData(PB_Material data)
        {
            if (data.Id.IsNullOrEmpty())
            {
                InitEntity(data);

                await _pB_MaterialBus.AddDataAsync(data);
            }
            else
            {
                await _pB_MaterialBus.UpdateDataAsync(data);
            }
        }

        [HttpPost]
        public async Task DeleteData(List<string> ids)
        {
            await _pB_MaterialBus.DeleteDataAsync(ids);
        }

        #endregion
    }
}