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
    public partial class PB_MaterialPointBusiness : BaseBusiness<PB_MaterialPoint>, IPB_MaterialPointBusiness, ITransientDependency
    {
        public PB_MaterialPointBusiness(IDbAccessor db)
            : base(db)
        {
        }

        #region 外部接口

        public async Task<PageResult<PB_MaterialPoint>> GetDataListAsync(PageInput<ConditionDTO> input)
        {
            var q = GetIQueryable();
            var search = input.Search;
            q = q.Include(i => i.PB_Material);

            //筛选
            if (!search.Keyword.IsNullOrEmpty())
            {
                q = q.Where(w => w.PointId == search.Keyword);
            }
            var res = await q.GetPageResultAsync(input);

            return res;
        }

        public async Task<List<PB_MaterialPoint>> GetDataListAsync(string pointId)
        {
            var q = GetIQueryable();
            q = q.Include(i => i.PB_Material);

            //筛选
            if (!pointId.IsNullOrEmpty())
            {
                q = q.Where(w => w.PointId == pointId);
            }
            var res = await q.ToListAsync();

            return res;
        }

        public async Task<PB_MaterialPoint> GetTheDataAsync(string id)
        {
            return await GetEntityAsync(id);
        }

        public async Task AddDataAsync(PB_MaterialPoint data)
        {
            await InsertAsync(data);
        }

        public async Task AddDataAsync(PBMaterialPointConditionDTO data)
        {
            var pointId = data.id;
            var targetKeys = data.keys;

            var list = await GetDataListAsync(pointId);
            var amlist = list.Select(t => t.MaterialId).ToList();

            var reault = targetKeys.Except(amlist);

            var addList = new List<PB_MaterialPoint>();

            foreach (var i in reault)
            {
                addList.Add(new PB_MaterialPoint()
                {
                    PointId = pointId,
                    MaterialId = i
                });
            }
            await InsertAsync(addList);
        }

        public async Task<int> AddDataAsync(List<PB_MaterialPoint> datas)
        {
            return await InsertAsync(datas);
        }

        public async Task UpdateDataAsync(PB_MaterialPoint data)
        {
            await UpdateAsync(data);
        }

        public async Task<int> UpdateDataAsync(List<PB_MaterialPoint> datas)
        {
            return await UpdateAsync(datas);
        }

        //public async Task DeleteDataAsync(List<string> ids)
        //{
        //    await DeleteAsync(ids);
        //}

        public async Task DeleteDataAsync(string PointId, List<string> materialIds)
        {

            foreach (var key in materialIds)
            {
                await Db.ExecuteSqlAsync(string.Format("delete from PB_MaterialPoint where PointId='{0}' and MaterialId='{1}'", PointId, key));
            }
        }

        #endregion

        #region 私有成员

        #endregion
    }
}