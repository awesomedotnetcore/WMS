using Coldairarrow.Business.Base;
using Coldairarrow.Business.IT;
using Coldairarrow.Business.PB;
using Coldairarrow.Business.TD;
using Coldairarrow.Entity.IT;
using Coldairarrow.Entity.PB;
using Coldairarrow.Entity.TD;
using Coldairarrow.IBusiness.DTO;
using Coldairarrow.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using org.apache.zookeeper.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Coldairarrow.IBusiness;

namespace Coldairarrow.Api.Controllers.TD
{
    public partial class TD_CheckController 
    {
        #region DI
        ITD_CheckBusiness _tD_CheckBus { get; }

        IServiceProvider _provider { get; }

        public TD_CheckController(ITD_CheckBusiness tD_CheckBus, IServiceProvider provider)
        {
            _tD_CheckBus = tD_CheckBus;
            _provider = provider;
        }

        #endregion

        [HttpPost]
        public async Task<PageResult<TD_Check>> QueryDataList(PageInput<TDCheckQueryDTO> input)
        {
            return await _tD_CheckBus.QueryDataListAsync(_provider.GetRequiredService<IOperator>().Property.DefaultStorageId, input);
        }

        [HttpPost]
        public async Task PushData(TDCheckConditionDTO model)
        {
            var _Op = _provider.GetRequiredService<IOperator>();
            var data = model.Data;
            if (data.Id.IsNullOrEmpty())
            {
                InitEntity(data);
                data.StorId = _Op.Property.DefaultStorageId;
                data.EquId = "1";
                data.IsComplete = false;
                data.Status = 0;
                if (string.IsNullOrWhiteSpace(data.Code))
                {
                    data.Code = await _provider.GetRequiredService<IPB_BarCodeTypeBusiness>().Generate("TD_Check");
                }
                await _tD_CheckBus.AddDataAsync(data);
            }
            else
            {
                await _tD_CheckBus.UpdateDataAsync(data);
                await _provider.GetRequiredService<ITD_CheckDataBusiness>().ClearDataAsync(data.Id);
                await _provider.GetRequiredService<ITD_CheckMaterialBusiness>().ClearDataAsync(data.Id);
                await _provider.GetRequiredService<ITD_CheckAreaBusiness>().ClearDataAsync(data.Id);
            }

            if(data.Type== "Storage")
            {
                var arealist= await _provider.GetRequiredService<IPB_StorAreaBusiness>().QueryAsync(_Op.Property.DefaultStorageId);
                await BuildDataByAreaAsync((from u in arealist select u.Id).ToList(), data);
            }
            else if(data.Type=="Area")
            {
                await BuildDataByAreaAsync(model.AreaIdList, data);
            }
            else if(data.Type== "Material" || data.Type == "Random")
            {
                await BuildDataByMaterialAsync(model, data);
            }
        }

        [HttpPost]
        public async Task<AjaxResult> Complete(string Id)
        {
            AjaxResult result = new AjaxResult();
            var completed = await _provider.GetRequiredService<ITD_CheckDataBusiness>().AllCompletedAsync(Id);
            if (completed)
            {
                try
                {
                    var entity = await _tD_CheckBus.GetTheDataAsync(Id);
                    entity.IsComplete = true;
                    entity.Status = 0;
                    entity.CheckTime = DateTime.Now;

                    await _tD_CheckBus.UpdateDataAsync(entity);
                    result.Success = true;
                }
                catch(Exception e)
                {
                    result.Success = false;
                    result.ErrorCode = 502;
                    result.Msg = e.Message;
                }
            }
            else
            {
                result.Success = false;
                result.ErrorCode = 502;
                result.Msg = "存在未盘点项目";
            }

            return result;
        }

        [HttpPost]
        public async Task<AjaxResult> Pass(string Id)
        {
            var _Op = _provider.GetRequiredService<IOperator>();
            var _checkSvc = _provider.GetRequiredService<ITD_CheckDataBusiness>();
            AjaxResult result = new AjaxResult();
            try
            {
                var entity = await _tD_CheckBus.GetTheDataAsync(Id);
                entity.Status = 1;
                entity.AuditeTime = DateTime.Now;
                entity.AuditUserId = _Op.UserId;

                var checkDataList = await _checkSvc.AllCheckDataListAsync(entity.Id);

                //库存更新数据
                var localData = (from u in checkDataList
                                 where u.DisNum.HasValue
                                 select new BusinessInfo()
                                 {
                                     ActionType = 2,
                                     BarCode = u.BarCode,
                                     BatchNo = u.BatchNo,
                                     LocalId = u.localId,
                                     MaterialId = u.MaterialId,
                                     MeasureId = u.MeasureId,
                                     Num = Convert.ToDouble(u.DisNum),
                                     StorId = u.StorId,
                                     ZoneId = u.ZoneId,
                                     TrayId = u.TrayId
                                 }).ToList();

                //台账数据
                var recordData = (from u in checkDataList
                                  where u.DisNum.HasValue
                                  select new IT_RecordBook()
                                  {
                                      Id = IdHelper.GetId(),
                                      BarCode = u.BarCode,
                                      BatchNo = u.BatchNo,
                                      CreateTime = DateTime.Now,
                                      CreatorId = _Op.UserId,
                                      Deleted = false,
                                      FromLocalId = u.localId,
                                      FromStorId = u.StorId,
                                      MaterialId = u.MaterialId,
                                      MeasureId = u.MeasureId,
                                      Num = System.Math.Abs(Convert.ToDouble(u.DisNum)),
                                      RefCode = entity.RefCode,
                                      Type = Convert.ToDouble(u.DisNum) > 0 ? "Panying" : "Deficit"
                                  }).ToList();

                await _tD_CheckBus.PassHandleAsync(entity, localData, recordData);
                result.Success = true;
            }
            catch (Exception e)
            {
                result.Success = false;
                result.ErrorCode = 502;
                result.Msg = e.Message;
            }

            return result;
        }

        [HttpPost]
        public async Task<AjaxResult> Reject(string Id)
        {
            var _Op = _provider.GetRequiredService<IOperator>();
            AjaxResult result = new AjaxResult();
            try
            {
                var entity = await _tD_CheckBus.GetTheDataAsync(Id);
                entity.Status = 2;
                entity.AuditeTime = DateTime.Now;
                entity.AuditUserId = _Op.UserId;

                await _tD_CheckBus.UpdateDataAsync(entity);
                result.Success = true;
            }
            catch (Exception e)
            {
                result.Success = false;
                result.ErrorCode = 502;
                result.Msg = e.Message;
            }

            return result;
        }

        [HttpPost]
        public async Task<AjaxResult> Back(string Id)
        {
            var _Op = _provider.GetRequiredService<IOperator>();
            AjaxResult result = new AjaxResult();
            try
            {
                var entity = await _tD_CheckBus.GetTheDataAsync(Id);
                entity.IsComplete = false;
                entity.Status = 3;
                entity.AuditeTime = DateTime.Now;
                entity.AuditUserId = _Op.UserId;

                await _tD_CheckBus.UpdateDataAsync(entity);
                result.Success = true;
            }
            catch (Exception e)
            {
                result.Success = false;
                result.ErrorCode = 502;
                result.Msg = e.Message;
            }

            return result;
        }

        [HttpPost]
        public async Task<List<PB_Material>> LoadRandomMaterial(int per)
        {
            return await _provider.GetRequiredService<IIT_LocalMaterialBusiness>()
                .LoadMaterialByRandomAsync(_provider.GetRequiredService<IOperator>().Property.DefaultStorageId, per);
        }

        private async Task BuildDataByMaterialAsync(TDCheckConditionDTO model, TD_Check data)
        {
            var _Op = _provider.GetRequiredService<IOperator>();

            var materialList = (from u in model.MaterialIdList select new TD_CheckMaterial() { CheckId = data.Id, MaterialId = u }).Distinct().ToList();
            await _provider.GetRequiredService<ITD_CheckMaterialBusiness>().PushAsync(materialList);

            var localList = await _provider.GetRequiredService<IIT_LocalMaterialBusiness>()
                .LoadCheckDataByMaterialAsync(_Op.Property.DefaultStorageId, model.MaterialIdList);
            var checkdata = (from u in localList
                             select new TD_CheckData()
                             {
                                 BatchNo = u.BatchNo,
                                 CheckId = data.Id,
                                 CreateTime = DateTime.Now,
                                 CreatorId = _Op.UserId,
                                 Id = IdHelper.GetId(),
                                 localId = u.LocalId,
                                 LocalNum = u.Num,
                                 MaterialId = u.MaterialId,
                                 StorId = u.StorId
                             }).ToList();

            await _provider.GetRequiredService<ITD_CheckDataBusiness>().PushDataAsync(checkdata);
        }

        private async Task BuildDataByAreaAsync(List<string> Ids, TD_Check data)
        {
            try
            {
                var areaList = new List<TD_CheckArea>();
                var materList = new List<PB_AreaMaterial>();
                var localList = new List<IT_LocalMaterial>();
                foreach (var id in Ids)
                {
                    TD_CheckArea area = new TD_CheckArea();
                    area.CherkId = data.Id;
                    area.StoarAreaId = id;

                    areaList.Add(area);

                    materList.AddRange(await _provider.GetRequiredService<IPB_AreaMaterialBusiness>().GetDataListAsync(id));

                    localList.AddRange(await _provider.GetRequiredService<IIT_LocalMaterialBusiness>().LoadCheckDataByAreaIdAsync(id));
                }
                await _provider.GetRequiredService<ITD_CheckAreaBusiness>().PushAsync(areaList);

                var idList = (from u in materList select u.MaterialId).Distinct().ToList();
                var materialList = (from u in idList select new TD_CheckMaterial() { CheckId = data.Id, MaterialId = u }).ToList();
                await _provider.GetRequiredService<ITD_CheckMaterialBusiness>().PushAsync(materialList);

                var checkdata = (from u in localList
                                 select new TD_CheckData()
                                 {
                                     BatchNo = u.BatchNo,
                                     CheckId = data.Id,
                                     CreateTime = DateTime.Now,
                                     CreatorId = _provider.GetRequiredService<IOperator>().UserId,
                                     Id = IdHelper.GetId(),
                                     localId = u.LocalId,
                                     TrayId = u.TrayId,
                                     ZoneId = u.ZoneId,
                                     LocalNum = u.Num,
                                     MaterialId = u.MaterialId,
                                     StorId = u.StorId
                                 }).ToList();

                await _provider.GetRequiredService<ITD_CheckDataBusiness>().PushDataAsync(checkdata);
            }
            catch(Exception)
            {
                throw;
            }
        }
    }
}