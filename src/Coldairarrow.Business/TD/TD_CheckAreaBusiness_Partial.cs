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
    public partial class TD_CheckAreaBusiness
    {

        public async Task<List<string>> QueryAsync(string checkId)
        {
            var q = GetIQueryable();
            var where = LinqHelper.True<TD_CheckArea>();

            where = where.And(p => p.CherkId == checkId);

            return await q.Where(where).Select(u=>u.StoarAreaId).ToListAsync();
        }

        public async Task ClearDataAsync(string checkId)
        {
            string sql = string.Format("DELETE FROM TD_CheckArea WHERE CherkId='{0}'", checkId);

            await Db.ExecuteSqlAsync(sql);
        }

        public async Task PushAsync(List<TD_CheckArea> data)
        {
            await base.InsertAsync(data);
        }
    }
}