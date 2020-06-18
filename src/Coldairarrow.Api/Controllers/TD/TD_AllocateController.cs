using Coldairarrow.Business.IT;
using Coldairarrow.Business.PB;
using Coldairarrow.Business.TD;
using Coldairarrow.Entity.TD;
using Coldairarrow.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Coldairarrow.Api.Controllers.TD
{
    [Route("/TD/[controller]/[action]")]
    public class TD_AllocateController : BaseApiController
    {
        #region DI

        public TD_AllocateController(ITD_AllocateBusiness tD_AllocateBus, IOperator @op, ITD_AllocateDetailBusiness tD_AllocateDetailBusiness,
            IIT_LocalMaterialBusiness iT_LocalMaterialBusiness, IIT_LocalDetailBusiness iT_LocalDetailBusiness,
            IServiceProvider svcProvider)
        {
            _tD_AllocateBus = tD_AllocateBus;
            _iT_LocalMaterialBusiness = iT_LocalMaterialBusiness;
            _iT_LocalDetailBusiness = iT_LocalDetailBusiness;
            _tD_AllocateDetailBusiness = tD_AllocateDetailBusiness;
            _Op = @op;
            _ServiceProvider = svcProvider;
        }

        ITD_AllocateBusiness _tD_AllocateBus { get; }
        IIT_LocalMaterialBusiness _iT_LocalMaterialBusiness { get; }
        IIT_LocalDetailBusiness _iT_LocalDetailBusiness { get; }
        ITD_AllocateDetailBusiness _tD_AllocateDetailBusiness { get; }
        IOperator _Op { get; }
        IServiceProvider _ServiceProvider { get; }

        #endregion

        #region 获取

        [HttpPost]
        public async Task<PageResult<TD_Allocate>> GetDataList(PageInput<ConditionDTO> input)
        {
            return await _tD_AllocateBus.GetDataListAsync(input, _Op.Property.DefaultStorageId);
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
            if (data.Code.IsNullOrEmpty())
            {
                var codeSvc = _ServiceProvider.GetRequiredService<IPB_BarCodeTypeBusiness>();
                data.Code = await codeSvc.Generate("TD_Allocate");
            }

            if (data.Id.IsNullOrEmpty())
            {
                InitEntity(data);
                data.StorId = _Op.Property.DefaultStorageId;
                data.Status = (int)AllocateStatus.待审核;

                await _tD_AllocateBus.AddDataAsync(data);
            }
            else
            {
                await _tD_AllocateBus.UpdateDataAsync(data);
            }
        }

        [HttpPost]
        public async Task DeleteData(List<string> ids)
        {
            await _tD_AllocateBus.DeleteDataAsync(ids);
        }

        [HttpPost]
        public async Task<AjaxResult> ApproveDatas(List<string> ids)
        {
            var allocateList = await _tD_AllocateBus.GetDataListAsync(ids);
            var changeList = new List<TD_Allocate>();

            foreach(var i in allocateList)
            {
                if(i.Status == (int)AllocateStatus.待审核)
                {
                    changeList.Add(i);
                }
            }

            if (changeList.Count <= 0)
            {
                return Error("所选数据没有待审批数据");
            }

            await _tD_AllocateBus.ApproveDatasAsync(changeList.Select(s => s.Id).ToList(), _Op.UserId);

            var dataList = new List<BusinessInfo>();
            var allocateDetailList = await _tD_AllocateDetailBusiness.GetDataListByAllocateIdsAsync(changeList.Select(s => s.Id).ToList());
            foreach (var i in allocateDetailList)
            {
                dataList.Add(new BusinessInfo()
                {
                    StorId = i.FromStorId,
                    LocalId = i.FromlocalId,
                    TrayId = i.FromTrayId,
                    ZoneId = i.FromZoneId,
                    MaterialId = i.MaterialId,
                    BarCode = i.BarCode,
                    BatchNo = i.BatchNo,
                    Num = i.LocalNum.HasValue ? i.LocalNum.Value : 0,
                    ActionType = (int)ActionTypeEnum.出库,
                    MeasureId = i.PB_Material.MeasureId
                });

                dataList.Add(new BusinessInfo()
                {
                    StorId = i.ToStorId,
                    LocalId = i.TolocalId,
                    MaterialId = i.MaterialId,
                    BarCode = i.BarCode,
                    BatchNo = i.BatchNo,
                    Num = i.LocalNum.HasValue ? i.LocalNum.Value : 0,
                    ActionType = (int)ActionTypeEnum.入库,
                    MeasureId = i.PB_Material.MeasureId
                });
            }

            await _iT_LocalMaterialBusiness.UpdataDatasByBussiness(dataList);
            await _iT_LocalDetailBusiness.UpdataDatasByBussiness(dataList, _Op.UserId);
            return Success();
        }

        [HttpPost]
        public async Task RejectDatas(List<string> ids)
        {
            await _tD_AllocateBus.RejectDatasAsync(ids, _Op.UserId);
        }
        #endregion
    }
}