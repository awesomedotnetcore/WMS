using Coldairarrow.Business.Base;
using Coldairarrow.Business.TD;
using Coldairarrow.Entity.TD;
using Coldairarrow.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Coldairarrow.IBusiness;

namespace Coldairarrow.Api.Controllers.TD
{
    [Route("/TD/[controller]/[action]")]
    public partial class TD_CheckController : BaseApiController
    {
        #region 获取

        [HttpPost]
        public async Task<PageResult<TD_Check>> GetDataList(PageInput<ConditionDTO> input)
        {
            return await _tD_CheckBus.GetDataListAsync(input);
        }

        [HttpPost]
        public async Task<TD_Check> GetTheData(IdInputDTO input)
        {
            return await _tD_CheckBus.GetTheDataAsync(input.id);
        }

        #endregion

        #region 提交

        [HttpPost]
        public async Task SaveData(TD_Check data)
        {
            if (data.Id.IsNullOrEmpty())
            {
                InitEntity(data);
                data.StorId = _provider.GetRequiredService<IOperator>().Property.DefaultStorageId;
                data.EquId = "1";
                data.IsComplete = false;
                data.Status = 0;

                await _tD_CheckBus.AddDataAsync(data);
            }
            else
            {
                await _tD_CheckBus.UpdateDataAsync(data);
            }
        }

        [HttpPost]
        public async Task DeleteData(List<string> ids)
        {
            await _tD_CheckBus.DeleteDataAsync(ids);
        }

        #endregion
    }
}