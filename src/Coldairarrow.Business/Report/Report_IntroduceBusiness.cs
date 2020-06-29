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
            var startTime = date.Date;
            var sql = @"
SELECT
	'总数' as `Name`,
	IFNULL(SUM(case when DATEDIFF(InStorTime,@CurDate)=0 then TotalNum ELSE 0 END),0) as Day0,
	IFNULL(SUM(case when DATEDIFF(InStorTime,@CurDate)=-1 then TotalNum ELSE 0 END),0) as Day1,
	IFNULL(SUM(case when DATEDIFF(InStorTime,@CurDate)=-2 then TotalNum ELSE 0 END),0) as Day2,
	IFNULL(SUM(case when DATEDIFF(InStorTime,@CurDate)=-3 then TotalNum ELSE 0 END),0) as Day3,
	IFNULL(SUM(case when DATEDIFF(InStorTime,@CurDate)=-4 then TotalNum ELSE 0 END),0) as Day4,
	IFNULL(SUM(case when DATEDIFF(InStorTime,@CurDate)=-5 then TotalNum ELSE 0 END),0) as Day5,
	IFNULL(SUM(case when DATEDIFF(InStorTime,@CurDate)=-6 then TotalNum ELSE 0 END),0) as Day6
from TD_InStorage
WHERE `Status`=1 AND InStorTime BETWEEN DATE_ADD(@CurDate,INTERVAL '-8' day) and DATE_ADD(@CurDate,INTERVAL '1' day)
UNION
SELECT
	'仓库' as `Name`,
	IFNULL(SUM(case when DATEDIFF(InStorTime,@CurDate)=0 then TotalNum ELSE 0 END),0) as Day0,
	IFNULL(SUM(case when DATEDIFF(InStorTime,@CurDate)=-1 then TotalNum ELSE 0 END),0) as Day1,
	IFNULL(SUM(case when DATEDIFF(InStorTime,@CurDate)=-2 then TotalNum ELSE 0 END),0) as Day2,
	IFNULL(SUM(case when DATEDIFF(InStorTime,@CurDate)=-3 then TotalNum ELSE 0 END),0) as Day3,
	IFNULL(SUM(case when DATEDIFF(InStorTime,@CurDate)=-4 then TotalNum ELSE 0 END),0) as Day4,
	IFNULL(SUM(case when DATEDIFF(InStorTime,@CurDate)=-5 then TotalNum ELSE 0 END),0) as Day5,
	IFNULL(SUM(case when DATEDIFF(InStorTime,@CurDate)=-6 then TotalNum ELSE 0 END),0) as Day6
from TD_InStorage
WHERE `Status`=1 AND StorId=@StorId AND InStorTime BETWEEN DATE_ADD(@CurDate,INTERVAL '-8' day) and DATE_ADD(@CurDate,INTERVAL '1' day)
";
            var list = await Db.GetListBySqlAsync<IntroduceHistoryDTO>(sql, ("@CurDate", startTime), ("@StorId", storId));
            return list;
        }

        public async Task<List<IntroduceHistoryDTO>> GetOutStorageList(DateTime date, string storId)
        {
            var startTime = date.Date;
            var sql = @"
SELECT
	'总数' as `Name`,
	IFNULL(SUM(case when DATEDIFF(OutTime,@CurDate)=0 then OutNum ELSE 0 END),0) as Day0,
	IFNULL(SUM(case when DATEDIFF(OutTime,@CurDate)=-1 then OutNum ELSE 0 END),0) as Day1,
	IFNULL(SUM(case when DATEDIFF(OutTime,@CurDate)=-2 then OutNum ELSE 0 END),0) as Day2,
	IFNULL(SUM(case when DATEDIFF(OutTime,@CurDate)=-3 then OutNum ELSE 0 END),0) as Day3,
	IFNULL(SUM(case when DATEDIFF(OutTime,@CurDate)=-4 then OutNum ELSE 0 END),0) as Day4,
	IFNULL(SUM(case when DATEDIFF(OutTime,@CurDate)=-5 then OutNum ELSE 0 END),0) as Day5,
	IFNULL(SUM(case when DATEDIFF(OutTime,@CurDate)=-6 then OutNum ELSE 0 END),0) as Day6
from TD_OutStorage
WHERE `Status`=1 AND OutTime BETWEEN DATE_ADD(@CurDate,INTERVAL '-8' day) and DATE_ADD(@CurDate,INTERVAL '1' day)
UNION
SELECT
	'仓库' as `Name`,
	IFNULL(SUM(case when DATEDIFF(OutTime,@CurDate)=0 then OutNum ELSE 0 END),0) as Day0,
	IFNULL(SUM(case when DATEDIFF(OutTime,@CurDate)=-1 then OutNum ELSE 0 END),0) as Day1,
	IFNULL(SUM(case when DATEDIFF(OutTime,@CurDate)=-2 then OutNum ELSE 0 END),0) as Day2,
	IFNULL(SUM(case when DATEDIFF(OutTime,@CurDate)=-3 then OutNum ELSE 0 END),0) as Day3,
	IFNULL(SUM(case when DATEDIFF(OutTime,@CurDate)=-4 then OutNum ELSE 0 END),0) as Day4,
	IFNULL(SUM(case when DATEDIFF(OutTime,@CurDate)=-5 then OutNum ELSE 0 END),0) as Day5,
	IFNULL(SUM(case when DATEDIFF(OutTime,@CurDate)=-6 then OutNum ELSE 0 END),0) as Day6
from TD_OutStorage
WHERE `Status`=1 AND StorageId=@StorId AND OutTime BETWEEN DATE_ADD(@CurDate,INTERVAL '-8' day) and DATE_ADD(@CurDate,INTERVAL '1' day)
";
            var list = await Db.GetListBySqlAsync<IntroduceHistoryDTO>(sql, ("@CurDate", startTime), ("@StorId", storId));
            return list;
        }
    }
}