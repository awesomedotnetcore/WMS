using Coldairarrow.Business.IT;
using Coldairarrow.Entity.IT;
using Coldairarrow.Util;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Api.Controllers.IT
{
    [Route("/IT/[controller]/[action]")]
    public class IT_RecordBookController : BaseApiController
    {
        #region DI

        public IT_RecordBookController(IIT_RecordBookBusiness iT_RecordBookBus)
        {
            _iT_RecordBookBus = iT_RecordBookBus;
        }

        IIT_RecordBookBusiness _iT_RecordBookBus { get; }

        #endregion

        #region 获取

        [HttpPost]
        public async Task<PageResult<IT_RecordBook>> GetDataList(IT_RecordBookPageInput input)
        {
            return await _iT_RecordBookBus.GetDataListAsync(input);
        }

        [HttpPost]
        public async Task<IT_RecordBook> GetTheData(IdInputDTO input)
        {
            return await _iT_RecordBookBus.GetTheDataAsync(input.id);
        }

        #endregion

        #region 提交

        [HttpPost]
        public async Task SaveData(IT_RecordBook data)
        {
            if (data.Id.IsNullOrEmpty())
            {
                InitEntity(data);

                await _iT_RecordBookBus.AddDataAsync(data);
            }
            else
            {
                await _iT_RecordBookBus.UpdateDataAsync(data);
            }
        }

        [HttpPost]
        public async Task DeleteData(List<string> ids)
        {
            await _iT_RecordBookBus.DeleteDataAsync(ids);
        }

        #endregion
    }
}