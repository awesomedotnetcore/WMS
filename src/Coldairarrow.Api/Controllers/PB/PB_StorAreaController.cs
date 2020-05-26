using Coldairarrow.Business.Base;
using Coldairarrow.Business.PB;
using Coldairarrow.Entity.PB;
using Coldairarrow.Util;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Api.Controllers.PB
{
    [Route("/PB/[controller]/[action]")]
    public partial class PB_StorAreaController : BaseApiController
    {
        #region DI
        IBase_UserStorBusiness _base_UserStorBus { get; }
        IOperator _Op { get; }

        public PB_StorAreaController(IPB_StorAreaBusiness pB_StorAreaBus, IBase_UserStorBusiness base_UserStorBus, IOperator op)
        {
            _pB_StorAreaBus = pB_StorAreaBus;
            _base_UserStorBus = base_UserStorBus;
            _Op = op;
        }

        IPB_StorAreaBusiness _pB_StorAreaBus { get; }

        #endregion

        #region 获取

        [HttpPost]
        public async Task<PageResult<PB_StorArea>> GetDataList(PageInput<ConditionDTO> input)
        {
            return await _pB_StorAreaBus.GetDataListAsync(input);
        }

        [HttpPost]
        public async Task<PB_StorArea> GetTheData(IdInputDTO input)
        {
            return await _pB_StorAreaBus.GetTheDataAsync(input.id);
        }

        /// <summary>
        /// 查询货区
        /// </summary>
        [HttpPost]
        public async Task<List<PB_StorArea>> QueryStorAreaData()
        {
            return await _pB_StorAreaBus.QueryStorAreaDataAsync();
        }

        #endregion

        #region 提交

        [HttpPost]
        public async Task SaveData(PB_StorArea data)
        {
            if (data.Id.IsNullOrEmpty())
            {
                InitEntity(data);

                await _pB_StorAreaBus.AddDataAsync(data);
            }
            else
            {
                await _pB_StorAreaBus.UpdateDataAsync(data);
            }
        }

        [HttpPost]
        public async Task DeleteData(List<string> ids)
        {
            await _pB_StorAreaBus.DeleteDataAsync(ids);
        }

        

        #endregion
    }
}