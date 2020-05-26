using Coldairarrow.Business.PB;
using Coldairarrow.Entity.PB;
using Coldairarrow.Util;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Api.Controllers.PB
{
    [Route("/PB/[controller]/[action]")]
    public class PB_BarCodeTypeController : BaseApiController
    {
        #region DI

        public PB_BarCodeTypeController(IPB_BarCodeTypeBusiness pB_BarCodeTypeBus)
        {
            _pB_BarCodeTypeBus = pB_BarCodeTypeBus;
        }

        IPB_BarCodeTypeBusiness _pB_BarCodeTypeBus { get; }

        #endregion

        #region 获取

        [HttpPost]
        public async Task<PageResult<PB_BarCodeType>> GetDataList(PageInput<PB_BarCodeTypeQM> input)
        {
            return await _pB_BarCodeTypeBus.GetDataListAsync(input);
        }

        [HttpPost]
        public async Task<PB_BarCodeType> GetTheData(IdInputDTO input)
        {
            return await _pB_BarCodeTypeBus.GetTheDataAsync(input.id);
        }

        [HttpGet]
        public async Task<List<PB_BarCodeType>> GetAllData()
        {
            return await _pB_BarCodeTypeBus.GetAllData();
        }

        [HttpPost]
        public async Task<string> Generate(string code, Dictionary<string, string> para)
        {
            return await _pB_BarCodeTypeBus.Generate(code, para);
        }

        #endregion

        #region 提交

        [HttpPost]
        public async Task SaveData(PB_BarCodeType data)
        {
            if (data.Id.IsNullOrEmpty())
            {
                InitEntity(data);

                await _pB_BarCodeTypeBus.AddDataAsync(data);
            }
            else
            {
                await _pB_BarCodeTypeBus.UpdateDataAsync(data);
            }
        }

        [HttpPost]
        public async Task DeleteData(List<string> ids)
        {
            await _pB_BarCodeTypeBus.DeleteDataAsync(ids);
        }

        #endregion
    }
}