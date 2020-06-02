using Coldairarrow.Business.TD;
using Coldairarrow.Entity.TD;
using Coldairarrow.Util;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Api.Controllers.TD
{
    [Route("/TD/[controller]/[action]")]
    public class TD_MoveDetailController : BaseApiController
    {
        #region DI

        public TD_MoveDetailController(ITD_MoveDetailBusiness tD_MoveDetailBus, IOperator @op)
        {
            _tD_MoveDetailBus = tD_MoveDetailBus;
            _Op = @op;
        }

        ITD_MoveDetailBusiness _tD_MoveDetailBus { get; }
        IOperator _Op { get; }

        #endregion

        #region 获取

        [HttpPost]
        public async Task<PageResult<TD_MoveDetail>> GetDataList(PageInput<ConditionDTO> input)
        {
            return await _tD_MoveDetailBus.GetDataListAsync(input);
        }

        [HttpPost]
        public async Task<TD_MoveDetail> GetTheData(IdInputDTO input)
        {
            return await _tD_MoveDetailBus.GetTheDataAsync(input.id);
        }

        #endregion

        #region 提交

        [HttpPost]
        public async Task SaveData(TD_MoveDetail data)
        {
            if (data.Id.IsNullOrEmpty())
            {
                InitEntity(data);
                data.StorId = _Op.Property.DefaultStorageId;
                data.Amount = data.LocalNum * data.Price;
                await _tD_MoveDetailBus.AddDataAsync(data);
            }
            else
            {
                await _tD_MoveDetailBus.UpdateDataAsync(data);
            }
        }

        [HttpPost]
        public async Task SaveDatas(List<TD_MoveDetailDTO> datas)
        {
            var AddDatas = new List<TD_MoveDetail>();
            var UpdateDatas = new List<TD_MoveDetail>();
            if (datas.Count > 0)
            {
                foreach(var data in datas)
                {
                    if (data.MoveDetailId.IsNullOrEmpty())
                    {
                        var newData = new TD_MoveDetail()
                        {
                            FromLocalId = data.FromLocalId,
                            FromTrayId = data.FromTrayId,
                            FromZoneId = data.FromZoneId,
                            ToLocalId = data.ToLocalId,
                            ToTrayId = data.ToTrayId,
                            ToZoneId = data.ToZoneId,
                            BarCode = data.BarCode,
                            BatchNo = data.BatchNo,
                            Deleted = data.Deleted,
                            LocalNum = data.LocalNum,
                            MaterialId = data.MaterialId,
                            MoveId = data.MoveId,
                            StorId = _Op.Property.DefaultStorageId
                        };
                        InitEntity(newData);
                        AddDatas.Add(newData);
                    }
                    else
                    {
                        var updataData = new TD_MoveDetail()
                        {
                            Id = data.MoveDetailId,
                            CreatorId = data.CreatorId,
                            CreateTime = data.CreateTime,
                            FromLocalId = data.FromLocalId,
                            FromTrayId = data.FromTrayId,
                            FromZoneId = data.FromZoneId,
                            ToLocalId = data.ToLocalId,
                            ToTrayId = data.ToTrayId,
                            ToZoneId = data.ToZoneId,
                            BarCode = data.BarCode,
                            BatchNo = data.BatchNo,
                            Deleted = data.Deleted,
                            LocalNum = data.LocalNum,
                            MaterialId = data.MaterialId,
                            MoveId = data.MoveId,
                            StorId = _Op.Property.DefaultStorageId
                        };
                        UpdateDatas.Add(updataData);
                    }
                }
                if(AddDatas.Count > 0)
                {
                    await _tD_MoveDetailBus.AddDatasAsync(AddDatas);
                }
                if (UpdateDatas.Count > 0)
                {
                    await _tD_MoveDetailBus.UpdateDatasAsync(UpdateDatas);
                }
            }
        }

        [HttpPost]
        public async Task DeleteData(List<string> ids)
        {
            await _tD_MoveDetailBus.DeleteDataAsync(ids);
        }

        #endregion
    }
}