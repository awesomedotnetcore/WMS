using Coldairarrow.Business.IT;
using Coldairarrow.Entity.IT;
using Coldairarrow.Util;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Coldairarrow.IBusiness;

namespace Coldairarrow.Api.Controllers.IT
{
    [Route("/IT/[controller]/[action]")]
    public class IT_LocalDetailController : BaseApiController
    {
        #region DI

        public IT_LocalDetailController(IIT_LocalDetailBusiness iT_LocalDetailBus,IOperator op)
        {
            _iT_LocalDetailBus = iT_LocalDetailBus;
            _Op = op;
        }
        IOperator _Op { get; }
        IIT_LocalDetailBusiness _iT_LocalDetailBus { get; }

        #endregion

        #region 获取

        [HttpPost]
        public async Task<PageResult<IT_LocalDetail>> GetDataList(IT_LocalDetailPageInput input)
        {
            input.StorId = _Op.Property.DefaultStorageId;
            return await _iT_LocalDetailBus.GetDataListAsync(input);
        }

        [HttpPost]
        public async Task<IT_LocalDetail> GetTheData(IdInputDTO input)
        {
            return await _iT_LocalDetailBus.GetTheDataAsync(input.id);
        }

        #endregion

        #region 提交

        [HttpPost]
        public async Task SaveData(IT_LocalDetail data)
        {
            if (data.Id.IsNullOrEmpty())
            {
                InitEntity(data);

                await _iT_LocalDetailBus.AddDataAsync(data);
            }
            else
            {
                await _iT_LocalDetailBus.UpdateDataAsync(data);
            }
        }

        [HttpPost]
        public async Task DeleteData(List<string> ids)
        {
            await _iT_LocalDetailBus.DeleteDataAsync(ids);
        }

        #endregion
    }
}