using Coldairarrow.Business.IT;
using Coldairarrow.Entity.IT;
using Coldairarrow.IBusiness.DTO;
using Coldairarrow.Util;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Coldairarrow.IBusiness;

namespace Coldairarrow.Api.Controllers.IT
{
    [Route("/IT/[controller]/[action]")]
    public class IT_LocalMaterialController : BaseApiController
    {
        #region DI

        public IT_LocalMaterialController(IIT_LocalMaterialBusiness iT_LocalMaterialBus, IOperator op, IServiceProvider provider)
        {
            _iT_LocalMaterialBus = iT_LocalMaterialBus;
            _Op = op;
            _provider = provider;
        }

        IIT_LocalMaterialBusiness _iT_LocalMaterialBus { get; }
        IOperator _Op { get; }
        IServiceProvider _provider { get; }

        #endregion

        #region 获取

        [HttpPost]
        public async Task<PageResult<IT_LocalMaterial>> GetDataList(IT_LocalMaterialPageInput input)
        {
            input.StorId = _Op.Property.DefaultStorageId;
            return await _iT_LocalMaterialBus.GetDataListAsync(input);
        }

        [HttpPost]
        public async Task<PageResult<IT_LocalMaterial>> GetDataListByMaterialId(PageInput<ConditionDTO> input)
        {
            input.Search.Condition =  _Op.Property.DefaultStorageId;
            return await _iT_LocalMaterialBus.GetDataListByMaterialId(input);
        }

        [HttpPost]
        public async Task<IT_LocalMaterial> GetTheData(IdInputDTO input)
        {
            return await _iT_LocalMaterialBus.GetTheDataAsync(input.id);
        }


        [HttpPost]
        public async Task<IT_LocalMaterial> GetTheLocalMaterial(BusinessInfo businessInfo)
        {
            businessInfo.StorId = _Op.Property.DefaultStorageId;
            return await _iT_LocalMaterialBus.GetDataByBussiness(businessInfo);
        }

        [HttpPost]
        public async Task<List<IT_LocalMaterial>> GetQueryData(SelectQueryDTO search)
        {
            return await _iT_LocalMaterialBus.GetQueryData(search, _Op.Property.DefaultStorageId);
        }

        #endregion

        #region 提交

        [HttpPost]
        public async Task SaveData(IT_LocalMaterial data)
        {
            if (data.Id.IsNullOrEmpty())
            {
                InitEntity(data);

                await _iT_LocalMaterialBus.AddDataAsync(data);
            }
            else
            {
                await _iT_LocalMaterialBus.UpdateDataAsync(data);
            }
        }

        [HttpPost]
        public async Task DeleteData(List<string> ids)
        {
            await _iT_LocalMaterialBus.DeleteDataAsync(ids);
        }

        #endregion
    }
}