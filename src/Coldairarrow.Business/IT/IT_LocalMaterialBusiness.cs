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
    public partial class IT_LocalMaterialBusiness : BaseBusiness<IT_LocalMaterial>, IIT_LocalMaterialBusiness, ITransientDependency
    {
        public IT_LocalMaterialBusiness(IDbAccessor db)
            : base(db)
        {
        }

        #region 外部接口

        public async Task<PageResult<IT_LocalMaterial>> GetDataListAsync(PageInput<ConditionDTO> input)
        {
            var q = GetIQueryable();
            var where = LinqHelper.True<IT_LocalMaterial>();
            var search = input.Search;

            //筛选
            if (!search.Condition.IsNullOrEmpty() && !search.Keyword.IsNullOrEmpty())
            {
                var newWhere = DynamicExpressionParser.ParseLambda<IT_LocalMaterial, bool>(
                    ParsingConfig.Default, false, $@"{search.Condition}.Contains(@0)", search.Keyword);
                where = where.And(newWhere);
            }

            return await q.Where(where).GetPageResultAsync(input);
        }

        public async Task<PageResult<IT_LocalMaterial>> GetDataListByMaterialId(PageInput<ConditionDTO> input)
        {
            var q = GetIQueryable();
            var search = input.Search;
            q = q.Include(i => i.Location).Include(i => i.Tray).Include(i => i.TrayZone).Include(i => i.Material).Include(i => i.Measure);

            //筛选
            if (!search.Keyword.IsNullOrEmpty())
            {
                q = q.Where(w => w.MaterialId == search.Keyword);
            }
            if (!search.Condition.IsNullOrEmpty())
            {
                q = q.Where(w => w.StorId == search.Condition);
            }

            return await q.GetPageResultAsync(input);
        }

        public async Task<IT_LocalMaterial> GetTheDataAsync(string id)
        {
            return await GetEntityAsync(id);
        }

        public async Task AddDataAsync(IT_LocalMaterial data)
        {
            await InsertAsync(data);
        }

        public async Task UpdateDataAsync(IT_LocalMaterial data)
        {
            await UpdateAsync(data);
        }

        public async Task DeleteDataAsync(List<string> ids)
        {
            await DeleteAsync(ids);
        }

        public async Task<IT_LocalMaterial> GetByLocalMaterial(string local)
        {
            //return await GetIQueryable.where(w=>local.Contains(w.Id));
            return await this.GetIQueryable().SingleOrDefaultAsync(w => w.LocalId == local);
        }

        #endregion

        #region 私有成员

        #endregion
    }
}