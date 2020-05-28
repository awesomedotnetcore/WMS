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
    public partial class PB_MaterialController : BaseApiController
    {
        #region DI

        public PB_MaterialController(IPB_MaterialBusiness pB_MaterialBus,IServiceProvider provider)
        {
            _pB_MaterialBus = pB_MaterialBus;
            _provider = provider;
        }

        IPB_MaterialBusiness _pB_MaterialBus { get; }

        IServiceProvider _provider { get; }

        #endregion

        #region 获取

        [HttpPost]
        public async Task<PageResult<PB_Material>> GetDataList(PageInput<ConditionDTO> input)
        {
            return await _pB_MaterialBus.GetDataListAsync(input);
        }


        [HttpPost]
        public async Task<List<PB_Material>> GetAllDataList()
        {
            return await _pB_MaterialBus.GetDataListAsync();
        }


        [HttpPost]
        public async Task<PB_Material> GetTheData(IdInputDTO input)
        {
            return await _pB_MaterialBus.GetTheDataAsync(input.id);
        }

        [HttpPost]
        public async Task<List<PB_Material>> GetQueryData(SelectQueryDTO search)
        {
            return await _pB_MaterialBus.GetQueryData(search);
        }

        #endregion

        #region 提交

        [HttpPost]
        public async Task SaveData(PB_Material data)
        {
            if (data.Id.IsNullOrEmpty())
            {
                InitEntity(data);
                if (data.Code.IsNullOrWhiteSpace())
                {
                    data.Code = await _provider.GetRequiredService<IPB_BarCodeTypeBusiness>().Generate("PB_Material");
                }

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