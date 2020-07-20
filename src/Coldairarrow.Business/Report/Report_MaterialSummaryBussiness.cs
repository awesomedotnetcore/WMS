using Coldairarrow.Entity.IT;
using Coldairarrow.Entity.PB;
using Coldairarrow.Entity.TD;
using Coldairarrow.IBusiness.Report;
using Coldairarrow.Util;
using EFCore.Sharding;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using OracleInternal.SqlAndPlsqlParser.LocalParsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Coldairarrow.Business.IT
{
    public partial class Report_MaterialSummaryBussiness : BaseBusiness<IT_LocalMaterial>, IReport_MaterialSummaryBussiness, ITransientDependency
    {
        readonly IServiceProvider _ServiceProvider;
        public Report_MaterialSummaryBussiness(IDbAccessor db, IServiceProvider svc)
            : base(db)
        {
            _ServiceProvider = svc;
        }
        public async Task<PageResult<Report_MaterialSummaryVM>> GetDataListAsync(PageInput<Report_MaterialSummaryQM> input)
        {
            var search = input.Search;
            var mQuery = from lm in Db.GetIQueryable<IT_LocalMaterial>()
                         group lm by new { lm.MaterialId,lm.BatchNo } into g
                         select new { MaterialId = g.Key.MaterialId,BatchNo=g.Key.BatchNo, SumCount = g.Sum(p => p.Num) } into lms
                         join m in Db.GetIQueryable<PB_Material>() on lms.MaterialId equals m.Id
                         join mt in Db.GetIQueryable<PB_MaterialType>() on m.MaterialTypeId equals mt.Id
                         join me in Db.GetIQueryable<PB_Measure>() on m.MeasureId equals me.Id                         
                         select new Report_MaterialSummaryVM
                         {
                             Id = m.Id,
                             Name = m.Name,                         
                             Code = m.Code,
                             BarCode = m.BarCode,
                             SimpleName = m.SimpleName,
                             MaterialTypeId = m.MaterialTypeId,
                             MaterialId = m.Id,
                             MaterialName = m.Name,
                             MaterialTypeName = mt.Name,
                             MeasureId = me.Id,
                             MeasureName = me.Name,
                             Spec = m.Spec,
                             Max = m.Max,
                             Min = m.Min,
                             Price = m.Price,
                             SumCount = lms.SumCount,
                             BatchNo = lms.BatchNo,
                         };
            if (!search.MaterialTypeId.IsNullOrEmpty())
                mQuery = mQuery.Where(w => w.MaterialTypeId == search.MaterialTypeId);
            if (!search.MaterialName.IsNullOrEmpty())
                mQuery = mQuery.Where(w => w.Name.Contains(search.MaterialName) || w.Code.Contains(search.MaterialName) || w.SimpleName.Contains(search.MaterialName) || w.BarCode.Contains(search.MaterialName));
            if (search.MinAlert)
                mQuery = mQuery.Where(w => w.Min >= w.SumCount && w.Min.HasValue);
            if (search.MaxAlert)
                mQuery = mQuery.Where(w => w.Max <= w.SumCount && w.Max.HasValue);
            if (!search.BatchNo.IsNullOrEmpty())
                mQuery = mQuery.Where(w => w.BatchNo == search.BatchNo);
            var pageResult = await mQuery.GetPageResultAsync(input);
            //var listbatch =
            foreach (var item in pageResult.Data)
            {
                item.Id = item.Id + "_" + item.BatchNo;
            }
            return pageResult;
        }
    }
}