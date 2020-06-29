using Coldairarrow.Entity.PB;
using Coldairarrow.Entity.TD;
using Coldairarrow.Util;
using EFCore.Sharding;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Coldairarrow.Business.TD
{
    public partial class TD_CheckMaterialBusiness
    {
        public async Task ClearDataAsync(string checkId)
        {
            string sql = string.Format("DELETE FROM TD_CheckMaterial WHERE CheckId='{0}'", checkId);

            await Db.ExecuteSqlAsync(sql);
        }

        public async Task PushAsync(List<TD_CheckMaterial> data)
        {
            await base.InsertAsync(data);
        }

        public async Task<List<PB_Material>> QueryAsync(string checkId)
        {
            var q = GetIQueryable();
            q = q.Include(i => i.Material);
            var where = LinqHelper.True<TD_CheckMaterial>();
            where = where.And(p => p.CheckId == checkId);
            return await q.Where(where).Select(u => u.Material).ToListAsync();
        }
    }
}