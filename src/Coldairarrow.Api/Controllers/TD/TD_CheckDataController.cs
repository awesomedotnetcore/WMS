using Coldairarrow.Business.TD;
using Coldairarrow.Entity.TD;
using Coldairarrow.Util;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Api.Controllers.TD
{
    [Route("/TD/[controller]/[action]")]
    public partial class TD_CheckDataController : BaseApiController
    {
        #region DI

        public TD_CheckDataController(ITD_CheckDataBusiness tD_CheckDataBus, IServiceProvider provider)
        {
            _tD_CheckDataBus = tD_CheckDataBus;
            _provider = provider;
        }

        ITD_CheckDataBusiness _tD_CheckDataBus { get; }

        IServiceProvider _provider { get; }

        #endregion

        #region 获取

        [HttpPost]
        public async Task<PageResult<TD_CheckData>> GetDataList(PageInput<ConditionDTO> input)
        {
            return await _tD_CheckDataBus.GetDataListAsync(input);
        }

        [HttpPost]
        public async Task<TD_CheckData> GetTheData(IdInputDTO input)
        {
            return await _tD_CheckDataBus.GetTheDataAsync(input.id);
        }

        #endregion

        #region 提交

        [HttpPost]
        public async Task SaveData(TD_CheckData data)
        {
            if (data.Id.IsNullOrEmpty())
            {
                InitEntity(data);

                await _tD_CheckDataBus.AddDataAsync(data);
            }
            else
            {
                await _tD_CheckDataBus.UpdateDataAsync(data);
            }
        }

        [HttpPost]
        public async Task DeleteData(List<string> ids)
        {
            await _tD_CheckDataBus.DeleteDataAsync(ids);
        }

        #endregion
    }
}