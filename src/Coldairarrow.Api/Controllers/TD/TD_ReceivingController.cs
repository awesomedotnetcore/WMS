using Coldairarrow.Business.TD;
using Coldairarrow.Entity.TD;
using Coldairarrow.IBusiness;
using Coldairarrow.IBusiness.DTO;
using Coldairarrow.Util;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Api.Controllers.TD
{
    [Route("/TD/[controller]/[action]")]
    public class TD_ReceivingController : BaseApiController
    {
        #region DI

        public TD_ReceivingController(ITD_ReceivingBusiness tD_ReceivingBus, IOperator op)
        {
            _tD_ReceivingBus = tD_ReceivingBus;
            _Op = op;
        }

        ITD_ReceivingBusiness _tD_ReceivingBus { get; }
        IOperator _Op { get; }

        #endregion

        #region 获取

        [HttpPost]
        public async Task<PageResult<TD_Receiving>> GetDataList(TD_ReceivingPageInput input)
        {
            input.StorId = _Op.Property.DefaultStorageId;
            return await _tD_ReceivingBus.GetDataListAsync(input);
        }

        [HttpPost]
        public async Task<TD_Receiving> GetTheData(IdInputDTO input)
        {
            return await _tD_ReceivingBus.GetTheDataAsync(input.id);
        }

        #endregion

        #region 提交

        [HttpPost]
        public async Task SaveData(TD_Receiving data)
        {
            if (data.Id.IsNullOrEmpty())
            {
                InitEntity(data);
                data.StorId = _Op.Property.DefaultStorageId;
                foreach (var item in data.RecDetails)
                {
                    InitEntity(item);
                    item.RecId = data.Id;
                    item.StorId = data.StorId;
                    item.Amount = item.Price * item.PlanNum;
                }
                await _tD_ReceivingBus.AddDataAsync(data);
            }
            else
            {
                foreach (var item in data.RecDetails)
                {
                    if (item.Id.StartsWith("newid_"))
                        InitEntity(item);
                    item.RecId = data.Id;
                    item.StorId = data.StorId;
                    item.Amount = item.Price * item.PlanNum;
                }
                await _tD_ReceivingBus.UpdateDataAsync(data);
            }
        }

        [HttpPost]
        public async Task DeleteData(List<string> ids)
        {
            await _tD_ReceivingBus.DeleteDataAsync(ids);
        }
        [HttpPost]
        public async Task Approval(AuditDTO audit)
        {
            audit.StorId = _Op.Property.DefaultStorageId;
            audit.AuditUserId = _Op.UserId;
            audit.AuditTime = DateTime.Now;
            await _tD_ReceivingBus.Approval(audit);
        }
        #endregion
    }
}