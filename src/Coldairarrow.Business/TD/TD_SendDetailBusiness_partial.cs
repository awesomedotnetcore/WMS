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
    public partial class TD_SendDetailBusiness : BaseBusiness<TD_SendDetail>, ITD_SendDetailBusiness, ITransientDependency
    {

        #region 外部接口

        public async Task AddDataAsync(List<TD_SendDetail> list)
        {
            await InsertAsync(list);
        }

        public async Task UpdateDataAsync(List<TD_SendDetail> list)
        {
            await UpdateAsync(list);
        }

        public async Task<PageResult<TD_SendDetail>> GetDataListAsync(TD_SendDetailPageInput input)
        {
            var queryable = this.GetIQueryable()             
                .Include(i => i.StorId)
                .Include(i => i.Material)
                .Where(w => w.StorId == input.StorId);
            var search = input.Search;
            var where = LinqHelper.True<TD_SendDetail>();
            if (!search.Code.IsNullOrEmpty())
                where = where.And(w => w.Send.Code.Contains(search.Code) || w.Send.RefCode.Contains(search.Code));
            if (!search.Type.IsNullOrEmpty())
                where = where.And(w => w.Send.Type == search.Type);
            if (search.OutTimeStart.HasValue)
                where = where.And(w => w.Send.SendTime >= search.OutTimeStart.Value);
            if (search.OutTimeEnd.HasValue)
                where = where.And(w => w.Send.SendTime >= search.OutTimeEnd.Value);
            //if (!search.LocalName.IsNullOrEmpty())
            //    where = where.And(w => w.Location.Name.Contains(search.LocalName) || w.Location.Code.Contains(search.LocalName));
            if (!search.MaterialName.IsNullOrEmpty())
                where = where.And(w => w.Material.Name.Contains(search.MaterialName) || w.Material.Code.Contains(search.MaterialName) || w.Material.SimpleName.Contains(search.MaterialName) || w.Material.BarCode.Contains(search.MaterialName) || w.BatchNo.Contains(search.MaterialName));
            return await queryable.Where(where).GetPageResultAsync(input);
        }
        #endregion


    }
}