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
    public class PB_LanewayController : BaseApiController
    {
        #region DI

        public PB_LanewayController(IPB_LanewayBusiness pB_LanewayBus, IServiceProvider provider)
        {
            _pB_LanewayBus = pB_LanewayBus;
            _provider = provider;
        }

        IPB_LanewayBusiness _pB_LanewayBus { get; }

        IServiceProvider _provider { get; }

        #endregion

        #region 获取

        [HttpPost]
        public async Task<PageResult<PB_Laneway>> GetDataList(PB_LanewayPageInput input)
        {
            return await _pB_LanewayBus.GetDataListAsync(input);
        }

        [HttpPost]
        public async Task<PB_Laneway> GetTheData(IdInputDTO input)
        {
            return await _pB_LanewayBus.GetTheDataAsync(input.id);
        }

        /// <summary>
        /// 查询巷道
        /// </summary>
        [HttpPost]
        public async Task<List<PB_Laneway>> QueryLanewayData()
        {
            return await _pB_LanewayBus.QueryLanewayDataAsync();
        }

        /// <summary>
        /// 查询巷道所属仓库
        /// </summary>
        [HttpPost]
        public async Task<List<PB_Laneway>> GetDataListByStor(string storId)
        {
            var res = await _pB_LanewayBus.GetDataListAsync(storId);
            return res;
        }

        #endregion

        #region 提交

        [HttpPost]
        public async Task SaveData(PB_Laneway data)
        {
            if (data.Id.IsNullOrEmpty())
            {
                InitEntity(data);
                if (data.Code.IsNullOrWhiteSpace())
                {
                    data.Code = await _provider.GetRequiredService<IPB_BarCodeTypeBusiness>().Generate("PB_Laneway");
                }

                await _pB_LanewayBus.AddDataAsync(data);
            }
            else
            {
                await _pB_LanewayBus.UpdateDataAsync(data);
            }
        }

        [HttpPost]
        public async Task DeleteData(List<string> ids)
        {
            await _pB_LanewayBus.DeleteDataAsync(ids);
        }

        #endregion
    }
}