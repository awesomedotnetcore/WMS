using Coldairarrow.Business.IT;
using Coldairarrow.Business.PB;
using Coldairarrow.Business.TD;
using Coldairarrow.Entity.TD;
using Coldairarrow.IBusiness.DTO;
using Coldairarrow.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Coldairarrow.IBusiness;

namespace Coldairarrow.Api.Controllers.TD
{
    [Route("/TD/[controller]/[action]")]
    public class TD_MoveController : BaseApiController
    {
        #region DI

        public TD_MoveController(ITD_MoveBusiness tD_MoveBus, IOperator op)
        {
            _tD_MoveBus = tD_MoveBus;
            _Op = op;
        }

        IOperator _Op { get; }
        ITD_MoveBusiness _tD_MoveBus { get; }

        #endregion

        #region 获取

        [HttpPost]
        public async Task<PageResult<TD_Move>> GetDataList(TD_MovePageInput input)
        {
            input.StorId = _Op.Property.DefaultStorageId;
            return await _tD_MoveBus.GetDataListAsync(input);
        }

        [HttpPost]
        public async Task<TD_Move> GetTheData(IdInputDTO input)
        {
            return await _tD_MoveBus.GetTheDataAsync(input.id);
        }

        #endregion

        #region 提交

        [HttpPost]
        public async Task SaveData(TD_Move data)
        {
            if (data.Id.IsNullOrEmpty())
            {
                InitEntity(data);
                data.StorId = _Op.Property.DefaultStorageId;
                foreach (var item in data.MoveDetails)
                {
                    InitEntity(item);
                    item.MoveId = data.Id;
                    item.StorId = data.StorId;
                    item.Amount = item.Price * item.MoveNum;
                }
                await _tD_MoveBus.AddDataAsync(data);
            }
            else
            {
                foreach (var item in data.MoveDetails)
                {
                    if (item.Id.StartsWith("newid_"))
                        InitEntity(item);
                    item.MoveId = data.Id;
                    item.StorId = data.StorId;
                    item.Amount = item.Price * item.MoveNum;
                }
                await _tD_MoveBus.UpdateDataAsync(data);
            }
        }

        [HttpPost]
        public async Task DeleteData(List<string> ids)
        {
            await _tD_MoveBus.DeleteDataAsync(ids);
        }

        [HttpPost]
        public async Task Audit(AuditDTO audit)
        {
            audit.StorId = _Op.Property.DefaultStorageId;
            audit.AuditUserId = _Op.UserId;
            audit.AuditTime = DateTime.Now;
            if (audit.AuditType == AuditType.Approve)
                await _tD_MoveBus.Approve(audit);
            else
                await _tD_MoveBus.Reject(audit);
        }
        #endregion


    }
}