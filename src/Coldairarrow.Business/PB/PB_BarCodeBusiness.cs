using Coldairarrow.Entity.PB;
using Coldairarrow.Util;
using EFCore.Sharding;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Coldairarrow.Business.PB
{
    public class PB_BarCodeBusiness : BaseBusiness<PB_BarCode>, IPB_BarCodeBusiness, ITransientDependency
    {
        public PB_BarCodeBusiness(IDbAccessor db)
            : base(db)
        {
        }

        #region 外部接口

        public async Task<PageResult<PB_BarCode>> GetDataListAsync(PageInput<PB_BarCodeQM> input)
        {
            var q = GetIQueryable();
            var where = LinqHelper.True<PB_BarCode>();
            var search = input.Search;
            q = q.Include(i => i.BarCodeType);

            //筛选
            if (!search.BarCode.IsNullOrEmpty())
                where = where.And(w => w.BarCode.Contains(search.BarCode));

            if (!search.BarCodeTypeName.IsNullOrEmpty())
                where = where.And(w => w.BarCodeType.Code.Contains(search.BarCodeTypeName) || w.BarCodeType.Name.Contains(search.BarCodeTypeName));

            return await q.Where(where).GetPageResultAsync(input);
        }

        public async Task<PB_BarCode> GetTheDataAsync(string id)
        {
            return await GetEntityAsync(id);
        }

        public async Task AddDataAsync(PB_BarCode data)
        {
            await InsertAsync(data);
        }

        public async Task UpdateDataAsync(PB_BarCode data)
        {
            await UpdateAsync(data);
        }

        public async Task DeleteDataAsync(List<string> ids)
        {
            await DeleteAsync(ids);
        }

        #endregion

        #region 私有成员

        #endregion
    }
}