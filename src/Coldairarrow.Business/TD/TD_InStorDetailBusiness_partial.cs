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
    public partial class TD_InStorDetailBusiness : BaseBusiness<TD_InStorDetail>, ITD_InStorDetailBusiness, ITransientDependency
    {
        public async Task AddDataAsync(List<TD_InStorDetail> list)
        {
            await InsertAsync(list);
        }

        public async Task UpdateDataAsync(List<TD_InStorDetail> list)
        {
            await UpdateAsync(list);
        }

        public async Task<PageResult<TD_InStorDetail>> GetDataListAsync(TD_InStorDetailPageInput input)
        {
            var queryable = this.GetIQueryable()
                .Include(i => i.InStorage)
                .Include(i => i.Location)
                .Include(i => i.Tray)
                .Include(i => i.TrayZone)
                .Include(i => i.Material)
                .Where(w => w.StorId == input.StorId && w.InStorage.Status == 1);
            var search = input.Search;
            var where = LinqHelper.True<TD_InStorDetail>();
            if (!search.Code.IsNullOrEmpty())
                where = where.And(w => w.InStorage.Code.Contains(search.Code) || w.InStorage.RefCode.Contains(search.Code));
            if (!search.InType.IsNullOrEmpty())
                where = where.And(w => w.InStorage.InType == search.InType);
            if (search.InTimeStart.HasValue)
                where = where.And(w => w.InStorage.InStorTime >= search.InTimeStart.Value);
            if (search.InTimeEnd.HasValue)
                where = where.And(w => w.InStorage.InStorTime >= search.InTimeEnd.Value);
            if (!search.LocalName.IsNullOrEmpty())
                where = where.And(w => w.Location.Name.Contains(search.LocalName) || w.Location.Code.Contains(search.LocalName));
            if (!search.MaterialName.IsNullOrEmpty())
                where = where.And(w => w.Material.Name.Contains(search.MaterialName) || w.Material.Code.Contains(search.MaterialName) || w.Material.SimpleName.Contains(search.MaterialName) || w.Material.BarCode.Contains(search.MaterialName) || w.BarCode.Contains(search.MaterialName) || w.BatchNo.Contains(search.MaterialName));
            return await queryable.Where(where).GetPageResultAsync(input);
        }
    }
}