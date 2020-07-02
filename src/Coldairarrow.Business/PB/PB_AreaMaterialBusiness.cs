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
    public partial class PB_AreaMaterialBusiness : BaseBusiness<PB_AreaMaterial>, IPB_AreaMaterialBusiness, ITransientDependency
    {
        public PB_AreaMaterialBusiness(IDbAccessor db)
            : base(db)
        {
        }

        #region 外部接口

        public async Task<PageResult<PB_AreaMaterial>> GetDataListAsync(PageInput<ConditionDTO> input)
        {
            var q = GetIQueryable();
            var search = input.Search;
            q = q.Include(i => i.PB_Material);

            //筛选
            if (!search.Keyword.IsNullOrEmpty())
            {
                q = q.Where(w => w.AreaId == search.Keyword);
            }
            var res = await q.GetPageResultAsync(input);

            return res;
        }

        public async Task<List<PB_AreaMaterial>> GetDataListAsync(string areaId)
        {
            var q = GetIQueryable();
            q = q.Include(i => i.PB_Material);

            //筛选
            if (!areaId.IsNullOrEmpty())
            {
                q = q.Where(w => w.AreaId == areaId);
            }
            var res = await q.ToListAsync();

            return res;
        }

        public async Task<PB_AreaMaterial> GetTheDataAsync(string id)
        {
            return await GetEntityAsync(id);
        }

        public async Task AddDataAsync(PB_AreaMaterial data)
        {
             await InsertAsync(data);
        }

        public async Task AddDataAsync(PBAreaMateriaConditionDTO data)
        {
            var areaId = data.id;
            var targetKeys = data.keys;

            var list = await GetDataListAsync(areaId);
            var amlist = list.Select(t => t.MaterialId).ToList();

            var reault = targetKeys.Except(amlist);

            var addList = new List<PB_AreaMaterial>();

            foreach (var i in reault)
            {
                addList.Add(new PB_AreaMaterial()
                {
                    AreaId = areaId,
                    MaterialId = i
                });
            }
            await InsertAsync(addList);
        }

        public async Task<int> AddDataAsync(List<PB_AreaMaterial> datas)
        {
            return await InsertAsync(datas);
        }

        public async Task UpdateDataAsync(PB_AreaMaterial data)
        {
            await UpdateAsync(data);
        }

        public async Task<int> UpdateDataAsync(List<PB_AreaMaterial> datas)
        {
            return await UpdateAsync(datas);
        }

        //public async Task DeleteDataAsync(List<string> ids)
        //{
        //    await DeleteAsync(ids);
        //}

        public async Task DeleteDataAsync(string AreaId, List<string> materialIds)
        {

            foreach (var key in materialIds)
            {
                await Db.ExecuteSqlAsync(string.Format("delete from PB_AreaMaterial where AreaId='{0}' and MaterialId='{1}'", AreaId, key));
            }
        }

        #endregion

        #region 私有成员

        #endregion
    }
}