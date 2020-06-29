using Coldairarrow.Business.TD;
using Coldairarrow.Entity.TD;
using Coldairarrow.IBusiness.DTO;
using Coldairarrow.Util;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Coldairarrow.IBusiness;

namespace Coldairarrow.Api.Controllers.TD
{
    [Route("/TD/[controller]/[action]")]
    public class TD_BadController : BaseApiController
    {
        #region DI

        public TD_BadController(ITD_BadBusiness tD_BadBus, IOperator op)
        {
            _tD_BadBus = tD_BadBus;
            _Op = op;
        }

        IOperator _Op { get; }
        ITD_BadBusiness _tD_BadBus { get; }

        #endregion

        #region 获取

        [HttpPost]
        public async Task<PageResult<TD_Bad>> GetDataList(TD_BadPageInput input)
        {
            input.StorId = _Op.Property.DefaultStorageId;
            return await _tD_BadBus.GetDataListAsync(input);
        }

        [HttpPost]
        public async Task<TD_Bad> GetTheData(IdInputDTO input)
        {
            return await _tD_BadBus.GetTheDataAsync(input.id);
        }

        #endregion

        #region 提交

        [HttpPost]
        public async Task SaveData(TD_Bad data)
        {
            if (data.Id.IsNullOrEmpty())
            {
                InitEntity(data);
                data.StorId = _Op.Property.DefaultStorageId;
                foreach (var item in data.BadDetails)
                {
                    InitEntity(item);
                    item.BadId = data.Id;
                    item.StorId = data.StorId;
                    item.Amount = (item.Price * item.BadNum * (100 - item.Surplus)) / 100;
                }
                await _tD_BadBus.AddDataAsync(data);
            }
            else
            {
                foreach (var item in data.BadDetails)
                {
                    if (item.Id.StartsWith("newid_"))
                        InitEntity(item);
                    item.BadId = data.Id;
                    item.StorId = data.StorId;
                    item.Amount = (item.Price * item.BadNum * (100 - item.Surplus)) / 100;
                }
                await _tD_BadBus.UpdateDataAsync(data);
            }
        }

        [HttpPost]
        public async Task DeleteData(List<string> ids)
        {
            await _tD_BadBus.DeleteDataAsync(ids);
        }

        [HttpPost]
        public async Task Audit(AuditDTO audit)
        {
            audit.StorId = _Op.Property.DefaultStorageId;
            audit.AuditUserId = _Op.UserId;
            audit.AuditTime = DateTime.Now;
            if (audit.AuditType == AuditType.Approve)
                await _tD_BadBus.Approve(audit);
            else
                await _tD_BadBus.Reject(audit);
        }
        #endregion
    }
}