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
    public partial class PB_LocalTrayBusiness : BaseBusiness<PB_LocalTray>, IPB_LocalTrayBusiness, ITransientDependency
    {
        public  PB_LocalTrayBusiness(IDbAccessor db)
            : base(db)
        {
        }

        #region 外部接口

        public async Task<PageResult<PB_LocalTray>> GetDataListAsync(PageInput<ConditionDTO> input)
        {
            var q = GetIQueryable();
            var search = input.Search;
            q = q.Include(i => i.PB_Location)
                    .ThenInclude(i=> i.PB_StorArea)
                .Include(i => i.PB_Location)
                    .ThenInclude(i => i.PB_Storage);

            //筛选
            if (!search.Keyword.IsNullOrEmpty())
            {
                q = q.Where(w => w.TrayTypeId == search.Keyword);
            }
            var res = await q.GetPageResultAsync(input);

            return res;
        }

        public async Task<List<PB_LocalTray>> GetDataListAsync(string trayId)
        {
            var q = GetIQueryable();
            q = q.Include(i => i.PB_Location);

            //筛选
            if (!trayId.IsNullOrEmpty())
            {
                q = q.Where(w => w.TrayTypeId == trayId);
            }
            var res = await q.ToListAsync();

            return res;
        }

        public async Task<PB_LocalTray> GetTheDataAsync(string id)
        {
            return await GetEntityAsync(id);
        }

        public async Task AddDataAsync(PB_LocalTray data)
        {
            await InsertAsync(data);
        }

        public async Task AddDataAsync(PBLocalTrayConditionDTO data)
        {
            var typeId = data.id;
            var targetKeys = data.keys;

            var list = await GetDataListAsync(typeId);
            var addList = new List<PB_LocalTray>();

            foreach (var i in targetKeys)
            {
                foreach (var s in list)
                {
                    if (i == s.LocalId)
                    {
                        list.Remove(s);
                    }
                }
                addList.Add(new PB_LocalTray()
                {
                    TrayTypeId = typeId,
                    LocalId = i
                });
            }
            await InsertAsync(addList);
        }

        public async Task<int> AddDataAsync(List<PB_LocalTray> datas)
        {
            return await InsertAsync(datas);
        }

        public async Task UpdateDataAsync(PB_LocalTray data)
        {
            await UpdateAsync(data);
        }

        //public async Task DeleteDataAsync(List<string> ids)
        //{
        //    await DeleteAsync(ids);
        //}

        public async Task<int> UpdateDataAsync(List<PB_LocalTray> datas)
        {
            return await UpdateAsync(datas);
        }

        public async Task DeleteDataAsync(string trayId, List<string> localIds)
        {

            foreach (var key in localIds)
            {
                await Db.ExecuteSqlAsync(string.Format("delete from PB_LocalTray where TrayTypeId='{0}' and localId='{1}'", trayId, key));
            }
        }

        #endregion

        #region 私有成员

        #endregion
    }
}