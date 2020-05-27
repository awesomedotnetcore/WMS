using Coldairarrow.Business.PB;
using Coldairarrow.Entity.PB;
using Coldairarrow.IBusiness.DTO;
using Coldairarrow.Util;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Api.Controllers.PB
{
    [Route("/PB/[controller]/[action]")]
    public class PB_TrayController : BaseApiController
    {
        #region DI

        public PB_TrayController(IPB_TrayBusiness pB_TrayBus)
        {
            _pB_TrayBus = pB_TrayBus;
        }

        IPB_TrayBusiness _pB_TrayBus { get; }

        #endregion

        #region 获取

        [HttpPost]
        public async Task<PageResult<PB_Tray>> GetDataList(PageInput<ConditionDTO> input)
        {
            return await _pB_TrayBus.GetDataListAsync(input);
        }

        [HttpPost]
        public async Task<PB_Tray> GetTheData(IdInputDTO input)
        {
            return await _pB_TrayBus.GetTheDataAsync(input.id);
        }


        [HttpPost]
        public async Task EnableTheData(IdInputDTO input)
        {
            await _pB_TrayBus.EnableTheData(input.id);
        }


        [HttpPost]
        public async Task DisableTheData(IdInputDTO input)
        {
            await _pB_TrayBus.DisableTheData(input.id);
        }

        [HttpPost]
        public async Task<List<PB_Tray>> GetQueryData(TraySelectQueryDTO search)
        {
            return await _pB_TrayBus.GetQueryData(search);
        }
        #endregion

        #region 提交

        [HttpPost]
        public async Task SaveData(PB_Tray data)
        {
            if (data.Id.IsNullOrEmpty())
            {
                InitEntity(data);

                await _pB_TrayBus.AddDataAsync(data);
            }
            else
            {
                await _pB_TrayBus.UpdateDataAsync(data);
            }
        }

        [HttpPost]
        public async Task DeleteData(List<string> ids)
        {
            await _pB_TrayBus.DeleteDataAsync(ids);
        }

        #endregion
    }
}