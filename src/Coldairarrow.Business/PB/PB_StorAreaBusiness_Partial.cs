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
    public partial class PB_StorAreaBusiness 
    {
        public async Task<List<PB_StorArea>> QueryAsync(string storageId)
        {
            var q = GetIQueryable();

            return await q.Where(p => p.StorId == storageId).ToListAsync();
        }

        public async Task<PB_StorArea> GetInnerArea(string storageId)
        {
            var q = GetIQueryable();

            return await q.Where(p => p.StorId == storageId && p.Type == "In").FirstOrDefaultAsync();
        }
    }
}