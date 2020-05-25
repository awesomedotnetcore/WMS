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
    public class PB_TrayMaterialBusiness : BaseBusiness<PB_TrayMaterial>, IPB_TrayMaterialBusiness, ITransientDependency
    {
        public PB_TrayMaterialBusiness(IRepository repository)
            : base(repository)
        {
            repository.HandleSqlLog = System.Console.WriteLine;
        }

        #region 外部接口

        public async Task<PageResult<PB_TrayMaterial>> GetDataListAsync(PageInput<ConditionDTO> input)
        {
            var q = GetIQueryable();
            var search = input.Search;
            q = q.Include(i => i.PB_Material);

            //筛选
            if (!search.Keyword.IsNullOrEmpty())
            {
                q = q.Where(w => w.TrayTypeId == search.Keyword);
            }
            var res = await q.GetPageResultAsync(input);

            return res;
        }

        public async Task<PB_TrayMaterial> GetTheDataAsync(string id)
        {
            return await GetEntityAsync(id);
        }

        public async Task AddDataAsync(PB_TrayMaterial data)
        {
            await InsertAsync(data);
        }

        public async Task UpdateDataAsync(PB_TrayMaterial data)
        {
            await UpdateAsync(data);
        }

        public async Task DeleteDataAsync(string trayTypeId, List<string> materialIds)
        {
            var q = GetIQueryable();
            q = q.Where(w => w.TrayTypeId == trayTypeId && materialIds.Contains(w.MaterialId));
            var res = await q.ToListAsync();

            await DeleteAsync(res);
        }

        #endregion

        #region 私有成员

        #endregion
    }
}