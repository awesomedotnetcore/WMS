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
    public partial class IT_RecordBookBusiness : BaseBusiness<IT_RecordBook>, IIT_RecordBookBusiness, ITransientDependency
    {
        public async Task AddDataAsync(List<IT_RecordBook> list)
        {
            await InsertAsync(list);
        }

        public async Task UpdateDataAsync(List<IT_RecordBook> list)
        {
            await UpdateAsync(list);
        }

        public async Task<PageResult<IT_RecordBook>> GetDataListAsync(IT_RecordBookPageInput input)
        {
            var q = GetIQueryable()
                .Include(i => i.FromLocation)
                .Include(i => i.ToLocation)
                .Include(i => i.FromStorage)
                .Include(i => i.ToStorage)
                .Include(i => i.Material)
                .Include(i => i.Measure);
            var where = LinqHelper.True<IT_RecordBook>();
            var search = input.Search;
            if (!search.LocalName.IsNullOrEmpty())
                where = where.And(w => w.FromLocation.Name.Contains(search.LocalName) || w.ToLocation.Name.Contains(search.LocalName) 
                                    || w.ToLocation.Code.Contains(search.LocalName) || w.ToLocation.Code.Contains(search.LocalName));
            if (!search.BarCodeBatchNo.IsNullOrEmpty())
                where = where.And(w => w.BatchNo.Contains(search.BarCodeBatchNo) || w.BarCode.Contains(search.BarCodeBatchNo));
            if (!search.MaterialName.IsNullOrEmpty())
                where = where.And(w => w.Material.Code.Contains(search.MaterialName) || w.Material.Name.Contains(search.MaterialName));
            if (!search.RefCode.IsNullOrEmpty())
                where = where.And(w => w.RefCode.Contains(search.RefCode));
            if (!search.Type.IsNullOrEmpty())
                where = where.And(w => w.Type.Contains(search.Type));


            return await q.Where(where).GetPageResultAsync(input);
        }
    }
}