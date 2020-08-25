using Coldairarrow.Entity.PB;
using Coldairarrow.IBusiness.DTO;
using Coldairarrow.Util;
using EFCore.Sharding;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Quartz.Util;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Coldairarrow.Business.PB
{
    public partial class PB_MaterialBusiness : BaseBusiness<PB_Material>, IPB_MaterialBusiness, ITransientDependency
    {
        public async Task<PB_Material> GetTheDataAsync(string id)
        {
            var q = this.GetIQueryable().Include(i => i.MaterialType).Include(i => i.Measure).Include(i => i.Supplier).Include(i => i.Customer);
            return await q.SingleOrDefaultAsync(w => w.Id == id);
        }
        public async Task<List<PB_Material>> GetQueryData(SelectQueryDTO search)
        {
            var q = GetIQueryable();
            var where = LinqHelper.True<PB_Material>();
            if (!search.Keyword.IsNullOrEmpty())
                where = where.And(w => w.Name.Contains(search.Keyword) || w.Code.Contains(search.Keyword) || w.BarCode.Contains(search.Keyword));

            var result = await q.Where(where).OrderBy(o => o.Name).Take(search.Take).ToListAsync();
            if (!search.Id.IsNullOrEmpty())
            {
                var one = await this.GetIQueryable().Where(w => w.Id == search.Id).SingleOrDefaultAsync();
                result.Add(one);
            }
            return result;
        }

        public async Task<PageResult<PB_Material>> QueryDataListAsync(PageInput<PBMaterialConditionDTO> input)
        {
            var q = GetIQueryable().Include(i => i.MaterialType).Include(i=>i.Measure);
            var where = LinqHelper.True<PB_Material>();
            var search = input.Search;

            if (!search.TypeId.IsNullOrWhiteSpace()) where = where.And(p => p.MaterialTypeId == search.TypeId);
            if (!search.ContactName.IsNullOrWhiteSpace()) where = where.And(p => p.Supplier.Name.Contains(search.ContactName) || p.Supplier.Code.Contains(search.ContactName) || p.Customer.Name.Contains(search.ContactName) || p.Customer.Code.Contains(search.ContactName));
            if (!search.Keyword.IsNullOrWhiteSpace()) where = where.And(p => p.Name.Contains(search.Keyword) || p.Code.Contains(search.Keyword) || p.BarCode.Contains(search.Keyword) || p.SimpleName.Contains(search.Keyword));


            return await q.Where(where).GetPageResultAsync(input);
        }

        //public async Task<List<PB_Material>> QueryAsync()
        //{
        //    var q = GetIQueryable();

        //    return await q.ToListAsync();
        //}

        public async Task<PB_Material> GetByBarcode(string code)
        {
            return await this.GetIQueryable().SingleOrDefaultAsync(w => w.BarCode == code);
        }        
    }
}