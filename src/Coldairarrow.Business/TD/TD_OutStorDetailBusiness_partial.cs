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
    public partial class TD_OutStorDetailBusiness : BaseBusiness<TD_OutStorDetail>, ITD_OutStorDetailBusiness, ITransientDependency
    {
        public async Task AddDataAsync(List<TD_OutStorDetail> list)
        {
            await InsertAsync(list);
        }

        public async Task UpdateDataAsync(List<TD_OutStorDetail> list)
        {
            await UpdateAsync(list);
        }

        public async Task<PageResult<TD_OutStorDetail>> GetDataListAsync(TD_OutStorDetailPageInput input)
        {
            var queryable = this.GetIQueryable()
                .Include(i => i.OutStorage)
                .Include(i => i.Location)
                .Include(i => i.Tray)
                .Include(i => i.TrayZone)
                .Include(i => i.Material)
                .Where(w => w.StorId == input.StorId && w.OutStorage.Status == 1);
            var search = input.Search;
            var where = LinqHelper.True<TD_OutStorDetail>();
            if (!search.Code.IsNullOrEmpty())
                where = where.And(w => w.OutStorage.Code.Contains(search.Code) || w.OutStorage.RefCode.Contains(search.Code));
            if (!search.OutType.IsNullOrEmpty())
                where = where.And(w => w.OutStorage.OutType == search.OutType);
            if (search.OutTimeStart.HasValue)
                where = where.And(w => w.OutStorage.OutTime >= search.OutTimeStart.Value);
            if (search.OutTimeEnd.HasValue)
                where = where.And(w => w.OutStorage.OutTime >= search.OutTimeEnd.Value);
            if (!search.LocalName.IsNullOrEmpty())
                where = where.And(w => w.Location.Name.Contains(search.LocalName) || w.Location.Code.Contains(search.LocalName));
            if (!search.MaterialName.IsNullOrEmpty())
                where = where.And(w => w.Material.Name.Contains(search.MaterialName) || w.Material.Code.Contains(search.MaterialName) || w.Material.SimpleName.Contains(search.MaterialName) || w.Material.BarCode.Contains(search.MaterialName) || w.BarCode.Contains(search.MaterialName) || w.BatchNo.Contains(search.MaterialName));
            return await queryable.Where(where).GetPageResultAsync(input);
        }
    }
}