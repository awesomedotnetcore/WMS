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
using Coldairarrow.Business.PD;

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
        public async Task<AjaxResult<(string Id, string LocalId)>> AutoInByTary(ProduceInStorageByTary data)
        {
            var traySvc = this.serviceProvider.GetRequiredService<IPB_TrayBusiness>();
            var tray = await traySvc.GetByCode(data.TaryCode);
            //var planSvc = this.serviceProvider.GetRequiredService<IPD_PlanBusiness>();
            //var plan = await planSvc.GetByCode(data.PlanCode);
            var materialSvc= this.serviceProvider.GetRequiredService<IPB_MaterialBusiness>();
            var material = await materialSvc.GetByCode(data.MaterialCode);
            var StorId = _Op.Property.DefaultStorageId;

            if (tray == null || material == null) return new AjaxResult<(string Id, string LocalId)>() { Success = false, Msg = "托盘或计划输入不正确" };
            (string StorId, string MaterialId, string TaryId) para = (StorId, material.Id, tray.Id);
            var localId = await _tD_InStorageBus.ReqLocation(para);
            if (localId.IsNullOrEmpty()) return new AjaxResult<(string Id, string LocalId)>() { Success = false, Msg = "没有可以入库的货位" };

            var entity = new TD_InStorage()
            {
                InStorTime = DateTime.Now,
                InType = "Product",
                //RefCode = data.PlanCode,
                Status = 0,
                StorId = StorId,
                //SupId = data.SupId,
                InStorDetails = new List<TD_InStorDetail>() {
                    new TD_InStorDetail(){
                        StorId=StorId,
                        LocalId=localId,
                        TrayId=tray.Id,
                        MaterialId=material.Id,
                        BatchNo=data.BatchNo,
                        Num=data.Num
                    }
                }
            };

            InitEntity(entity);
            foreach (var item in entity.InStorDetails)
            {
                InitEntity(item);
                item.InStorId = entity.Id;
                item.TotalAmt = item.Price * item.Num;
            }
            await _tD_InStorageBus.AddDataAsync(entity);

            return Success<(string Id, string LocalId)>((entity.Id, localId));
        }

        /// <summary>
        /// 生产入库完成
        /// </summary>
        /// <param name="id">自动入库ID</param>
        /// <returns></returns>
        [HttpPost]
        public async Task ComplatedInByTray(string id)
        {
            var audit = new AuditDTO();
            audit.Id = id;
            audit.StorId = _Op.Property.DefaultStorageId;
            audit.AuditUserId = _Op.UserId;
            audit.AuditTime = DateTime.Now;
            audit.AuditType = AuditType.Approve;
            await _tD_InStorageBus.Approve(audit);
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