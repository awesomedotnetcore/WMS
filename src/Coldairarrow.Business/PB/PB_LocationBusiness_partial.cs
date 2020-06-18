using Coldairarrow.Entity.PB;
using Coldairarrow.IBusiness.DTO;
using Coldairarrow.Util;
using EFCore.Sharding;
using LinqKit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Coldairarrow.Business.PB
{
    public partial class PB_LocationBusiness : BaseBusiness<PB_Location>, IPB_LocationBusiness, ITransientDependency
    {
        public async Task<List<PB_Location>> GetQueryData(SelectQueryDTO search)
        {
            var q = GetIQueryable();
            var where = LinqHelper.False<PB_Location>();

            if (!search.Keyword.IsNullOrEmpty())
                where = where.Or(w => w.Name.Contains(search.Keyword) || w.Code.Contains(search.Keyword));

            if (!search.Id.IsNullOrEmpty())
                where = where.Or(w => w.Id == search.Id);
            return await q.Where(where).OrderBy(o => o.Name).Take(search.Take).ToListAsync();
        }

        public async Task<PB_Location> GetDefaultLocal(string storId, string storAreaId)
        {
            var q = GetIQueryable();

            return await q.Where(p => p.StorId == storId && p.AreaId == storAreaId && p.IsDefault == true).FirstOrDefaultAsync();
        }
        public async Task<PageResult<PB_Location>> GetDataListAsync(PageInput<PB_LocationQM> input)
        {
            var q = GetIQueryable();
            var where = LinqHelper.True<PB_Location>();
            var search = input.Search;
            q = q.Include(i => i.PB_Storage).Include(i => i.PB_Laneway).Include(i => i.PB_StorArea).Include(i => i.PB_Rack);

            if (!search.StorName.IsNullOrEmpty())
            {
                where = where.And(p => p.StorId == search.StorName);
            }

            if (!search.Keyword.IsNullOrEmpty())
                where = where.And(w => w.Code.Contains(search.Keyword) || w.Name.Contains(search.Keyword) || w.PB_Rack.Name.Contains(search.Keyword) || w.PB_Rack.Code.Contains(search.Keyword) || w.PB_Laneway.Name.Contains(search.Keyword) || w.PB_Laneway.Code.Contains(search.Keyword));
            //if (!search.StorName.IsNullOrEmpty())
            //    where = where.And(w => w.PB_Storage.Name.Contains(search.StorName) || w.PB_Storage.Code.Contains(search.StorName));
            if (!search.AreaName.IsNullOrEmpty())
                where = where.And(w => w.PB_StorArea.Name.Contains(search.AreaName) || w.PB_StorArea.Code.Contains(search.AreaName));
            return await q.Where(where).GetPageResultAsync(input);
        }

        public async Task<List<PB_Location>> QueryAsync()
        {
            var q = GetIQueryable();

            return await q.ToListAsync();
        }
    }
}