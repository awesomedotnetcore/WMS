using Coldairarrow.Business.PB;
using Coldairarrow.Entity.PB;
using Coldairarrow.Util;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Api.Controllers.PB
{
    [Route("/PB/[controller]/[action]")]
    public class PB_TrayTypeController : BaseApiController
    {
        #region DI

        public PB_TrayTypeController(IPB_TrayTypeBusiness pB_TrayTypeBus)
        {
            _pB_TrayTypeBus = pB_TrayTypeBus;
        }

        IPB_TrayTypeBusiness _pB_TrayTypeBus { get; }

        #endregion

        #region 获取

        [HttpPost]
        public async Task<PageResult<PB_TrayType>> GetDataList(PageInput<ConditionDTO> input)
        {
            return await _pB_TrayTypeBus.GetDataListBySearch(input);
        }

        [HttpPost]
        public async Task<PB_TrayType> GetTheData(IdInputDTO input)
        {
            return await _pB_TrayTypeBus.GetTheDataAsync(input.id);
        }

        #endregion

        #region 提交

        [HttpPost]
        public async Task SaveData(PB_TrayType data)
        {
            if (data.Id.IsNullOrEmpty())
            {
                InitEntity(data);

                await _pB_TrayTypeBus.AddDataAsync(data);
            }
            else
            {
                await _pB_TrayTypeBus.UpdateDataAsync(data);
            }
        }

        [HttpPost]
        public async Task DeleteData(List<string> ids)
        {
            await _pB_TrayTypeBus.DeleteDataAsync(ids);
        }

        #endregion
    }
}