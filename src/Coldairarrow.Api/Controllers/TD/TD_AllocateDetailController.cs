using Coldairarrow.Business.PB;
using Coldairarrow.Business.TD;
using Coldairarrow.Entity.TD;
using Coldairarrow.Util;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Api.Controllers.TD
{
    [Route("/TD/[controller]/[action]")]
    public class TD_AllocateDetailController : BaseApiController
    {
        #region DI

        public TD_AllocateDetailController(ITD_AllocateDetailBusiness tD_AllocateDetailBus, ITD_AllocateBusiness tD_AllocateBusiness,
            IPB_StorAreaBusiness pB_StorAreaBusiness, IPB_LocationBusiness pB_LocationBusiness)
        {
            _tD_AllocateDetailBus = tD_AllocateDetailBus;
            _tD_AllocateBusiness = tD_AllocateBusiness;
            _pB_StorAreaBusiness = pB_StorAreaBusiness;
            _pB_LocationBusiness = pB_LocationBusiness;
        }

        ITD_AllocateDetailBusiness _tD_AllocateDetailBus { get; }
        ITD_AllocateBusiness _tD_AllocateBusiness { get; }
        IPB_StorAreaBusiness _pB_StorAreaBusiness { get; }
        IPB_LocationBusiness _pB_LocationBusiness { get; }

        #endregion

        #region 获取

        [HttpPost]
        public async Task<PageResult<TD_AllocateDetail>> GetDataList(PageInput<ConditionDTO> input)
        {
            return await _tD_AllocateDetailBus.GetDataListAsync(input);
        }

        [HttpPost]
        public async Task<TD_AllocateDetail> GetTheData(IdInputDTO input)
        {
            return await _tD_AllocateDetailBus.GetTheDataAsync(input.id);
        }

        #endregion

        #region 提交

        [HttpPost]
        public async Task SaveData(TD_AllocateDetail data)
        {
            if (data.Id.IsNullOrEmpty())
            {
                InitEntity(data);

                await _tD_AllocateDetailBus.AddDataAsync(data);
            }
            else
            {
                await _tD_AllocateDetailBus.UpdateDataAsync(data);
            }
        }

        [HttpPost]
        public async Task SaveDatas(List<TD_AllocateDetailDTO> datas)
        {
            var AddDatas = new List<TD_AllocateDetail>();
            var UpdateDatas = new List<TD_AllocateDetail>();
            if (datas.Count > 0)
            {
                var allocateData = await _tD_AllocateBusiness.GetTheDataAsync(datas[0].AllocateId);
                var storAreaData = await _pB_StorAreaBusiness.GetInnerArea(allocateData.ToStorId);
                var localData = await _pB_LocationBusiness.GetDefaultLocal(allocateData.ToStorId, storAreaData.Id);

                foreach (var data in datas)
                {
                    if (data.AllocateDetailId.IsNullOrEmpty())
                    {
                        var newData = new TD_AllocateDetail()
                        {
                            FromStorId = data.StorId,
                            FromlocalId = data.FromLocalId,
                            FromTrayId = data.FromTrayId,
                            FromZoneId = data.FromZoneId,
                            ToStorId = allocateData.ToStorId,
                            TolocalId = localData.Id,
                            BarCode = data.BarCode,
                            BatchNo = data.BatchNo,
                            LocalNum = data.LocalNum,
                            MaterialId = data.MaterialId,
                            AllocateId = data.AllocateId
                        };
                        InitEntity(newData);
                        AddDatas.Add(newData);
                    }
                    else
                    {
                        var updataData = new TD_AllocateDetail()
                        {
                            Id = data.AllocateDetailId,
                            CreatorId = data.CreatorId,
                            CreateTime = data.CreateTime,
                            FromStorId = data.StorId,
                            FromlocalId = data.FromLocalId,
                            FromTrayId = data.FromTrayId,
                            FromZoneId = data.FromZoneId,
                            ToStorId = data.ToStorId,
                            TolocalId = data.ToLocalId,
                            BarCode = data.BarCode,
                            BatchNo = data.BatchNo,
                            Deleted = data.Deleted,
                            LocalNum = data.LocalNum,
                            MaterialId = data.MaterialId,
                            AllocateId = data.AllocateId
                        };
                        UpdateDatas.Add(updataData);
                    }
                }
                if (AddDatas.Count > 0)
                {
                    await _tD_AllocateDetailBus.AddDatasAsync(AddDatas);
                }
                if (UpdateDatas.Count > 0)
                {
                    await _tD_AllocateDetailBus.UpdateDatasAsync(UpdateDatas);
                }
            }
        }

        [HttpPost]
        public async Task DeleteData(List<string> ids)
        {
            await _tD_AllocateDetailBus.DeleteDataAsync(ids);
        }

        #endregion
    }
}