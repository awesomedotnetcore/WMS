using Coldairarrow.Business.PB;
using Coldairarrow.Entity.TD;
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

        public async Task<TD_Send> GetTheDataAsync(string id)
        {
            return await this.GetIQueryable()
                .Include(i => i.SendDetails)
                    .ThenInclude(t => t.Material)
                .Include(i => i.SendDetails)
                    .ThenInclude(t => t.Measure)
                .SingleOrDefaultAsync(w => w.Id == id);
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
    }
}