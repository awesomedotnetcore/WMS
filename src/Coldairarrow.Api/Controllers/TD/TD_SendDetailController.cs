using Coldairarrow.Business.TD;
using Coldairarrow.Entity.TD;
using Coldairarrow.IBusiness;
using Coldairarrow.Util;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Api.Controllers.TD
{
    [Route("/TD/[controller]/[action]")]
    public class TD_SendDetailController : BaseApiController
    {
        #region DI

        public TD_SendDetailController(ITD_SendDetailBusiness tD_SendDetailBus, IOperator @op)
        {
            _tD_SendDetailBus = tD_SendDetailBus;
            _Op = @op;
        }

        IOperator _Op { get; }

        ITD_SendDetailBusiness _tD_SendDetailBus { get; }

        #endregion

        #region 获取

        [HttpPost]
        public async Task<PageResult<TD_SendDetail>> GetDataList(TD_SendDetailPageInput input)
        {
            input.StorId = _Op.Property.DefaultStorageId;
            return await _tD_SendDetailBus.GetDataListAsync(input);
        }

        [HttpPost]
        public async Task<TD_SendDetail> GetTheData(IdInputDTO input)
        {
            return await _tD_SendDetailBus.GetTheDataAsync(input.id);
        }

        #endregion

        #region 提交

        [HttpPost]
        public async Task SaveData(TD_SendDetail data)
        {
            if (data.Id.IsNullOrEmpty())
            {
                InitEntity(data);

                await _tD_SendDetailBus.AddDataAsync(data);
            }
            else
            {
                await _tD_SendDetailBus.UpdateDataAsync(data);
            }
        }

        [HttpPost]
        public async Task DeleteData(List<string> ids)
        {
            await _tD_SendDetailBus.DeleteDataAsync(ids);
        }

        #endregion
    }
}