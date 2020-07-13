using Coldairarrow.Entity.IT;
using Coldairarrow.Entity.TD;
using Coldairarrow.IBusiness.Report;
using Coldairarrow.Util;
using EFCore.Sharding;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Coldairarrow.Business.IT
{
    public partial class Report_IntroduceBusiness : BaseBusiness<IT_LocalMaterial>, IReport_IntroduceBussiness, ITransientDependency
    {
        readonly IServiceProvider _ServiceProvider;
        public Report_IntroduceBusiness(IDbAccessor db, IServiceProvider svc)
            : base(db)
        {
            _ServiceProvider = svc;
        }

        public async Task<IntroduceDTO> GetMaterialSummary(DateTime date, string storId)
        {
            var result = new IntroduceDTO();
            result.Total = await this.GetIQueryable().SumAsync(w => w.Num);
            if (!storId.IsNullOrEmpty())
                result.Storage = await this.GetIQueryable().Where(w => w.StorId == storId).SumAsync(w => w.Num);
            return result;
        }

        public async Task<IntroduceDTO> GetInStorageSummary(DateTime date, string storId)
        {
            var startTime = date.Date;
            var endTime = startTime.AddDays(1);
            var result = new IntroduceDTO();
            result.Total = await Db.GetIQueryable<TD_InStorage>().Where(w => w.Status == 1 && w.InStorTime >= startTime && w.InStorTime < endTime).SumAsync(w => w.TotalNum);
            if (!storId.IsNullOrEmpty())
                result.Storage = await Db.GetIQueryable<TD_InStorage>().Where(w => w.Status == 1 && w.StorId == storId).Where(w => w.InStorTime >= startTime && w.InStorTime < endTime).SumAsync(w => w.TotalNum);
            return result;
        }

        public async Task<IntroduceDTO> GetOutStorageSummary(DateTime date, string storId)
        {
            var startTime = date.Date;
            var endTime = startTime.AddDays(1);
            var result = new IntroduceDTO();
            result.Total = await Db.GetIQueryable<TD_OutStorage>().Where(w => w.Status == 1 && w.OutTime >= startTime && w.OutTime < endTime).SumAsync(w => w.OutNum);
            if (!storId.IsNullOrEmpty())
                result.Storage = await Db.GetIQueryable<TD_OutStorage>().Where(w => w.Status == 1 && w.StorageId == storId).Where(w => w.OutTime >= startTime && w.OutTime < endTime).SumAsync(w => w.OutNum);
            return result;
        }

        public async Task<List<IntroduceHistoryDTO>> GetInStorageList(DateTime date, string storId)
        {
            var startTime = date.Date.AddDays(-6);
            var endTime = date.Date.AddDays(1);

            var qTotal = from i in Db.GetIQueryable<TD_InStorage>()
                         where i.Status == 1 && i.InStorTime >= startTime && i.InStorTime < endTime
                         group i by EF.Functions.DateDiffDay(i.InStorTime, endTime) into g
                         select new
                         {
                             Name = "总数",
                             Day0 = g.Sum(a => g.Key.Equals(0) ? a.TotalNum : 0),
                             Day1 = g.Sum(a => g.Key.Equals(1) ? a.TotalNum : 0),
                             Day2 = g.Sum(a => g.Key.Equals(2) ? a.TotalNum : 0),
                             Day3 = g.Sum(a => g.Key.Equals(3) ? a.TotalNum : 0),
                             Day4 = g.Sum(a => g.Key.Equals(4) ? a.TotalNum : 0),
                             Day5 = g.Sum(a => g.Key.Equals(5) ? a.TotalNum : 0),
                             Day6 = g.Sum(a => g.Key.Equals(6) ? a.TotalNum : 0)
                         } into gs
                         group gs by gs.Name into s
                         select new IntroduceHistoryDTO()
                         {
                             Name = s.Key,
                             Day0 = s.Sum(a => a.Day0),
                             Day1 = s.Sum(a => a.Day1),
                             Day2 = s.Sum(a => a.Day2),
                             Day3 = s.Sum(a => a.Day3),
                             Day4 = s.Sum(a => a.Day4),
                             Day5 = s.Sum(a => a.Day5),
                             Day6 = s.Sum(a => a.Day6),
                         };

            var listTotal = await qTotal.ToListAsync();
            if (listTotal.Count == 0) listTotal.Add(new IntroduceHistoryDTO() { Name = "总数" });

            var qStorage = from i in Db.GetIQueryable<TD_InStorage>()
                           where i.Status == 1 && i.InStorTime >= startTime && i.InStorTime < endTime && i.StorId == storId
                           group i by EF.Functions.DateDiffDay(i.InStorTime, endTime) into g
                           select new
                           {
                               Name = "仓库",
                               Day0 = g.Sum(a => g.Key.Equals(0) ? a.TotalNum : 0),
                               Day1 = g.Sum(a => g.Key.Equals(1) ? a.TotalNum : 0),
                               Day2 = g.Sum(a => g.Key.Equals(2) ? a.TotalNum : 0),
                               Day3 = g.Sum(a => g.Key.Equals(3) ? a.TotalNum : 0),
                               Day4 = g.Sum(a => g.Key.Equals(4) ? a.TotalNum : 0),
                               Day5 = g.Sum(a => g.Key.Equals(5) ? a.TotalNum : 0),
                               Day6 = g.Sum(a => g.Key.Equals(6) ? a.TotalNum : 0)
                           } into gs
                           group gs by gs.Name into s
                           select new IntroduceHistoryDTO()
                           {
                               Name = s.Key,
                               Day0 = s.Sum(a => a.Day0),
                               Day1 = s.Sum(a => a.Day1),
                               Day2 = s.Sum(a => a.Day2),
                               Day3 = s.Sum(a => a.Day3),
                               Day4 = s.Sum(a => a.Day4),
                               Day5 = s.Sum(a => a.Day5),
                               Day6 = s.Sum(a => a.Day6),
                           };
            var listStorge = await qStorage.ToListAsync();
            if (listStorge.Count == 0) listStorge.Add(new IntroduceHistoryDTO() { Name = "仓库" });

            listTotal.AddRange(listStorge);
            return listTotal;
            //            var sql = @"
            //SELECT
            //	'总数' as `Name`,
            //	IFNULL(SUM(case when DATEDIFF(InStorTime,@CurDate)=0 then TotalNum ELSE 0 END),0) as Day0,
            //	IFNULL(SUM(case when DATEDIFF(InStorTime,@CurDate)=-1 then TotalNum ELSE 0 END),0) as Day1,
            //	IFNULL(SUM(case when DATEDIFF(InStorTime,@CurDate)=-2 then TotalNum ELSE 0 END),0) as Day2,
            //	IFNULL(SUM(case when DATEDIFF(InStorTime,@CurDate)=-3 then TotalNum ELSE 0 END),0) as Day3,
            //	IFNULL(SUM(case when DATEDIFF(InStorTime,@CurDate)=-4 then TotalNum ELSE 0 END),0) as Day4,
            //	IFNULL(SUM(case when DATEDIFF(InStorTime,@CurDate)=-5 then TotalNum ELSE 0 END),0) as Day5,
            //	IFNULL(SUM(case when DATEDIFF(InStorTime,@CurDate)=-6 then TotalNum ELSE 0 END),0) as Day6
            //from TD_InStorage
            //WHERE `Status`=1 AND InStorTime BETWEEN DATE_ADD(@CurDate,INTERVAL '-8' day) and DATE_ADD(@CurDate,INTERVAL '1' day)
            //UNION
            //SELECT
            //	'仓库' as `Name`,
            //	IFNULL(SUM(case when DATEDIFF(InStorTime,@CurDate)=0 then TotalNum ELSE 0 END),0) as Day0,
            //	IFNULL(SUM(case when DATEDIFF(InStorTime,@CurDate)=-1 then TotalNum ELSE 0 END),0) as Day1,
            //	IFNULL(SUM(case when DATEDIFF(InStorTime,@CurDate)=-2 then TotalNum ELSE 0 END),0) as Day2,
            //	IFNULL(SUM(case when DATEDIFF(InStorTime,@CurDate)=-3 then TotalNum ELSE 0 END),0) as Day3,
            //	IFNULL(SUM(case when DATEDIFF(InStorTime,@CurDate)=-4 then TotalNum ELSE 0 END),0) as Day4,
            //	IFNULL(SUM(case when DATEDIFF(InStorTime,@CurDate)=-5 then TotalNum ELSE 0 END),0) as Day5,
            //	IFNULL(SUM(case when DATEDIFF(InStorTime,@CurDate)=-6 then TotalNum ELSE 0 END),0) as Day6
            //from TD_InStorage
            //WHERE `Status`=1 AND StorId=@StorId AND InStorTime BETWEEN DATE_ADD(@CurDate,INTERVAL '-8' day) and DATE_ADD(@CurDate,INTERVAL '1' day)
            //";
            //            var list = await Db.GetListBySqlAsync<IntroduceHistoryDTO>(sql, ("@CurDate", startTime), ("@StorId", storId));
            //            return list;
        }

        public async Task<List<IntroduceHistoryDTO>> GetOutStorageList(DateTime date, string storId)
        {
            var startTime = date.Date.AddDays(-6);
            var endTime = date.Date.AddDays(1);
            var qTotal = from i in Db.GetIQueryable<TD_OutStorage>()
                         where i.Status == 1 && i.OutTime >= startTime && i.OutTime < endTime
                         group i by EF.Functions.DateDiffDay(i.OutTime, endTime) into g
                         select new
                         {
                             Name = "总数",
                             Day0 = g.Sum(a => g.Key.Equals(0) ? a.OutNum : 0),
                             Day1 = g.Sum(a => g.Key.Equals(1) ? a.OutNum : 0),
                             Day2 = g.Sum(a => g.Key.Equals(2) ? a.OutNum : 0),
                             Day3 = g.Sum(a => g.Key.Equals(3) ? a.OutNum : 0),
                             Day4 = g.Sum(a => g.Key.Equals(4) ? a.OutNum : 0),
                             Day5 = g.Sum(a => g.Key.Equals(5) ? a.OutNum : 0),
                             Day6 = g.Sum(a => g.Key.Equals(6) ? a.OutNum : 0)
                         } into gs
                         group gs by gs.Name into s
                         select new IntroduceHistoryDTO()
                         {
                             Name = s.Key,
                             Day0 = s.Sum(a => a.Day0),
                             Day1 = s.Sum(a => a.Day1),
                             Day2 = s.Sum(a => a.Day2),
                             Day3 = s.Sum(a => a.Day3),
                             Day4 = s.Sum(a => a.Day4),
                             Day5 = s.Sum(a => a.Day5),
                             Day6 = s.Sum(a => a.Day6),
                         };

            var listTotal = await qTotal.ToListAsync();
            if (listTotal.Count == 0) listTotal.Add(new IntroduceHistoryDTO() { Name = "总数" });

            var qStorage = from i in Db.GetIQueryable<TD_OutStorage>()
                           where i.Status == 1 && i.OutTime >= startTime && i.OutTime < endTime && i.StorageId == storId
                           group i by EF.Functions.DateDiffDay(i.OutTime, endTime) into g
                           select new
                           {
                               Name = "仓库",
                               Day0 = g.Sum(a => g.Key.Equals(0) ? a.OutNum : 0),
                               Day1 = g.Sum(a => g.Key.Equals(1) ? a.OutNum : 0),
                               Day2 = g.Sum(a => g.Key.Equals(2) ? a.OutNum : 0),
                               Day3 = g.Sum(a => g.Key.Equals(3) ? a.OutNum : 0),
                               Day4 = g.Sum(a => g.Key.Equals(4) ? a.OutNum : 0),
                               Day5 = g.Sum(a => g.Key.Equals(5) ? a.OutNum : 0),
                               Day6 = g.Sum(a => g.Key.Equals(6) ? a.OutNum : 0)
                           } into gs
                           group gs by gs.Name into s
                           select new IntroduceHistoryDTO()
                           {
                               Name = s.Key,
                               Day0 = s.Sum(a => a.Day0),
                               Day1 = s.Sum(a => a.Day1),
                               Day2 = s.Sum(a => a.Day2),
                               Day3 = s.Sum(a => a.Day3),
                               Day4 = s.Sum(a => a.Day4),
                               Day5 = s.Sum(a => a.Day5),
                               Day6 = s.Sum(a => a.Day6),
                           };
            var listStorge = await qStorage.ToListAsync();
            if (listStorge.Count == 0) listStorge.Add(new IntroduceHistoryDTO() { Name = "仓库" });

            listTotal.AddRange(listStorge);
            return listTotal;
            //            var startTime = date.Date;
            //            var sql = @"
            //SELECT
            //	'总数' as `Name`,
            //	IFNULL(SUM(case when DATEDIFF(OutTime,@CurDate)=0 then OutNum ELSE 0 END),0) as Day0,
            //	IFNULL(SUM(case when DATEDIFF(OutTime,@CurDate)=-1 then OutNum ELSE 0 END),0) as Day1,
            //	IFNULL(SUM(case when DATEDIFF(OutTime,@CurDate)=-2 then OutNum ELSE 0 END),0) as Day2,
            //	IFNULL(SUM(case when DATEDIFF(OutTime,@CurDate)=-3 then OutNum ELSE 0 END),0) as Day3,
            //	IFNULL(SUM(case when DATEDIFF(OutTime,@CurDate)=-4 then OutNum ELSE 0 END),0) as Day4,
            //	IFNULL(SUM(case when DATEDIFF(OutTime,@CurDate)=-5 then OutNum ELSE 0 END),0) as Day5,
            //	IFNULL(SUM(case when DATEDIFF(OutTime,@CurDate)=-6 then OutNum ELSE 0 END),0) as Day6
            //from TD_OutStorage
            //WHERE `Status`=1 AND OutTime BETWEEN DATE_ADD(@CurDate,INTERVAL '-8' day) and DATE_ADD(@CurDate,INTERVAL '1' day)
            //UNION
            //SELECT
            //	'仓库' as `Name`,
            //	IFNULL(SUM(case when DATEDIFF(OutTime,@CurDate)=0 then OutNum ELSE 0 END),0) as Day0,
            //	IFNULL(SUM(case when DATEDIFF(OutTime,@CurDate)=-1 then OutNum ELSE 0 END),0) as Day1,
            //	IFNULL(SUM(case when DATEDIFF(OutTime,@CurDate)=-2 then OutNum ELSE 0 END),0) as Day2,
            //	IFNULL(SUM(case when DATEDIFF(OutTime,@CurDate)=-3 then OutNum ELSE 0 END),0) as Day3,
            //	IFNULL(SUM(case when DATEDIFF(OutTime,@CurDate)=-4 then OutNum ELSE 0 END),0) as Day4,
            //	IFNULL(SUM(case when DATEDIFF(OutTime,@CurDate)=-5 then OutNum ELSE 0 END),0) as Day5,
            //	IFNULL(SUM(case when DATEDIFF(OutTime,@CurDate)=-6 then OutNum ELSE 0 END),0) as Day6
            //from TD_OutStorage
            //WHERE `Status`=1 AND StorageId=@StorId AND OutTime BETWEEN DATE_ADD(@CurDate,INTERVAL '-8' day) and DATE_ADD(@CurDate,INTERVAL '1' day)
            //";
            //            var list = await Db.GetListBySqlAsync<IntroduceHistoryDTO>(sql, ("@CurDate", startTime), ("@StorId", storId));
            //            return list;
        }
    }
}