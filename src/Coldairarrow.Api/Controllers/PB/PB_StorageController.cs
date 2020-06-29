using Coldairarrow.Business.PB;
using Coldairarrow.Entity.PB;
using Coldairarrow.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Quartz.Util;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using Coldairarrow.IBusiness;

namespace Coldairarrow.Api.Controllers.PB
{
    [Route("/PB/[controller]/[action]")]
    public class PB_StorageController : BaseApiController
    {
        #region DI

        public PB_StorageController(IPB_StorageBusiness PB_StorageBus,IOperator op, IServiceProvider provider)
        {
            _PB_StorageBus = PB_StorageBus;
            _Op = op;
            _provider = provider;
        }

        IOperator _Op { get; }
        IPB_StorageBusiness _PB_StorageBus { get; }
        IServiceProvider _provider { get; }

        #endregion

        #region 获取

        [HttpPost]
        public async Task<PageResult<PB_Storage>> GetDataList(PB_StoragePageInput input)
        {
            return await _PB_StorageBus.GetDataListAsync(input);
        }

        [HttpPost]
        public async Task<PB_Storage> GetTheData(IdInputDTO input)
        {
            return await _PB_StorageBus.GetTheDataAsync(input.id);
        }

        /// <summary>
        /// 获取当前用户默认的仓库
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<PB_Storage> GetCurStorage()
        {
            return await _PB_StorageBus.GetTheDataAsync(_Op.Property.DefaultStorageId);
        }

        #endregion

        #region 提交

        [HttpPost]
        public async Task SaveData(PB_Storage data)
        {
            if (data.Id.IsNullOrEmpty())
            {
                InitEntity(data);
                if (data.Code.IsNullOrWhiteSpace())
                {
                    data.Code = await _provider.GetRequiredService<IPB_BarCodeTypeBusiness>().Generate("PB_Storage");
                }

                await _PB_StorageBus.AddDataAsync(data);
            }
            else
            {
                await _PB_StorageBus.UpdateDataAsync(data);
            }
        }

        /// <summary>
        /// 更新默认仓库
        /// </summary>
        [HttpPost]
        public async Task SaveDataDefault(PB_Storage data)
        {
            await _PB_StorageBus.ModifyDefaultAsync(data);
        }

        [HttpPost]
        public async Task DeleteData(List<string> ids)
        {
            await _PB_StorageBus.DeleteDataAsync(ids);
        }

        /// <summary>
        /// 查询仓库ID
        /// </summary>
        [HttpPost]
        public async Task<List<PB_Storage>> QueryStorageData()
        {
            return await _PB_StorageBus.QueryStorageDataAsync();
        }

        /// <summary>
        /// 更新托盘管理状态
        /// </summary>
        [HttpPost]
        public async Task ModifyIsTray(string id)
        {
            await _PB_StorageBus.ModifyIsTrayAsync(id);
        }

        /// <summary>
        /// 更新托盘分区状态
        /// </summary>
        [HttpPost]
        public async Task ModifyIsZone(string id)
        {
            await _PB_StorageBus.ModifyIsZoneAsync(id);
        }

        /// <summary>
        /// 更新仓库是否启用
        /// </summary>
        [HttpPost]
        public async Task ModifyDisable(string id)
        {
            await _PB_StorageBus.ModifyDisableAsync(id);
        }

        #endregion
    }
}