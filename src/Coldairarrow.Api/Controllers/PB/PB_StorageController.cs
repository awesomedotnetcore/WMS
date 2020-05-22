using Coldairarrow.Business.PB;
using Coldairarrow.Entity.PB;
using Coldairarrow.Util;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Api.Controllers.PB
{
    [Route("/PB/[controller]/[action]")]
    public class PB_StorageController : BaseApiController
    {
        #region DI

        public PB_StorageController(IPB_StorageBusiness PB_StorageBus)
        {
            _PB_StorageBus = PB_StorageBus;
        }

        IPB_StorageBusiness _PB_StorageBus { get; }

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

        #endregion

        #region 提交

        [HttpPost]
        public async Task SaveData(PB_Storage data)
        {
            if (data.Id.IsNullOrEmpty())
            {
                InitEntity(data);

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

        #endregion
    }
}