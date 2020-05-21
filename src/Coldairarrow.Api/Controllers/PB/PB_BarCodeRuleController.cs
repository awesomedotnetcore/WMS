using Coldairarrow.Business.PB;
using Coldairarrow.Entity.PB;
using Coldairarrow.Util;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Api.Controllers.PB
{
    [Route("/PB/[controller]/[action]")]
    public class PB_BarCodeRuleController : BaseApiController
    {
        #region DI

        public PB_BarCodeRuleController(IPB_BarCodeRuleBusiness pB_BarCodeRuleBus)
        {
            _pB_BarCodeRuleBus = pB_BarCodeRuleBus;
        }

        IPB_BarCodeRuleBusiness _pB_BarCodeRuleBus { get; }

        #endregion

        #region 获取

        [HttpPost]
        public async Task<PageResult<PB_BarCodeRule>> GetDataList(PB_BarCodeRulePageInput input)
        {
            return await _pB_BarCodeRuleBus.GetDataListAsync(input);
        }

        [HttpPost]
        public async Task<PB_BarCodeRule> GetTheData(IdInputDTO input)
        {
            return await _pB_BarCodeRuleBus.GetTheDataAsync(input.id);
        }

        #endregion

        #region 提交

        [HttpPost]
        public async Task SaveData(PB_BarCodeRule data)
        {
            if (data.Id.IsNullOrEmpty())
            {
                InitEntity(data);

                await _pB_BarCodeRuleBus.AddDataAsync(data);
            }
            else
            {
                await _pB_BarCodeRuleBus.UpdateDataAsync(data);
            }
        }

        [HttpPost]
        public async Task DeleteData(List<string> ids)
        {
            await _pB_BarCodeRuleBus.DeleteDataAsync(ids);
        }

        #endregion
    }
}