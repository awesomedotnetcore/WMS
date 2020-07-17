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
    public class TD_SendController : BaseApiController
    {
        #region DI

        public TD_SendController(ITD_SendBusiness tD_SendBus, IOperator op)
        {
            _tD_SendBus = tD_SendBus;
            _Op = op;
        }

        ITD_SendBusiness _tD_SendBus { get; }

        IOperator _Op { get; }

        #endregion

        #region 获取

        [HttpPost]
        public async Task<PageResult<TD_Send>> GetDataList(TD_SendPageInput input)
        {
            input.StorId = _Op.Property.DefaultStorageId;
            return await _tD_SendBus.GetDataListAsync(input);
        }

        [HttpPost]
        public async Task<TD_Send> GetTheData(IdInputDTO input)
        {
            return await _tD_SendBus.GetTheDataAsync(input.id);
        }

        #endregion

        #region 提交

        [HttpPost]
        public async Task SaveData(TD_Send data)
        {
            if (data.Id.IsNullOrEmpty())
            {
                InitEntity(data);
                data.StorId = _Op.Property.DefaultStorageId;
                foreach (var item in data.SendDetails)
                {
                    InitEntity(item);
                    item.SendId = data.Id;
                    item.StorId = data.StorId;
                    item.Amount = item.Price * item.LocalNum;
                }
                await _tD_SendBus.AddDataAsync(data);
            }
            else
            {
                foreach (var item in data.SendDetails)
                {
                    if (item.Id.StartsWith("newid_"))
                        InitEntity(item);
                    item.SendId = data.Id;
                    item.StorId = data.StorId;
                    item.Amount = item.Price * item.LocalNum;
                }
                await _tD_SendBus.UpdateDataAsync(data);
            }
        }
        //{
        //    if (data.Id.IsNullOrEmpty())
        //    {
        //        InitEntity(data);

        //        await _tD_SendBus.AddDataAsync(data);
        //    }
        //    else
        //    {
        //        await _tD_SendBus.UpdateDataAsync(data);
        //    }
        //}

        [HttpPost]
        public async Task DeleteData(List<string> ids)
        {
            await _tD_SendBus.DeleteDataAsync(ids);
        }

        [HttpPost]
        public async Task Approval(AuditDTO audit)
        {
            audit.StorId = _Op.Property.DefaultStorageId;
            audit.AuditUserId = _Op.UserId;
            audit.AuditTime = DateTime.Now;
            await _tD_SendBus.Approval(audit);
        }

        #endregion
    }
}