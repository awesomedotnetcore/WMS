using Coldairarrow.Business.Base;
using Coldairarrow.Business.PB;
using Coldairarrow.Entity.PB;
using Coldairarrow.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Quartz.Util;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Coldairarrow.IBusiness;

namespace Coldairarrow.Api.Controllers.PB
{
    [Route("/PB/[controller]/[action]")]
    public partial class PB_StorAreaController : BaseApiController
    {
        #region DI
        IBase_UserStorBusiness _base_UserStorBus { get; }        

        public PB_StorAreaController(IPB_StorAreaBusiness pB_StorAreaBus, IBase_UserStorBusiness base_UserStorBus, IOperator op, IServiceProvider provider, IPB_StorageBusiness pB_StorageBusiness)
        {
            _pB_StorAreaBus = pB_StorAreaBus;
            _base_UserStorBus = base_UserStorBus;
            _Op = op;
            _provider = provider;
            _pB_StorageBusiness = pB_StorageBusiness;
        }

        IPB_StorAreaBusiness _pB_StorAreaBus { get; }

        IServiceProvider _provider { get; }

        IOperator _Op { get; }

        IPB_StorageBusiness _pB_StorageBusiness { get; }

        #endregion

        #region 获取

        [HttpPost]
        public async Task<PageResult<PB_StorArea>> GetDataList(PB_StorAreaPageInput input)
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

        
        //[HttpPost]
        //public async Task<List<PB_StorArea>> GetDataListByType(string typeId)
        //{
        //    var res = await _pB_StorAreaBus.GetDataListAsync(typeId);
        //    return res;
        //}

        /// <summary>
        /// 查询货区所属仓库
        /// </summary>
        [HttpPost]
        public async Task<List<PB_StorArea>> GetDataListByStor(string storId)
        {
            var res = await _pB_StorAreaBus.GetDataListAsync(storId);
            return res;
        }

        #endregion

        #region 提交

        [HttpPost]
        public async Task SaveData(PB_StorArea data)
        {
            if (data.Id.IsNullOrEmpty())
            {
                InitEntity(data);
                if (data.Code.IsNullOrWhiteSpace())
                {
                    data.Code = await _provider.GetRequiredService<IPB_BarCodeTypeBusiness>().Generate("PB_StorArea");
                }

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