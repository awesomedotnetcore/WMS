﻿using Coldairarrow.Business.TD;
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

        public TD_MoveController(ITD_MoveBusiness tD_MoveBus,IOperator @op)
        {
            _tD_MoveBus = tD_MoveBus;
            _Op = @op;
        }

        ITD_MoveBusiness _tD_MoveBus { get; }
        IOperator _Op { get; }

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
        public async Task DeleteData(List<string> ids)
        {
            await _tD_MoveBus.DeleteDataAsync(ids);
        }

        #endregion
    }
}