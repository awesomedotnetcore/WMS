using Coldairarrow.Business.PB;
using Coldairarrow.Entity.PB;
using Coldairarrow.Util;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Api.Controllers.PB
{
    [Route("/PB/[controller]/[action]")]
    public partial class PB_AddressController : BaseApiController
    {
        #region DI

        public PB_AddressController(IPB_AddressBusiness pB_AddressBus)
        {
            _pB_AddressBus = pB_AddressBus;
        }

        IPB_AddressBusiness _pB_AddressBus { get; }

        #endregion

        #region 获取

        [HttpPost]
        public async Task<PageResult<PB_Address>> GetDataList(PageInput<ConditionDTO> input)
        {
            return await _pB_AddressBus.GetDataListAsync(input);
        }

        [HttpPost]
        public async Task<PB_Address> GetTheData(IdInputDTO input)
        {
            return await _pB_AddressBus.GetTheDataAsync(input.id);
        }

        #endregion

        #region 提交

        [HttpPost]
        public async Task SaveData(PB_Address data)
        {
            if (data.Id.IsNullOrEmpty())
            {
                InitEntity(data);
                if(string.IsNullOrWhiteSpace(data.SupId)) data.SupId = null;
                else if (string.IsNullOrWhiteSpace(data.CusId)) data.CusId = null;


                await _pB_AddressBus.AddDataAsync(data);
            }
            else
            {
                await _pB_AddressBus.UpdateDataAsync(data);
            }
        }

        [HttpPost]
        public async Task DeleteData(List<string> ids)
        {
            await _pB_AddressBus.DeleteDataAsync(ids);
        }

        #endregion
    }
}