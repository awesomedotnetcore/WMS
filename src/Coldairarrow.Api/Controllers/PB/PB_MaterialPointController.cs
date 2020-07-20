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
    public class PB_MaterialPointController : BaseApiController
    {
        #region DI

        public PB_MaterialPointController(IPB_MaterialPointBusiness pB_MaterialPointBus)
        {
            _pB_MaterialPointBus = pB_MaterialPointBus;
        }

        IPB_MaterialPointBusiness _pB_MaterialPointBus { get; }

        #endregion

        #region 获取

        [HttpPost]
        public async Task<PageResult<PB_MaterialPoint>> GetDataList(PB_MaterialPointPageInput input)
        {
            return await _pB_MaterialPointBus.GetDataListAsync(input);
        }

        [HttpPost]
        public async Task<PB_MaterialPoint> GetTheData(IdInputDTO input)
        {
            return await _pB_MaterialPointBus.GetTheDataAsync(input.id);
        }

        [HttpPost]
        public async Task<List<PB_MaterialPoint>> GetDataListByPointId(string PointId)
        {
            return await _pB_MaterialPointBus.GetDataListAsync(PointId);
        }

        #endregion

        #region 提交

        [HttpPost]
        public async Task SaveData(PB_MaterialPoint data)
        {
            //if (data.Id.IsNullOrEmpty())
            //{
            //    InitEntity(data);

            //    await _pB_MaterialPointBus.AddDataAsync(data);
            //}
            //else
            //{
            //    await _pB_MaterialPointBus.UpdateDataAsync(data);
            //}
            await _pB_MaterialPointBus.AddDataAsync(data);
        }

        [HttpPost]
        public async Task SaveDatas(PBMaterialPointConditionDTO data)
        {
            await _pB_MaterialPointBus.AddDataAsync(data);
        }

        [HttpPost]
        public async Task DeleteData(string PointId, List<string> materialIds)
        {
            await _pB_MaterialPointBus.DeleteDataAsync(PointId, materialIds);
        }

        #endregion
    }
}