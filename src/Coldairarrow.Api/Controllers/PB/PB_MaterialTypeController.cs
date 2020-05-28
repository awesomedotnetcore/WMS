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
    public partial class PB_MaterialTypeController : BaseApiController
    {
        #region DI

        public PB_MaterialTypeController(IPB_MaterialTypeBusiness pB_MaterialTypeBus, IServiceProvider provider)
        {
            _pB_MaterialTypeBus = pB_MaterialTypeBus;
            _provider = provider;
        }

        IPB_MaterialTypeBusiness _pB_MaterialTypeBus { get; }

        IServiceProvider _provider { get; }

        #endregion

        #region 获取

        [HttpPost]
        public async Task<PageResult<PB_MaterialType>> GetDataList(PageInput<ConditionDTO> input)
        {
            return await _pB_MaterialTypeBus.GetDataListAsync(input);
        }

        [HttpPost]
        public async Task<PB_MaterialType> GetTheData(IdInputDTO input)
        {
            return await _pB_MaterialTypeBus.GetTheDataAsync(input.id);
        }

        #endregion

        #region 提交

        [HttpPost]
        public async Task SaveData(PB_MaterialType data)
        {
            if (data.Id.IsNullOrEmpty())
            {
                InitEntity(data);
                if (data.Code.IsNullOrWhiteSpace())
                {
                    data.Code = await _provider.GetRequiredService<IPB_BarCodeTypeBusiness>().Generate("PB_MaterialType");
                }

                await _pB_MaterialTypeBus.AddDataAsync(data);
            }
            else
            {
                await _pB_MaterialTypeBus.UpdateDataAsync(data);
            }
        }

        [HttpPost]
        public async Task DeleteData(List<string> ids)
        {
            await _pB_MaterialTypeBus.DeleteDataAsync(ids);
        }

        #endregion
    }
}