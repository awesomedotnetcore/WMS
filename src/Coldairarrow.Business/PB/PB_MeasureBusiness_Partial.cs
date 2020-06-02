using Coldairarrow.Entity.PB;
using Coldairarrow.Util;
using EFCore.Sharding;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Coldairarrow.Business.PB
{
    public partial class PB_MeasureBusiness
    {
        public async Task<List<PB_Measure>> QueryAllDataAsync()
        {
            var q = GetIQueryable();
            var where = LinqHelper.True<PB_Measure>();
            

            return await q.Where(where).ToListAsync();
        }
    }
}