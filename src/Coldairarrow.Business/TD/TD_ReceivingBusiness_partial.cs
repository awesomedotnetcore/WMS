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
    public partial class TD_ReceivingBusiness : BaseBusiness<TD_Receiving>, ITD_ReceivingBusiness, ITransientDependency
    {
        public TD_ReceivingBusiness(IDbAccessor db, IServiceProvider svcProvider)
            : base(db)
        {
            _ServiceProvider = svcProvider;
        }
        readonly IServiceProvider _ServiceProvider;
        public async Task<PageResult<TD_Receiving>> GetDataListAsync(TD_ReceivingPageInput input)
        {
            var q = this.GetIQueryable().Include(i => i.Supplier).Where(w => w.StorId == input.StorId);
            var search = input.Search;
            var where = LinqHelper.True<TD_Receiving>();
            where = where.AndIf(!search.Keyword.IsNullOrEmpty(), w => w.Code.Contains(search.Keyword) || w.RefCode.Contains(search.Keyword) || w.Supplier.Code.Contains(search.Keyword) || w.Supplier.Name.Contains(search.Keyword));
            where = where.AndIf(!search.Type.IsNullOrEmpty(), w => w.Type == search.Type);
            where = where.AndIf(search.Status.HasValue, w => w.Status == search.Status.Value);
            where = where.AndIf(search.OrderTimeStart.HasValue, w => w.OrderTime >= search.OrderTimeStart.Value);
            where = where.AndIf(search.OrderTimeEnd.HasValue, w => w.OrderTime <= search.OrderTimeEnd.Value);
            return await q.Where(where).GetPageResultAsync(input);
        }
        public async Task<TD_Receiving> GetTheDataAsync(string id)
        {
            return await this.GetIQueryable()
                .Include(i => i.RecDetails)
                    .ThenInclude(t => t.Material)
                .Include(i => i.RecDetails)
                    .ThenInclude(t => t.Measure)
                .SingleOrDefaultAsync(w => w.Id == id);
        }

        [DataAddLog(UserLogType.收货管理, "Code", "收货单")]
        [Transactional]
        public async Task AddDataAsync(TD_Receiving data)
        {
            if (data.Code.IsNullOrEmpty())
            {
                var codeSvc = _ServiceProvider.GetRequiredService<IPB_BarCodeTypeBusiness>();
                data.Code = await codeSvc.Generate("TD_Receiving");
            }
            data.TotalNum = data.RecDetails.Sum(s => s.PlanNum);
            data.TotalAmt = data.RecDetails.Sum(s => s.Amount);
            await InsertAsync(data);
        }

        [DataEditLog(UserLogType.收货管理, "Code", "收货单")]
        [Transactional]
        public async Task UpdateDataAsync(TD_Receiving data)
        {
            var curDetail = data.RecDetails;
            var listDetail = await Db.GetIQueryable<TD_RecDetail>().Where(w => w.RecId == data.Id).ToListAsync();

            var curIds = curDetail.Select(s => s.Id).ToList();
            var dbIds = listDetail.Select(s => s.Id).ToList();
            var deleteIds = dbIds.Except(curIds).ToList();
            var detailSvc = _ServiceProvider.GetRequiredService<ITD_RecDetailBusiness>();
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

            data.TotalNum = data.RecDetails.Sum(s => s.PlanNum);
            data.TotalAmt = data.RecDetails.Sum(s => s.Amount);

            await UpdateAsync(data);
        }

        [DataDeleteLog(UserLogType.收货管理, "Code", "收货单")]
        public async Task DeleteDataAsync(List<string> ids)
        {
            await DeleteAsync(ids);
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
            if (audit.AuditType == AuditType.Approve)
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

        [Transactional]
        public async Task UpdateByInStorage(string id)
        {
            var listIn = await Db.GetIQueryable<TD_InStorage>().Include(i => i.InStorDetails).Where(w => w.RecId == id && w.Status <= 1).ToListAsync();
            var receive = await this.GetTheDataAsync(id);

            // 所有入库的
            var listInDetail1 = new List<TD_InStorDetail>();
            // 已经审批入库
            var listInDetail2 = new List<TD_InStorDetail>();
            foreach (var item in listIn)
            {
                listInDetail1.AddRange(item.InStorDetails);
                if (item.Status == 1)
                    listInDetail2.AddRange(item.InStorDetails);
            }

            var dicIn1 = listInDetail1.GroupBy(g => g.MaterialId).Select(s => new { s.Key, Num = s.Sum(i => i.Num) }).ToDictionary(k => k.Key, v => v.Num);
            var dicIn2 = listInDetail2.GroupBy(g => g.MaterialId).Select(s => new { s.Key, Num = s.Sum(i => i.Num) }).ToDictionary(k => k.Key, v => v.Num);
            var receiveDetail = receive.RecDetails;
            foreach (var item in receiveDetail)
            {
                if (dicIn1.ContainsKey(item.MaterialId))
                    item.RecNum = dicIn1[item.MaterialId];
                if (dicIn2.ContainsKey(item.MaterialId))
                    item.InNum = dicIn2[item.MaterialId];
            }

            var detailSvc = _ServiceProvider.GetRequiredService<ITD_RecDetailBusiness>();
            await detailSvc.UpdateDataAsync(receiveDetail);

            var sunRec = dicIn1.Values.Sum();
            receive.InNum = sunRec;
            receive.Status = sunRec < receive.TotalNum ? 5 : 6;

            await this.UpdateAsync(receive);
        }

        public async Task AutoInStorage(string id)
        {
            await Task.Delay(0);

        }
    }
}