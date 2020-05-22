using Coldairarrow.Business.PB;
using Coldairarrow.Entity.PB;
using Coldairarrow.Util;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Api.Controllers.PB
{
    [Route("/PB/[controller]/[action]")]
    public class PB_EquipmentController : BaseApiController
    {
        #region DI

        public PB_EquipmentController(IPB_EquipmentBusiness pB_EquipmentBus)
        {
            _pB_EquipmentBus = pB_EquipmentBus;
        }

        IPB_EquipmentBusiness _pB_EquipmentBus { get; }

        #endregion

        #region 获取

        [HttpPost]
        public async Task<PageResult<PB_Equipment>> GetDataList(PageInput<ConditionDTO> input)
        {
            return await _pB_EquipmentBus.GetDataListAsync(input);
        }

        [HttpPost]
        public async Task<PB_Equipment> GetTheData(IdInputDTO input)
        {
            return await _pB_EquipmentBus.GetTheDataAsync(input.id);
        }

        #endregion

        #region 提交

        [HttpPost]
        public async Task SaveData(PB_Equipment data)
        {
            if (data.Id.IsNullOrEmpty())
            {
                InitEntity(data);

                await _pB_EquipmentBus.AddDataAsync(data);
            }
            else
            {
                await _pB_EquipmentBus.UpdateDataAsync(data);
            }
        }

        [HttpPost]
        public async Task DeleteData(List<string> ids)
        {
            await _pB_EquipmentBus.DeleteDataAsync(ids);
        }

        #endregion
    }
}