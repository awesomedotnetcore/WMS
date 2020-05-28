using Coldairarrow.Business.PB;
using Coldairarrow.Entity.PB;
using Coldairarrow.IBusiness.DTO;
using Coldairarrow.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Quartz.Util;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Api.Controllers.PB
{
    [Route("/PB/[controller]/[action]")]
    public class PB_LocationController : BaseApiController
    {
        #region DI

        public PB_LocationController(IPB_LocationBusiness pB_LocationBus,IOperator op, IServiceProvider provider)
        {
            _pB_LocationBus = pB_LocationBus;
            _Op = op;
            _provider = provider;
        }
        IOperator _Op { get; }
        IPB_LocationBusiness _pB_LocationBus { get; }
        IServiceProvider _provider { get; }

        #endregion

        #region 获取

        [HttpPost]
        public async Task<PageResult<PB_Location>> GetDataList(PageInput<ConditionDTO> input)
        {
            return await _pB_LocationBus.GetDataListAsync(input);
        }

        [HttpPost]
        public async Task<List<PB_Location>> GetAllData()
        {
            return await _pB_LocationBus.GetDataListAsync();
        }

        [HttpPost]
        public async Task<PB_Location> GetTheData(IdInputDTO input)
        {
            return await _pB_LocationBus.GetTheDataAsync(input.id);
        }

        [HttpPost]
        public async Task<List<PB_Location>> GetQueryData(SelectQueryDTO search)
        {
            return await _pB_LocationBus.GetQueryData(search, _Op.Property.DefaultStorageId);
        }
        #endregion

        #region 提交

        [HttpPost]
        public async Task SaveData(PB_Location data)
        {
            if (data.Id.IsNullOrEmpty())
            {
                InitEntity(data);
                if (data.Code.IsNullOrWhiteSpace())
                {
                    data.Code = await _provider.GetRequiredService<IPB_BarCodeTypeBusiness>().Generate("PB_Location");
                }

                await _pB_LocationBus.AddDataAsync(data);
            }
            else
            {
                await _pB_LocationBus.UpdateDataAsync(data);
            }
        }

        [HttpPost]
        public async Task DeleteData(List<string> ids)
        {
            await _pB_LocationBus.DeleteDataAsync(ids);
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public async Task ModifyEnable(string id)
        //{
        //    await _pB_LocationBus.ModifyEnableAsync(id);
        //}

        #endregion
    }
}