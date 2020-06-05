using Coldairarrow.Entity.IT;
using Coldairarrow.Util;
using EFCore.Sharding;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Coldairarrow.Business.IT
{
    public partial class IT_LocalDetailBusiness : BaseBusiness<IT_LocalDetail>, IIT_LocalDetailBusiness, ITransientDependency
    {
        public async Task AddDataAsync(List<IT_LocalDetail> list)
        {
            await InsertAsync(list);
        }

        public async Task UpdateDataAsync(List<IT_LocalDetail> list)
        {
            await UpdateAsync(list);
        }
        public async Task DeleteDataAsync(List<IT_LocalDetail> list)
        {
            await DeleteAsync(list);
        }

        public async Task<PageResult<IT_LocalDetail>> GetDataListAsync(IT_LocalDetailPageInput input)
        {
            var q = GetIQueryable()
                .Include(i => i.Location)
                .Include(i => i.Tray)
                .Include(i => i.TrayZone)
                .Include(i => i.Material)
                .Include(i => i.Measure);
            var where = LinqHelper.True<IT_LocalDetail>();
            where = where.And(w => w.StorId == input.StorId);
            var search = input.Search;
            if (!search.LocalName.IsNullOrEmpty())
                where = where.And(w => w.Location.Code.Contains(search.LocalName) || w.Location.Name.Contains(search.LocalName));
            if (!search.TrayName.IsNullOrEmpty())
                where = where.And(w => w.Tray.Code.Contains(search.TrayName) || w.Tray.Name.Contains(search.TrayName));
            if (!search.MaterialName.IsNullOrEmpty())
                where = where.And(w => w.Material.Code.Contains(search.MaterialName) || w.Material.Name.Contains(search.MaterialName));
            if (!search.Code.IsNullOrEmpty())
                where = where.And(w => w.BatchNo.Contains(search.Code) || w.BarCode.Contains(search.Code));

            if (search.InTimeStart.HasValue)
                where = where.And(w => w.InTime >= search.InTimeStart.Value);
            if (search.InTimeEnd.HasValue)
                where = where.And(w => w.InTime <= search.InTimeEnd.Value);


            return await q.Where(where).GetPageResultAsync(input);
        }
    }
}