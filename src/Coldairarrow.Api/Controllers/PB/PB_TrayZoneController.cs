using Coldairarrow.Business.PB;
using Coldairarrow.Entity.PB;
using Coldairarrow.Util;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Api.Controllers.PB
{
    [Route("/PB/[controller]/[action]")]
    public class PB_TrayZoneController : BaseApiController
    {
        #region DI

        public PB_TrayZoneController(IPB_TrayZoneBusiness pB_TrayZoneBus, IPB_TrayBusiness pB_TrayBusiness)
        {
            _pB_TrayZoneBus = pB_TrayZoneBus;
            _pB_TrayBusiness = pB_TrayBusiness;
        }

        IPB_TrayZoneBusiness _pB_TrayZoneBus { get; }
        IPB_TrayBusiness _pB_TrayBusiness { get; }

        #endregion

        #region 获取

        [HttpPost]
        public async Task<PageResult<PB_TrayZone>> GetDataList(PageInput<ConditionDTO> input)
        {
            return await _pB_TrayZoneBus.GetDataListAsync(input);
        }

        [HttpPost]
        public async Task<List<PB_TrayZone>> GetDataListByType(string typeId)
        {
            var res = await _pB_TrayZoneBus.GetDataListAsync(typeId);
            return res;
        }

        [HttpPost]
        public async Task<List<PB_TrayZone>> GetDataListByTray(string trayId)
        {
            var traydata = await _pB_TrayBusiness.GetTheDataAsync(trayId);
            var res = await _pB_TrayZoneBus.GetDataListAsync(traydata.TrayTypeId);
            return res;
        }

        [HttpPost]
        public async Task<PB_TrayZone> GetTheData(IdInputDTO input)
        {
            return await _pB_TrayZoneBus.GetTheDataAsync(input.id);
        }

        #endregion

        #region 提交

        [HttpPost]
        public async Task SaveData(PB_TrayZone data)
        {
            if (data.Id.IsNullOrEmpty())
            {
                InitEntity(data);

                await _pB_TrayZoneBus.AddDataAsync(data);
            }
            else
            {
                await _pB_TrayZoneBus.UpdateDataAsync(data);
            }
        }

        [HttpPost]
        public async Task DeleteData(List<string> ids)
        {
            await _pB_TrayZoneBus.DeleteDataAsync(ids);
        }

        #endregion
    }
}