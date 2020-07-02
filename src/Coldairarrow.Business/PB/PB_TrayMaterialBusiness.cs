using Coldairarrow.Entity.PB;
using Coldairarrow.IBusiness.DTO;
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
        public PB_TrayMaterialBusiness(IDbAccessor db)
            : base(db)
        {
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

        public async Task<List<PB_TrayMaterial>> GetDataListAsync(string typeId)
        {
            var q = GetIQueryable();
            q = q.Include(i => i.PB_Material);

            //筛选
            if (!typeId.IsNullOrEmpty())
            {
                q = q.Where(w => w.TrayTypeId == typeId);
            }
            var res = await q.ToListAsync();

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

        public async Task AddDataAsync(PBTrayMateriaConditionDTO data)
        {
            var typeId = data.id;
            var targetKeys = data.keys;

            var list = await GetDataListAsync(typeId);
            var amlist = list.Select(t => t.MaterialId).ToList();

            var reault = targetKeys.Except(amlist);

            var addList = new List<PB_TrayMaterial>();

            foreach (var i in reault)
            {
                addList.Add(new PB_TrayMaterial()
                {
                    TrayTypeId = typeId,
                    MaterialId = i
                });
            }
            await InsertAsync(addList);
        }

        public async Task<int> AddDataAsync(List<PB_TrayMaterial> datas)
        {
            return await InsertAsync(datas);
        }

        public async Task UpdateDataAsync(PB_TrayMaterial data)
        {
            await UpdateAsync(data);
        }

        public async Task<int> UpdateDataAsync(List<PB_TrayMaterial> datas)
        {
           return await UpdateAsync(datas);
        }

        public async Task DeleteDataAsync(string trayTypeId, List<string> materialIds)
        {
            
            foreach(var key in materialIds)
            {
                await Db.ExecuteSqlAsync(string.Format("delete from PB_TrayMaterial where TrayTypeId='{0}' and MaterialId='{1}'", trayTypeId, key));
            }
        }

        #endregion

        #region 私有成员

        #endregion
    }
}