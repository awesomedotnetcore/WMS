using Coldairarrow.Entity.PD;
using Coldairarrow.Util;
using EFCore.Sharding;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Coldairarrow.Business.PD
{
    public partial class PD_PlanBusiness
    {
        public async Task<List<PD_Plan>> QueryAllDataAsync()
        {
            var q = GetIQueryable();
            var where = LinqHelper.True<PD_Plan>();

            return await q.Where(where).ToListAsync();
        }
    }
}
