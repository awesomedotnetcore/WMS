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
        public PB_TrayBusiness(IDbAccessor db, IServiceProvider svcProvider)
            : base(db)
        {
            _ServiceProvider = svcProvider;
        }
        readonly IServiceProvider _ServiceProvider;

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

        public async Task<List<string>> GetByLocation(string tarytypeId)
        {
            var query = Db.GetIQueryable<PB_Tray>();
            var listLocal = await query.Where(w => w.TrayTypeId == tarytypeId).Select(s => s.LocalId).Distinct().ToListAsync();
            return listLocal;
            //var listType = await this.GetIQueryable().SingleOrDefaultAsync(w => w.TrayTypeId == type);
            //return listType;
        }

        [DataAddLog(UserLogType.托盘管理, "Code", "托盘")]
        [DataRepeatAndValidate(new string[] { "TrayTypeId", "Code" }, new string[] { "托盘类型", "编码" })]
        public async Task AddDataAsync(PB_Tray data)
        {
            if (data.Code.IsNullOrEmpty())
            {
                var type = await Db.GetIQueryable<PB_TrayType>().SingleAsync(w => w.Id == data.TrayTypeId);
                var codeSvc = _ServiceProvider.GetRequiredService<IPB_BarCodeTypeBusiness>();
                var dic = new Dictionary<string, string>();
                dic.Add("TypeCode", type.Code);
                data.Code = await codeSvc.Generate("PB_Tray", dic);
            }
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