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
        public async Task<List<PB_Location>> GetQueryData(PB_LocationSelectQueryDTO search)
        {
            var q = GetIQueryable();
            var where = LinqHelper.True<PB_Location>();
            where = where.AndIf(!search.StorId.IsNullOrEmpty(), w => w.StorId == search.StorId);
            where = where.AndIf(!search.Keyword.IsNullOrEmpty(), w => w.Name.Contains(search.Keyword) || w.Code.Contains(search.Keyword));
            var list = await q.Where(where).OrderBy(o => o.Name).Take(search.Take).ToListAsync();
            if (!search.Id.IsNullOrEmpty())
            {
                var one = await this.GetIQueryable().Where(w => w.Id == search.Id).SingleOrDefaultAsync();
                list.Add(one);
            }
            return list;
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

            if (!search.StorId.IsNullOrEmpty())
            {
                where = where.And(p => p.StorId == search.StorId);
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

        public async Task UpdateDataAsync(List<PB_Location> list)
        {
            await UpdateAsync(list);
        }

        public async Task<PB_Location> GetByCode(string storId, string code)
        {
            return await this.GetIQueryable().SingleOrDefaultAsync(w => w.StorId == storId && w.Code == code);
        }

        public async Task<PB_Location> GetBylocal(string id)
        {
           var rel =  await this.GetIQueryable().SingleOrDefaultAsync(w => w.Id == id);
            return rel;
        }
    }
}