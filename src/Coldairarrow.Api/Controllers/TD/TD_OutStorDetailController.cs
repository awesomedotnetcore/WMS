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
    public class TD_OutStorDetailController : BaseApiController
    {
        #region DI

        public TD_OutStorDetailController(ITD_OutStorDetailBusiness tD_OutStorDetailBus,IOperator @op)
        {
            _tD_OutStorDetailBus = tD_OutStorDetailBus;
            _Op = @op;
        }
        IOperator _Op { get; }
        ITD_OutStorDetailBusiness _tD_OutStorDetailBus { get; }

        #endregion

        #region 获取

        [HttpPost]
        public async Task<PageResult<TD_OutStorDetail>> GetDataList(TD_OutStorDetailPageInput input)
        {
            input.StorId = _Op.Property.DefaultStorageId;
            return await _tD_OutStorDetailBus.GetDataListAsync(input);
        }

        [HttpPost]
        public async Task<TD_OutStorDetail> GetTheData(IdInputDTO input)
        {
            return await _tD_OutStorDetailBus.GetTheDataAsync(input.id);
        }

        #endregion

        #region 提交

        [HttpPost]
        public async Task SaveData(TD_OutStorDetail data)
        {
            if (data.Id.IsNullOrEmpty())
            {
                InitEntity(data);

                await _tD_OutStorDetailBus.AddDataAsync(data);
            }
            else
            {
                await _tD_OutStorDetailBus.UpdateDataAsync(data);
            }
        }

        [HttpPost]
        public async Task DeleteData(List<string> ids)
        {
            await _tD_OutStorDetailBus.DeleteDataAsync(ids);
        }

        #endregion
    }
}