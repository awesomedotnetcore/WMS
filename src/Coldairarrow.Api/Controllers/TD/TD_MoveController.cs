using Coldairarrow.Business.TD;
using Coldairarrow.Entity.TD;
using Coldairarrow.Util;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Api.Controllers.TD
{
    [Route("/TD/[controller]/[action]")]
    public class TD_MoveController : BaseApiController
    {
        #region DI

        public TD_MoveController(ITD_MoveBusiness tD_MoveBus,IOperator @op, ITD_MoveDetailBusiness tD_MoveDetailBus)
        {
            _tD_MoveBus = tD_MoveBus;
            _Op = @op;
            _tD_MoveDetailBus = tD_MoveDetailBus;
        }

        ITD_MoveBusiness _tD_MoveBus { get; }
        IOperator _Op { get; }
        ITD_MoveDetailBusiness _tD_MoveDetailBus { get; }

        #endregion

        #region 获取

        [HttpPost]
        public async Task<PageResult<TD_Move>> GetDataList(PageInput<SearchCondition> input)
        {
            return await _tD_MoveBus.GetDataListAsync(input, _Op.Property.DefaultStorageId);
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
                data.Status = (int)MoveStatus.待审核;

                await _tD_MoveBus.AddDataAsync(data);
            }
            else
            {
                await _tD_MoveBus.UpdateDataAsync(data);
            }
        }

        [HttpPost]
        public async Task UpdateData(UpdateMoveInfo updateMoveInfo)
        {
            var data = await _tD_MoveBus.GetTheDataAsync(updateMoveInfo.moveId);
            double moveNum = 0;
            double totalAmt = 0;

            foreach (var i in updateMoveInfo.localNums)
            {
                moveNum += i;
            }
            foreach(var a in updateMoveInfo.amounts)
            {
                totalAmt += a;
            }
            data.MoveNum = moveNum.ToString();
            data.TotalAmt = totalAmt.ToString();

            await _tD_MoveBus.UpdateDataAsync(data);
        }

        [HttpPost]
        public async Task DeleteData(List<string> ids)
        {
            await _tD_MoveBus.DeleteDataAsync(ids);
        }

        #endregion
    }
}