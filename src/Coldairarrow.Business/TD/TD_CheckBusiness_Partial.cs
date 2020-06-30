using Coldairarrow.Business.IT;
using Coldairarrow.Entity.IT;
using Coldairarrow.Entity.TD;
using Coldairarrow.IBusiness.DTO;
using Coldairarrow.Util;
using EFCore.Sharding;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Quartz.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Coldairarrow.Business.TD
{
    public partial class TD_CheckBusiness
    {
        public async Task<PageResult<TD_Check>> QueryDataListAsync(string storId,PageInput<TDCheckQueryDTO> input)
        {
            var q = GetIQueryable().Include(i => i.AuditUser);
            var where = LinqHelper.True<TD_Check>();
            var search = input.Search;

            where = where.And(p => p.StorId == storId);
            if (search.IsComplete > -1) where = where.And(p => p.IsComplete == (search.IsComplete == 1));
            if (search.Status > -1) where = where.And(p => p.Status == search.Status);
            if (!search.RefCode.IsNullOrWhiteSpace()) where = where.And(p => p.RefCode.Contains(search.RefCode));
            if (!search.RefCode.IsNullOrWhiteSpace()) where = where.And(p => p.Code.Contains(search.RefCode));
            if (!search.Type.IsNullOrWhiteSpace()) where = where.And(p => p.Type == search.Type);

            DateTime dtStartTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-01 00:00:00"));
            DateTime dtEndTime= Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 23:59:59"));
            if (search.RangeDate!=null && search.RangeDate.Length==2)
            {
                if(!search.RangeDate[0].IsNullOrWhiteSpace())
                {
                    dtStartTime = Convert.ToDateTime(search.RangeDate[0]);
                }

                if (!search.RangeDate[1].IsNullOrWhiteSpace())
                {
                    dtEndTime = Convert.ToDateTime(search.RangeDate[1]);
                }
            }
            where = where.And(p => p.CheckTime >= dtStartTime && p.CheckTime <= dtEndTime);

            return await q.Where(where).GetPageResultAsync(input);
        }

        /// <summary>
        /// 通过盘点审核，处理盘点数据
        /// </summary>
        /// <param name="data"></param>
        /// <param name="localData"></param>
        /// <param name="recordData"></param>
        /// <returns></returns>
        [Transactional]
        public async Task PassHandleAsync(TD_Check data, List<BusinessInfo> localData, List<IT_RecordBook> recordData)
        {
            var localSvc = _ServiceProvider.GetRequiredService<IIT_LocalMaterialBusiness>();
            var recordSvc = _ServiceProvider.GetRequiredService<IIT_RecordBookBusiness>();

            await UpdateAsync(data);
            await localSvc.UpdataDatasByBussiness(localData);//更新库存
            await recordSvc.AddDataAsync(recordData);        //添加台帐
        }
    }
}