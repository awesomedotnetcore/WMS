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
    public partial class PB_TrayBusiness : BaseBusiness<PB_Tray>, IPB_TrayBusiness, ITransientDependency
    {
        public PB_TrayBusiness(IRepository repository)
            : base(repository)
        {

        }

        #region 外部接口

        public async Task<PageResult<PB_Tray>> GetDataListAsync(PageInput<ConditionDTO> input)
        {
            var q = GetIQueryable();
            var search = input.Search;
            q = q.Include(i => i.PB_TrayType).Include(i => i.PB_Location);

            //筛选
            if (!search.Keyword.IsNullOrEmpty())
            {
                q = q.Where(w => w.Name.Contains(search.Keyword) || w.Code.Contains(search.Keyword));
            }

            return await q.GetPageResultAsync(input);
        }

        public async Task<PB_Tray> GetTheDataAsync(string id)
        {
            return await GetEntityAsync(id);
        }

        public async Task AddDataAsync(PB_Tray data)
        {
            await InsertAsync(data);
        }

        public async Task UpdateDataAsync(PB_Tray data)
        {
            await UpdateAsync(data);
        }

        public async Task EnableTheData(string id)
        {
            await UpdateWhereAsync(w => w.Id == id, entity => { entity.Status = 1; });
        }

        public async Task DisableTheData(string id)
        {
            await UpdateWhereAsync(w => w.Id == id, entity => { entity.Status = 0; });
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