using Coldairarrow.Business.PB;
using Coldairarrow.Entity.PB;
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
    public class PB_RackController : BaseApiController
    {
        #region DI

        public PB_RackController(IPB_RackBusiness pB_RackBus, IServiceProvider provider)
        {
            _pB_RackBus = pB_RackBus;
            _provider = provider;
        }

        IPB_RackBusiness _pB_RackBus { get; }

        IServiceProvider _provider { get; }

        #endregion

        #region 获取

        [HttpPost]
        public async Task<PageResult<PB_Rack>> GetDataList(PB_RackPageInput input)
        {
            return await _pB_RackBus.GetDataListAsync(input);
        }

        [HttpPost]
        public async Task<PB_Rack> GetTheData(IdInputDTO input)
        {
            return await _pB_RackBus.GetTheDataAsync(input.id);
        }
        /// <summary>
        /// 查询货架
        /// </summary>
        [HttpPost]
        public async Task<List<PB_Rack>> QueryRackData()
        {
            return await _pB_RackBus.QueryRackDataAsync();
        }

        /// <summary>
        /// 查询货架所属仓库
        /// </summary>
        [HttpPost]
        public async Task<List<PB_Rack>> GetDataListByStor(string storId)
        {
            var res = await _pB_RackBus.GetDataListAsync(storId);
            return res;
        }
        #endregion

        #region 提交

        [HttpPost]
        public async Task SaveData(PB_Rack data)
        {
            if (data.Id.IsNullOrEmpty())
            {
                InitEntity(data);
                if (data.Code.IsNullOrWhiteSpace())
                {
                    data.Code = await _provider.GetRequiredService<IPB_BarCodeTypeBusiness>().Generate("PB_Rack");
                }

                await _pB_RackBus.AddDataAsync(data);
            }
            else
            {
                await _pB_RackBus.UpdateDataAsync(data);
            }
        }

        [HttpPost]
        public async Task DeleteData(List<string> ids)
        {
            await _pB_RackBus.DeleteDataAsync(ids);
        }

        #endregion
    }
}