using Coldairarrow.Business.PB;
using Coldairarrow.Entity.TD;
using Coldairarrow.IBusiness.DTO;
using Coldairarrow.Util;
using EFCore.Sharding;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Coldairarrow.Business.TD
{
    public partial class TD_SendBusiness : BaseBusiness<TD_Send>, ITD_SendBusiness, ITransientDependency
    {
        public TD_SendBusiness(IDbAccessor db, IServiceProvider svcProvider)
            : base(db)
        {
            _ServiceProvider = svcProvider;
        }
        readonly IServiceProvider _ServiceProvider;

        public async Task<PageResult<TD_Send>> GetDataListAsync(TD_SendPageInput input)
        {
            var q = this.GetIQueryable().Include(i => i.Customer).Where(w => w.StorId == input.StorId);
            var search = input.Search;
            var where = LinqHelper.True<TD_Send>();
            where = where.AndIf(!search.Keyword.IsNullOrEmpty(), w => w.Code.Contains(search.Keyword) || w.RefCode.Contains(search.Keyword) || w.Customer.Code.Contains(search.Keyword) || w.Customer.Name.Contains(search.Keyword));
            where = where.AndIf(!search.Type.IsNullOrEmpty(), w => w.Type == search.Type);
            where = where.AndIf(search.Status.HasValue, w => w.Status == search.Status.Value);
            where = where.AndIf(search.OrderTimeStart.HasValue, w => w.OrderTime >= search.OrderTimeStart.Value);
            where = where.AndIf(search.OrderTimeEnd.HasValue, w => w.OrderTime <= search.OrderTimeEnd.Value);
            return await q.Where(where).GetPageResultAsync(input);
        }

        public async Task<TD_Send> GetTheDataAsync(string id)
        {
           var result = await this.GetIQueryable()
                .Include(i => i.SendDetails)
                    .ThenInclude(t => t.Location)
                .Include(i => i.SendDetails)
                    .ThenInclude(t => t.Material)
                .Include(i => i.SendDetails)
                    .ThenInclude(t => t.Measure)
                .SingleOrDefaultAsync(w => w.Id == id);

            return result;

            //return await this.GetIQueryable()
            //    .Include(i => i.SendDetails)
            //        .ThenInclude(t => t.Location)
            //    .Include(i => i.SendDetails)
            //        .ThenInclude(t => t.Material)
            //    .Include(i => i.SendDetails)
            //        .ThenInclude(t => t.Measure)
            //    .SingleOrDefaultAsync(w => w.Id == id);
        }

        [DataAddLog(UserLogType.发货管理, "Code", "发货单")]
        [Transactional]
        public async Task AddDataAsync(TD_Send data)
        {
            if (data.Code.IsNullOrEmpty())
            {
                var codeSvc = _ServiceProvider.GetRequiredService<IPB_BarCodeTypeBusiness>();
                data.Code = await codeSvc.Generate("TD_Send");
            }
            data.TotalNum = data.SendDetails.Sum(s => s.PlanNum);
            data.TotalAmt = data.SendDetails.Sum(s => s.Amount);
            await InsertAsync(data);
        }

        public async Task UpdateDetailAsync(TD_Send data)
        {
            data.SendNum = data.SendDetails.Sum(s => s.SendNum);
            data.TotalAmt = data.SendDetails.Sum(s => s.Amount);
            await UpdateAsync(data);
        }

        [DataEditLog(UserLogType.发货管理, "Code", "发货单")]
        public async Task UpdateDataAsync(TD_Send data)
        {
            var curDetail = data.SendDetails;
            var listDetail = await Db.GetIQueryable<TD_SendDetail>().Where(w => w.SendId == data.Id).ToListAsync();

            var curIds = curDetail.Select(s => s.Id).ToList();
            var dbIds = listDetail.Select(s => s.Id).ToList();
            var deleteIds = dbIds.Except(curIds).ToList();
            var detailSvc = _ServiceProvider.GetRequiredService<ITD_SendDetailBusiness>();
            if (deleteIds.Count > 0)
                await detailSvc.DeleteDataAsync(deleteIds);

            var addIds = curIds.Except(dbIds).ToList();
            if (addIds.Count > 0)
            {
                var listAdds = curDetail.Where(w => addIds.Contains(w.Id)).ToList();
                await detailSvc.AddDataAsync(listAdds);
            }

            var updateIds = curIds.Except(addIds).ToList();
            if (updateIds.Count > 0)
            {
                var listUpdates = curDetail.Where(w => updateIds.Contains(w.Id)).ToList();
                await detailSvc.UpdateDataAsync(listUpdates);
            }

            data.SendNum = data.SendDetails.Sum(s => s.SendNum);
            data.TotalAmt = data.SendDetails.Sum(s => s.Amount);

            await UpdateAsync(data);
        }


        public async Task Approval(AuditDTO audit)
        {
            var data = await this.GetEntityAsync(audit.Id);
            if (audit.AuditType == AuditType.Confirm)
            {
                data.Status = 1;
                data.ConfirmTime = audit.AuditTime;
                data.ConfirmUserId = audit.AuditUserId;
            }
            if (audit.AuditType == AuditType.Cancel)
            {
                data.Status = 2;
                data.ConfirmTime = audit.AuditTime;
                data.ConfirmUserId = audit.AuditUserId;
            }
            if (audit.AuditType == AuditType.Approve)//审核通过
            {            
                data.Status = 3;
                data.AuditeTime = audit.AuditTime;
                data.AuditUserId = audit.AuditUserId;

            }
            if (audit.AuditType == AuditType.Reject)
            {
                data.Status = 4;
                data.AuditeTime = audit.AuditTime;
                data.AuditUserId = audit.AuditUserId;
            }
            await UpdateAsync(data);
        }

        public async Task UpdateByOutStorage(string id)
        {
            var listOut = await Db.GetIQueryable<TD_OutStorage>().Include(i => i.OutStorDetails).Where(w => w.SendId == id && w.Status <= 1).ToListAsync();
            var receive = await this.GetTheDataAsync(id);

            // 所有出库的
            var listOutDetail1 = new List<TD_OutStorDetail>();
            // 已经审批出库
            var listOutDetail2 = new List<TD_OutStorDetail>();
            foreach (var item in listOut)
            {
                listOutDetail1.AddRange(item.OutStorDetails);
                if (item.Status == 1)
                    listOutDetail2.AddRange(item.OutStorDetails);
            }

            var dicIn1 = listOutDetail1.GroupBy(g => g.MaterialId).Select(s => new { s.Key, Num = s.Sum(i => i.OutNum) }).ToDictionary(k => k.Key, v => v.Num);
            var dicIn2 = listOutDetail2.GroupBy(g => g.MaterialId).Select(s => new { s.Key, Num = s.Sum(i => i.OutNum) }).ToDictionary(k => k.Key, v => v.Num);
            var receiveDetail = receive.SendDetails;
            foreach (var item in receiveDetail)
            {
                if (dicIn1.ContainsKey(item.MaterialId))
                    item.SendNum = dicIn1[item.MaterialId];
                if (dicIn2.ContainsKey(item.MaterialId))
                    item.SendNum = dicIn2[item.MaterialId];
            }

            var detailSvc = _ServiceProvider.GetRequiredService<ITD_SendDetailBusiness>();
            await detailSvc.UpdateDataAsync(receiveDetail);

            var sunRec = dicIn1.Values.Sum();
            receive.SendNum = sunRec;
            receive.Status = receive.TotalNum == sunRec ? 6 : 5;

            await this.UpdateAsync(receive);
        }
    }
}