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
    public class TD_AllocateController : BaseApiController
    {
        #region DI

        public TD_AllocateController(ITD_AllocateBusiness tD_AllocateBus, IOperator op)
        {
            _tD_AllocateBus = tD_AllocateBus;
            _Op = op;
        }

        IOperator _Op { get; }
        ITD_AllocateBusiness _tD_AllocateBus { get; }

        #endregion

        #region 获取

        [HttpPost]
        public async Task<PageResult<TD_Allocate>> GetDataList(TD_AllocatePageInput input)
        {
            input.StorId = _Op.Property.DefaultStorageId;
            return await _tD_AllocateBus.GetDataListAsync(input);
        }

        [HttpPost]
        public async Task<TD_Allocate> GetTheData(IdInputDTO input)
        {
            return await _tD_AllocateBus.GetTheDataAsync(input.id);
        }

        #endregion

        #region 提交

        [HttpPost]
        public async Task SaveData(TD_Allocate data)
        {
            if (data.Id.IsNullOrEmpty())
            {
                InitEntity(data);
                data.StorId = _Op.Property.DefaultStorageId;
                foreach (var item in data.AllocateDetails)
                {
                    InitEntity(item);
                    item.AllocateId = data.Id;
                    item.FromStorId = data.StorId;
                    item.Amount = item.Price * item.AllocateNum;
                }
                await _tD_AllocateBus.AddDataAsync(data);
            }
            else
            {
                foreach (var item in data.AllocateDetails)
                {
                    if (item.Id.StartsWith("newid_"))
                        InitEntity(item);
                    item.AllocateId = data.Id;
                    item.FromStorId = data.StorId;
                    item.Amount = item.Price * item.AllocateNum;
                }
                await _tD_AllocateBus.UpdateDataAsync(data);
            }
        }

        [HttpPost]
        public async Task DeleteData(List<string> ids)
        {
            await _tD_AllocateBus.DeleteDataAsync(ids);
        }

        [HttpPost]
        public async Task Audit(AuditDTO audit)
        {
            audit.StorId = _Op.Property.DefaultStorageId;
            audit.AuditUserId = _Op.UserId;
            audit.AuditTime = DateTime.Now;
            if (audit.AuditType == AuditType.Approve)
                await _tD_AllocateBus.Approve(audit);
            else
                await _tD_AllocateBus.Reject(audit);
        }
        #endregion
    }
}