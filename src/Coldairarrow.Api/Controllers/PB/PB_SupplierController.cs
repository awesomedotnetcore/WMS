using Coldairarrow.Business.PB;
using Coldairarrow.Entity.PB;
using Coldairarrow.Util;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Api.Controllers.PB
{
    [Route("/PB/[controller]/[action]")]
    public class PB_SupplierController : BaseApiController
    {
        #region DI

        public PB_SupplierController(IPB_SupplierBusiness pB_SupplierBus)
        {
            _pB_SupplierBus = pB_SupplierBus;
        }

        IPB_SupplierBusiness _pB_SupplierBus { get; }

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