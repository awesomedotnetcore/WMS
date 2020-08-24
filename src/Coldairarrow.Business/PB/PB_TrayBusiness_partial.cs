using Coldairarrow.Entity.IT;
using Coldairarrow.Entity.PB;
using Coldairarrow.IBusiness.DTO;
using Coldairarrow.Util;
using EFCore.Sharding;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Coldairarrow.Business.PB
{
    public partial class PB_TrayBusiness : BaseBusiness<PB_Tray>, IPB_TrayBusiness, ITransientDependency
    {
        public PB_TrayBusiness(IDbAccessor db, IServiceProvider svcProvider, IServiceProvider provider)
            : base(db)
        {
            _ServiceProvider = svcProvider;
            _provider = provider;
        }
        readonly IServiceProvider _ServiceProvider;
        IServiceProvider _provider { get; }

        public async Task<PageResult<PB_Tray>> GetDataListAsync(PageInput<PB_TrayQM> input)
        {
            var q = GetIQueryable();
            var search = input.Search;
            q = q.Include(i => i.PB_TrayType).Include(i => i.PB_Location);

            //筛选
            if (!search.Keyword.IsNullOrEmpty())
                q = q.Where(w => w.Name.Contains(search.Keyword) || w.Code.Contains(search.Keyword));
            if (!search.TypeName.IsNullOrEmpty())
                q = q.Where(w => w.PB_TrayType.Name.Contains(search.TypeName) || w.PB_TrayType.Code.Contains(search.TypeName));

            return await q.GetPageResultAsync(input);
        }
        public async Task<List<PB_Tray>> GetQueryData(TraySelectQueryDTO search)
        {
            var q = GetIQueryable();
            var where = LinqHelper.True<PB_Tray>();

            var listTypeId = new List<string>();
            if (!search.MaterialId.IsNullOrEmpty())
            {
                var query = Db.GetIQueryable<PB_TrayMaterial>();
                var listType = await query.Where(w => w.MaterialId == search.MaterialId).Select(s => s.TrayTypeId).Distinct().ToListAsync();
                listTypeId.AddRange(listType);
            }
            if (!search.LocationId.IsNullOrEmpty())
            {
                var query = Db.GetIQueryable<PB_LocalTray>();
                var listType = await query.Where(w => w.LocalId == search.LocationId).Select(s => s.TrayTypeId).Distinct().ToListAsync();
                listTypeId.AddRange(listType);
            }
            if (listTypeId.Count > 0)
                where = where.And(w => listTypeId.Contains(w.TrayTypeId));

            if (!search.Keyword.IsNullOrEmpty())
                where = where.And(w => w.Name.Contains(search.Keyword) || w.Code.Contains(search.Keyword));

            var result = await q.Where(where).OrderBy(o => o.Name).Take(search.Take).ToListAsync();
            if (!search.Id.IsNullOrEmpty())
            {
                var one = await this.GetIQueryable().Where(w => w.Id == search.Id).SingleOrDefaultAsync();
                result.Add(one);
            }
            return result;
        }

        public async Task<(PB_Location Local, PB_Tray Tray)> ReqBlankTray(string storId, string typeId)
        {
            var lmTrayId = from lm in Db.GetIQueryable<IT_LocalMaterial>()
                           join l in Db.GetIQueryable<PB_Location>() on lm.LocalId equals l.Id
                           where l.StorId == storId && l.LockType == 0 && l.IsForbid == false
                           select lm.TrayId;
            var listTrayId = from t in Db.GetIQueryable<PB_Tray>()
                             join l in Db.GetIQueryable<PB_Location>() on t.LocalId equals l.Id
                             where t.Status == 1
                             && t.TrayTypeId == typeId
                             && l.StorId == storId
                             && l.LockType == 0
                             && !lmTrayId.Contains(t.Id)
                             select new { Local = l, Tray=t };
           // if (listTrayId == null) return new AjaxResult<(PB_Tray)>() { Success = false, Msg = "可用库存不足" };

            var resut = await listTrayId.FirstOrDefaultAsync();
            return (resut.Local, resut.Tray);  
        }

        public async Task<List<PB_Tray>> GetByLocation(string traytypeId)
        {
            var q = GetIQueryable();
            var where = LinqHelper.True<PB_Tray>();
            var result = await q.Where(where).OrderBy(o => o.TrayTypeId).Distinct().ToListAsync();
            return result;

            //await this.GetIQueryable().SingleOrDefaultAsync(w => w.TrayTypeId == traytypeId);
            // result.Add(one);
        }

        //public async Task<PB_Tray> GetByTrayId(string traycode)
        //{
        //    return await this.GetIQueryable().SingleOrDefaultAsync(w => w.Code == traycode);
        //}

        //  public async Task<PB_Tray> InNulltray(string trayId)
        // {
        //  var traySvc = this._provider.GetRequiredService<IPB_TrayBusiness>();
        //  var local = await traySvc.GetTheDataAsync(trayId);


        //var tray = await traySvc.GetByTrayId(traycode);

        //var localIds = local.Select(s => s.Id).ToList();

        //var listLocal = await Db.GetIQueryable<PB_Location>().Where(w => w.Id == w.Id).ToListAsync();
        //foreach (var item in listLocal)
        //{
        //    item.LockType = 2;
        //}
        // var localSvc = _ServiceProvider.GetRequiredService<IPB_LocationBusiness>();
        // await localSvc.UpdateDataAsync(listLocal);
        //return 
        //  }

        [DataAddLog(UserLogType.托盘管理, "Code", "托盘名称")]
        [DataRepeatAndValidate(new string[] { "TrayTypeId", "Code" }, new string[] { "托盘类型", "托盘编号" })]
        public async Task AddDataAsync(PB_Tray data)
        {
            await InsertAsync(data);
        }
        public async Task AddDataAsync(List<PB_Tray> list)
        {
            await InsertAsync(list);
        }

        public async Task UpdateDataAsync(List<PB_Tray> list)
        {
            await UpdateAsync(list);
        }
    }
}