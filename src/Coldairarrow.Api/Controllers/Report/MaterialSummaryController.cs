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
    public class MaterialSummaryController : BaseApiController
    {
        readonly IReport_MaterialSummaryBussiness _reportSvc;
        public MaterialSummaryController(IReport_MaterialSummaryBussiness svc)
        {
            _reportSvc = svc;
        }
        [HttpPost]
        [NoCheckJWT]
        public async Task<PageResult<Report_MaterialSummaryVM>> GetDataList(PageInput<Report_MaterialSummaryQM> input)
        {
            return await _reportSvc.GetDataListAsync(input);
        }
    }
}