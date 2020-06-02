using Coldairarrow.Business.PB;
using Coldairarrow.Business.TD;
using Coldairarrow.Entity.TD;
using Coldairarrow.IBusiness.DTO;
using Coldairarrow.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Quartz.Util;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Api.Controllers.TD
{
    [Route("/TD/[controller]/[action]")]
    public class TD_OutStorageController : BaseApiController
    {
        #region DI

        public TD_OutStorageController(ITD_OutStorageBusiness tD_OutStorageBus, IServiceProvider provider, IOperator op)
        {
            _tD_OutStorageBus = tD_OutStorageBus;
            _provider = provider;
            _Op = op;
        }

        ITD_OutStorageBusiness _tD_OutStorageBus { get; }

        IServiceProvider _provider { get; }

        IOperator _Op { get; }

        #endregion

        #region 获取

        [HttpPost]
        public async Task<PageResult<TD_OutStorage>> GetDataList(TD_OutStoragePageInput input)
        {
            input.StorId = _Op.Property.DefaultStorageId;
            return await _tD_OutStorageBus.GetDataListAsync(input);
        }

        [HttpPost]
        public async Task<TD_OutStorage> GetTheData(IdInputDTO input)
        {
            return await _tD_OutStorageBus.GetTheDataAsync(input.id);
        }

        #endregion

        #region 提交

        [HttpPost]
        public async Task SaveData(TD_OutStorage data)
        {
            if (data.Id.IsNullOrEmpty())
            {
                InitEntity(data);
                data.StorageId = _Op.Property.DefaultStorageId;
                foreach (var item in data.OutStorDetails)
                {
                    InitEntity(item);
                    item.OutStorId = data.Id;
                    item.StorId = data.StorageId;
                    item.TotalAmt = item.Price * item.LocalNum;
                }
                await _tD_OutStorageBus.AddDataAsync(data);
            }
            else
            {
                foreach (var item in data.OutStorDetails)
                {
                    if (item.Id.StartsWith("newid_"))
                        InitEntity(item);
                    item.OutStorId = data.Id;
                    item.StorId = data.StorageId;
                    item.TotalAmt = item.Price * item.LocalNum ;
                }
                await _tD_OutStorageBus.UpdateDataAsync(data);
            }
        }

        [HttpPost]
        public async Task DeleteData(List<string> ids)
        {
            await _tD_OutStorageBus.DeleteDataAsync(ids);
        }

        [HttpPost]
        public async Task Audit(AuditDTO audit)
        {
            audit.StorId = _Op.Property.DefaultStorageId;
            audit.AuditUserId = _Op.UserId;
            audit.AuditTime = DateTime.Now;
            if (audit.AuditType == AuditType.Approve)
                await _tD_OutStorageBus.Approve(audit);
            else
                await _tD_OutStorageBus.Reject(audit);
        }

        #endregion
    }
}