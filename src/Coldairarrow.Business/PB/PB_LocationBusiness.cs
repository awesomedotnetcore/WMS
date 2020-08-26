using Coldairarrow.Entity.PB;
using Coldairarrow.Util;
using EFCore.Sharding;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Coldairarrow.Business.PB
{
    public partial class PB_LocationBusiness : BaseBusiness<PB_Location>, IPB_LocationBusiness, ITransientDependency
    {
        public PB_LocationBusiness(IDbAccessor db)
            : base(db)
        {
        }

        #region 外部接口



        public async Task<List<PB_Location>> GetDataListAsync()
        {
            var q = GetIQueryable();

            return await q.ToListAsync();
        }

        public async Task<PB_Location> GetTheDataAsync(string id)
        {
            return await GetEntityAsync(id);
        }

        [DataAddLog(UserLogType.货位管理, "Name", "货位")]
        [DataRepeatValidate(new string[] { "StorId", "AreaId", "Code" }, new string[] { "仓库", "货区", "货位" }, allData: false, matchOr: false)]
        public async Task AddDataAsync(PB_Location data)
        {
            await InsertAsync(data);
        }

        [DataEditLog(UserLogType.货位管理, "Name", "货位")]
        [DataRepeatValidate(new string[] { "StorId", "AreaId", "Code" }, new string[] { "仓库", "货区", "货位" }, allData: false, matchOr: false)]
        public async Task UpdateDataAsync(PB_Location data)
        {
            await UpdateAsync(data);
        }

        [DataDeleteLog(UserLogType.货位管理, "Name", "货位")]
        public async Task DeleteDataAsync(List<string> ids)
        {
            await DeleteAsync(ids);
        }

        public async Task ModifyEnableAsync(string Id)
        {
            var entity = await GetEntityAsync(Id);
            if (entity.IsForbid)
            {
                entity.IsForbid = false;
                if (entity.IsDefault) entity.IsDefault = false;
            }
            else
            {
                entity.IsForbid = true;
            }

            await UpdateAsync(entity);
        }

        public async Task AddDataExlAsync(List<PB_Location> list)//void
        {
            await InsertAsync(list);   //  BulkInsert    InsertAsync    

        }

        public IQueryable<T> GetQueryable<T>() where T : class, new()
        {
            return Db.GetIQueryable<T>();
        }


        #endregion

        #region 私有成员

        #endregion
    }
}