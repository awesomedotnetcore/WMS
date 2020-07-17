using Coldairarrow.Business.PB;
using Coldairarrow.Entity.PB;
using Coldairarrow.Util;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Api.Controllers.PB
{
    [Route("/PB/[controller]/[action]")]
    public class PB_FeedPointController : BaseApiController
    {
        #region DI

        public PB_FeedPointController(IPB_FeedPointBusiness pB_FeedPointBus)
        {
            _pB_FeedPointBus = pB_FeedPointBus;
        }

        IPB_FeedPointBusiness _pB_FeedPointBus { get; }

        #endregion

        #region 获取

        [HttpPost]
        public async Task<PageResult<PB_FeedPoint>> GetDataList(PageInput<PB_FeedPointQM> input)
        {
            return await _pB_FeedPointBus.GetDataListAsync(input);
        }

        [HttpPost]
        public async Task<PB_FeedPoint> GetTheData(IdInputDTO input)
        {
            return await _pB_FeedPointBus.GetTheDataAsync(input.id);
        }

        #endregion

        #region 提交

        [HttpPost]
        public async Task SaveData(PB_FeedPoint data)
        {
            if (data.Id.IsNullOrEmpty())
            {
                InitEntity(data);

                await _pB_FeedPointBus.AddDataAsync(data);
            }
            else
            {
                await _pB_FeedPointBus.UpdateDataAsync(data);
            }
        }

        [HttpPost]
        public async Task DeleteData(List<string> ids)
        {
            await _pB_FeedPointBus.DeleteDataAsync(ids);
        }

        #endregion
    }
}