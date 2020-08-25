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
using Coldairarrow.IBusiness;
using Coldairarrow.Business.IT;
using Coldairarrow.Entity.PB;

namespace Coldairarrow.Api.Controllers.TD
{
    [Route("/TD/[controller]/[action]")]
    public class TD_OutStorageController : BaseApiController
    {
        #region DI

        public TD_OutStorageController(ITD_OutStorageBusiness tD_OutStorageBus, IServiceProvider provider, IOperator op, IServiceProvider svcProvider)
        {
            _tD_OutStorageBus = tD_OutStorageBus;
            _provider = provider;
            _Op = op;
            this.serviceProvider = svcProvider;
        }

        ITD_OutStorageBusiness _tD_OutStorageBus { get; }

        IServiceProvider _provider { get; }

        IOperator _Op { get; }

        IServiceProvider serviceProvider { get; }

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
                    item.TotalAmt = item.Price * item.LocalNum;
                }
                await _tD_OutStorageBus.UpdateDataAsync(data);
            }
        }

        [HttpPost]
        public async Task DeleteData(List<string> ids)
        {
            await _tD_OutStorageBus.DeleteDataAsync(ids);
        }

        /// <summary>
        /// 出库审核
        /// </summary>
        /// <param name="audit"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 空托盘出库
        /// </summary>
        /// <param name="listTray"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<AjaxResult> OutBlankTray(List<KeyValuePair<string, string>> listTray)
        {
            return await _tD_OutStorageBus.OutBlankTray(listTray, _Op.Property.DefaultStorageId);
        }

        /// <summary>
        /// 自动出库
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<AjaxResult<TD_OutStorage>> AutoOut(AutoOutStorageQM data)
        {
            var materialSvc = this._provider.GetRequiredService<IPB_MaterialBusiness>();
            var material = await materialSvc.GetByBarcode(data.MaterialCode);
            if (material == null) return new AjaxResult<TD_OutStorage>() { Success = false, Msg = "物料编码不正确" };
            var StorId = _Op.Property.DefaultStorageId;
            var reqData = new ReqMaterialQM() { StorId = StorId, MaterialId = material.Id, BatchNo = data.BatchNo, Num = data.Num };
            var listOut = await this._tD_OutStorageBus.ReqMaterial(reqData);
            if (!listOut.Success) return new AjaxResult<TD_OutStorage>() { Success = false, Msg = listOut.Msg };

            var entity = new TD_OutStorage()
            {
                SendId = data.SendId,
                StorageId = StorId,
                OutTime = DateTime.Now,
                OutType = "ProductOut",
                OutNum = data.Num,
                Status = 0,
                OutStorDetails = new List<TD_OutStorDetail>()
            };
            InitEntity(entity);
            foreach (var detail in listOut.Data)
            {
                var item = new TD_OutStorDetail()
                {
                    StorId = StorId,
                    OutStorId = entity.Id,
                    LocalId = detail.LocalId,
                    TrayId = detail.TrayId,
                    MaterialId = detail.MaterialId,
                    BatchNo = detail.BatchNo,
                    LocalNum = detail.LocalNum,
                    OutNum = detail.OutNum,
                    Price = material.Price.GetValueOrDefault(0),
                    TotalAmt = material.Price.GetValueOrDefault(0) * detail.OutNum
                };
                InitEntity(item);
                entity.OutStorDetails.Add(item);
            }
            await _tD_OutStorageBus.AddDataAsync(entity);
            return new AjaxResult<TD_OutStorage>() { Success = true, Msg = "出库成功", Data = entity };
        }

        /// <summary>
        /// 手动出库
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<AjaxResult<TD_OutStorage>> ManualOut(ManualOutStorageQM data)
        {
            var materialSvc = this._provider.GetRequiredService<IPB_MaterialBusiness>();
            var material = await materialSvc.GetByBarcode(data.MaterialCode);
            if (material == null) return new AjaxResult<TD_OutStorage>() { Success = false, Msg = "物料编码不正确" };
            var StorId = _Op.Property.DefaultStorageId;

            var entity = new TD_OutStorage()
            {
                SendId = data.SendId,
                StorageId = StorId,
                OutTime = DateTime.Now,
                OutType = "ProductOut",
                OutNum = data.Num,
                Status = 0,
                OutStorDetails = new List<TD_OutStorDetail>()
            };
            InitEntity(entity);

            var localSvc = this._provider.GetRequiredService<IPB_LocationBusiness>();
            var local = await localSvc.GetByCode(StorId, data.LocalCode);
            if (local == null) return new AjaxResult<TD_OutStorage>() { Success = false, Msg = "货位编码不正确" };

            string trayId = null;
            if (!data.TrayCode.IsNullOrEmpty())
            {
                var traySvc = this._provider.GetRequiredService<IPB_TrayBusiness>();
                var tray = await traySvc.GetByCode(data.TrayCode);
                if (tray == null) return new AjaxResult<TD_OutStorage>() { Success = false, Msg = "托盘编码不正确" };
                trayId = tray.Id;
            }

            var item = new TD_OutStorDetail()
            {
                StorId = StorId,
                OutStorId = entity.Id,
                LocalId = local.Id,
                TrayId = trayId,
                MaterialId = material.Id,
                BatchNo = data.BatchNo,
                LocalNum = data.Num,
                OutNum = data.Num,
                Price = material.Price.GetValueOrDefault(0),
                TotalAmt = material.Price.GetValueOrDefault(0) * data.Num
            };
            InitEntity(item);
            entity.OutStorDetails.Add(item);

            await _tD_OutStorageBus.AddDataAsync(entity);
            return new AjaxResult<TD_OutStorage>() { Success = true, Msg = "出库成功", Data = entity };
        }
        #endregion
    }
}