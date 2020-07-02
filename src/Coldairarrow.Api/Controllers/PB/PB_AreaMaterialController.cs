using Coldairarrow.Business.PB;
using Coldairarrow.Entity.PB;
using Coldairarrow.IBusiness.DTO;
using Coldairarrow.Util;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public async Task<PageResult<PB_AreaMaterial>> GetDataList(PB_AreaMaterialPageInput input)
        {
            return await _pB_AreaMaterialBus.GetDataListAsync(input);
        }

        [HttpPost]
        public async Task<PB_AreaMaterial> GetTheData(IdInputDTO input)
        {
            return await _pB_AreaMaterialBus.GetTheDataAsync(input.id);
        }

        [HttpPost]
        public async Task<List<PB_AreaMaterial>> GetDataListByAreaId(string areaId)
        {
            return await _pB_AreaMaterialBus.GetDataListAsync(areaId);
        }

        #endregion

        #region 提交

        [HttpPost]
        public async Task SaveData(PB_AreaMaterial data)
        {
            //if (data.Id.IsNullOrEmpty())
            //{
            //    InitEntity(data);

            //    await _pB_AreaMaterialBus.AddDataAsync(data);
            //}
            //else
            //{
            //    await _pB_AreaMaterialBus.UpdateDataAsync(data);
            //}
             await _pB_AreaMaterialBus.AddDataAsync(data);
        }

        [HttpPost]
        public async Task SaveDatas(PBAreaMateriaConditionDTO data)
        {
            await _pB_AreaMaterialBus.AddDataAsync(data);
        }

        [HttpPost]
        public async Task DeleteData(string AreaId, List<string> materialIds)
        {
            await _pB_AreaMaterialBus.DeleteDataAsync(AreaId, materialIds);
        }

        #endregion
    }
}