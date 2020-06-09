using Coldairarrow.Business.IT;
using Coldairarrow.Business.TD;
using Coldairarrow.Entity.TD;
using Coldairarrow.Util;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Api.Controllers.TD
{
    [Route("/TD/[controller]/[action]")]
    public class TD_AllocateController : BaseApiController
    {
        #region DI

        public TD_AllocateController(ITD_AllocateBusiness tD_AllocateBus, IOperator @op, ITD_AllocateDetailBusiness tD_AllocateDetailBusiness,
            IIT_LocalMaterialBusiness iT_LocalMaterialBusiness, IIT_LocalDetailBusiness iT_LocalDetailBusiness)
        {
            _tD_AllocateBus = tD_AllocateBus;
            _iT_LocalMaterialBusiness = iT_LocalMaterialBusiness;
            _iT_LocalDetailBusiness = iT_LocalDetailBusiness;
            _tD_AllocateDetailBusiness = tD_AllocateDetailBusiness;
            _Op = @op;
        }

        ITD_AllocateBusiness _tD_AllocateBus { get; }
        IIT_LocalMaterialBusiness _iT_LocalMaterialBusiness { get; }
        IIT_LocalDetailBusiness _iT_LocalDetailBusiness { get; }
        ITD_AllocateDetailBusiness _tD_AllocateDetailBusiness { get; }
        IOperator _Op { get; }

        #endregion

        #region 获取

        [HttpPost]
        public async Task<PageResult<TD_Allocate>> GetDataList(PageInput<SearchCondition> input)
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
        public async Task ApproveDatas(List<string> ids)
        {
            await _tD_AllocateBus.ApproveDatasAsync(ids, _Op.UserId);
            var dataList = new List<BusinessInfo>();
            var allocateList = await _tD_AllocateDetailBusiness.GetDataListByAllocateIdsAsync(ids);
            foreach (var i in allocateList)
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
            await _iT_LocalDetailBusiness.UpdataDatasByBussiness(dataList);
        }

        [HttpPost]
        public async Task RejectDatas(List<string> ids)
        {
            await _tD_AllocateBus.RejectDatasAsync(ids, _Op.UserId);
        }
        #endregion
    }
}