using Coldairarrow.Business.TD;
using Coldairarrow.Entity.TD;
using Coldairarrow.IBusiness.DTO;
using Coldairarrow.Util;
using Elasticsearch.Net;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Coldairarrow.IBusiness;
using Microsoft.Extensions.DependencyInjection;
using Coldairarrow.Business.PB;

namespace Coldairarrow.Api.Controllers.TD
{
    [Route("/TD/[controller]/[action]")]
    public class TD_InStorageController : BaseApiController
    {
        #region DI

        public TD_InStorageController(ITD_InStorageBusiness tD_InStorageBus, IOperator op, IServiceProvider svcProvider)
        {
            _tD_InStorageBus = tD_InStorageBus;
            _Op = op;
            this.serviceProvider = svcProvider;
        }
        IServiceProvider serviceProvider { get; }
        ITD_InStorageBusiness _tD_InStorageBus { get; }
        IOperator _Op { get; }

        #endregion

        #region 获取

        [HttpPost]
        public async Task<PageResult<TD_InStorage>> GetDataList(TD_InStoragePageInput input)
        {
            input.StorId = _Op.Property.DefaultStorageId;
            return await _tD_InStorageBus.GetDataListAsync(input);
        }

        [HttpPost]
        public async Task<TD_InStorage> GetTheData(IdInputDTO input)
        {
            return await _tD_InStorageBus.GetTheDataAsync(input.id);
        }

        #endregion

        #region 提交

        [HttpPost]
        public async Task SaveData(TD_InStorage data)
        {
            if (data.Id.IsNullOrEmpty())
            {
                InitEntity(data);
                data.StorId = _Op.Property.DefaultStorageId;
                foreach (var item in data.InStorDetails)
                {
                    InitEntity(item);
                    item.InStorId = data.Id;
                    item.StorId = data.StorId;
                    item.TotalAmt = item.Price * item.Num;
                }
                await _tD_InStorageBus.AddDataAsync(data);
            }
            else
            {
                foreach (var item in data.InStorDetails)
                {
                    if (item.Id.StartsWith("newid_"))
                        InitEntity(item);
                    item.InStorId = data.Id;
                    item.StorId = data.StorId;
                    item.TotalAmt = item.Price * item.Num;
                }
                await _tD_InStorageBus.UpdateDataAsync(data);
            }
        }

        /// <summary>
        /// 生产自动入库
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<AjaxResult> AutoInByTary(ProduceInStorageByTary data)
        {
            var tarySvc = this.serviceProvider.GetRequiredService<IPB_TrayBusiness>();
            var tary = await tarySvc.GetByCode(data.TaryCode);
            return Success();
        }

        [HttpPost]
        public async Task DeleteData(List<string> ids)
        {
            await _tD_InStorageBus.DeleteDataAsync(ids);
        }

        [HttpPost]
        public async Task Audit(AuditDTO audit)
        { 
            audit.StorId = _Op.Property.DefaultStorageId;
            audit.AuditUserId = _Op.UserId;
            audit.AuditTime = DateTime.Now;
            if (audit.AuditType == AuditType.Approve)
                await _tD_InStorageBus.Approve(audit);
            else
                await _tD_InStorageBus.Reject(audit);
        }

        [HttpPost]
        public async Task InBlankTray(List<KeyValuePair<string, string>> listTray)
        {
            await _tD_InStorageBus.InBlankTray(listTray);
        }
        #endregion
    }
}