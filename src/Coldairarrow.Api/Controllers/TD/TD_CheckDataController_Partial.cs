using Coldairarrow.Business.TD;
using Coldairarrow.Entity.TD;
using Coldairarrow.IBusiness.DTO;
using Coldairarrow.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Api.Controllers.TD
{
    public partial class TD_CheckDataController 
    {
        [HttpPost]
        public async Task<PageResult<TDCheckDataDTO>> QueryDataList(PageInput<TDCheckDataConditionDTO> input)
        {
            return await _tD_CheckDataBus.QueryDataListAsync(input);
        }

        [HttpPost]
        public async Task ModifyCheckNum(TDCheckNumModifyDTO data)
        {
            await _tD_CheckDataBus.ModifyCheckNumAsync(_provider.GetRequiredService<IOperator>().UserId, data);
        }
    }
}