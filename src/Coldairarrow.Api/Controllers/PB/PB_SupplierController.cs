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
    public partial class PB_SupplierController : BaseApiController
    {
        #region DI

        public PB_SupplierController(IPB_SupplierBusiness pB_SupplierBus, IServiceProvider provider)
        {
            _pB_SupplierBus = pB_SupplierBus;
            _provider = provider;
        }

        IPB_SupplierBusiness _pB_SupplierBus { get; }

        IServiceProvider _provider { get; }

        #endregion

        #region 获取

        [HttpPost]
        public async Task<PageResult<PB_Supplier>> GetDataList(PageInput<ConditionDTO> input)
        {
            return await _pB_SupplierBus.GetDataListAsync(input);
        }

        [HttpPost]
        public async Task<PB_Supplier> GetTheData(IdInputDTO input)
        {
            return await _pB_SupplierBus.GetTheDataAsync(input.id);
        }

        #endregion

        #region 提交

        [HttpPost]
        public async Task SaveData(PB_Supplier data)
        {
            if (data.Id.IsNullOrEmpty())
            {
                InitEntity(data);
                if (data.Code.IsNullOrWhiteSpace())
                {
                    data.Code = await _provider.GetRequiredService<IPB_BarCodeTypeBusiness>().Generate("PB_Supplier");
                }

                await _pB_SupplierBus.AddDataAsync(data);
            }
            else
            {
                await _pB_SupplierBus.UpdateDataAsync(data);
            }
        }

        [HttpPost]
        public async Task DeleteData(List<string> ids)
        {
            await _pB_SupplierBus.DeleteDataAsync(ids);
        }

        #endregion
    }
}