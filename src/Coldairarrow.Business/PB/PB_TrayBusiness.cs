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
        
        #region 外部接口

        

        public async Task<PB_Tray> GetTheDataAsync(string id)
        {
            return await GetEntityAsync(id);
        }


        [DataEditLog(UserLogType.托盘管理, "Code", "托盘")]
        [DataRepeatValidate(new string[] { "TrayTypeId", "Code" }, new string[] { "托盘类型", "编码" }, allData: false, matchOr: false)]
        public async Task UpdateDataAsync(PB_Tray data)
        {
            await UpdateAsync(data);
        }

        public async Task EnableTheData(string id)
        {
            await UpdateAsync(w => w.Id == id, entity => { entity.Status = 1; });
        }

        public async Task DisableTheData(string id)
        {
            await UpdateAsync(w => w.Id == id, entity => { entity.Status = 0; });
        }

        [DataDeleteLog(UserLogType.托盘管理, "Code", "托盘")]
        public async Task DeleteDataAsync(List<string> ids)
        {
            await DeleteAsync(ids);
        }

        public async Task AddDataExlAsync(List<PB_Tray> list)//void
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