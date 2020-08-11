using Coldairarrow.Business.PB;
using Coldairarrow.Business.PD;
using Coldairarrow.Entity.PD;
using Coldairarrow.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Quartz.Util;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Api.Controllers.PD
{
    [Route("/PD/[controller]/[action]")]
    public partial class PD_PlanController : BaseApiController
    {
        #region DI

        public PD_PlanController(IPD_PlanBusiness pD_PlanBus, IServiceProvider provider)
        {
            _pD_PlanBus = pD_PlanBus;
            _provider = provider;
        }

        IPD_PlanBusiness _pD_PlanBus { get; }

        IServiceProvider _provider { get; }

        #endregion

        #region 获取

        [HttpPost]
        public async Task<PageResult<PD_Plan>> GetDataList(PageInput<PD_PlanQM> input)
        {
            return await _pD_PlanBus.GetDataListAsync(input);
        }

        [HttpPost]
        public async Task<PD_Plan> GetTheData(IdInputDTO input)
        {
            return await _pD_PlanBus.GetTheDataAsync(input.id);
        }

        #endregion

        #region 提交

        [HttpPost]
        public async Task SaveData(PD_Plan data)
        {
            if (data.Id.IsNullOrEmpty())
            {
                InitEntity(data);
                if (data.Code.IsNullOrWhiteSpace())
                {
                    data.Code = await _provider.GetRequiredService<IPB_BarCodeTypeBusiness>().Generate("PD_Plan");
                }
                await _pD_PlanBus.AddDataAsync(data);
            }
            else
            {
                await _pD_PlanBus.UpdateDataAsync(data);
            }
        }

        [HttpPost]
        public async Task DeleteData(List<string> ids)
        {
            await _pD_PlanBus.DeleteDataAsync(ids);
        }

        [HttpPost]
        public async Task Status(string id, int status)
        {
            await _pD_PlanBus.Status(id, status);
        }
        #endregion
    }
}