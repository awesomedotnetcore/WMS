using Coldairarrow.Business.Base_Manage;
using Coldairarrow.IBusiness.Report;
using Coldairarrow.Util;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Coldairarrow.IBusiness;

namespace Coldairarrow.Api.Controllers.Report
{
    /// <summary>
    /// 首页控制器
    /// </summary>
    [Route("/Report/[controller]/[action]")]
    public class IntroduceController : BaseApiController
    {
        readonly IReport_IntroduceBussiness _reportSvc;
        readonly IOperator _operator;
        public IntroduceController(IReport_IntroduceBussiness svc, IOperator @operator)
        {
            _reportSvc = svc;
            _operator = @operator;
        }

        [HttpGet]
        public async Task<IntroduceDTO> MaterialSummary()
        {
            return await _reportSvc.GetMaterialSummary(DateTime.Now, _operator.Property.DefaultStorageId);
        }
        [HttpGet]
        public async Task<IntroduceDTO> InStorageSummary()
        {
            return await _reportSvc.GetInStorageSummary(DateTime.Now, _operator.Property.DefaultStorageId);
        }
        [HttpGet]
        public async Task<IntroduceDTO> OutStorageSummary()
        {
            return await _reportSvc.GetOutStorageSummary(DateTime.Now, _operator.Property.DefaultStorageId);
        }
        [HttpGet]
        public async Task<List<IntroduceHistoryDTO>> InStorageList()
        {
            return await _reportSvc.GetInStorageList(DateTime.Now, _operator.Property.DefaultStorageId);
        }
        [HttpGet]
        public async Task<List<IntroduceHistoryDTO>> OutStorageList()
        {
            return await _reportSvc.GetOutStorageList(DateTime.Now, _operator.Property.DefaultStorageId);
        }
    }
}