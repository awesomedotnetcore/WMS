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
        public PB_TrayBusiness(IRepository repository, IServiceProvider svcProvider)
            : base(repository)
        {
            _ServiceProvider = svcProvider;
        }
        readonly IServiceProvider _ServiceProvider;
        public async Task<List<PB_Tray>> GetQueryData(TraySelectQueryDTO search)
        {
            var q = GetIQueryable();
            var where = LinqHelper.True<PB_Tray>();

            var listTypeId = new List<string>();
            if (!search.MaterialId.IsNullOrEmpty())
            {
                var query = Service.GetIQueryable<PB_TrayMaterial>();
                var listType = await query.Where(w => w.MaterialId == search.MaterialId).Select(s => s.TrayTypeId).Distinct().ToListAsync();
                listTypeId.AddRange(listType);
            }
            if (!search.LocationId.IsNullOrEmpty())
            {
                var query = Service.GetIQueryable<PB_LocalTray>();
                var listType = await query.Where(w => w.LocalId == search.LocationId).Select(s => s.TrayTypeId).Distinct().ToListAsync();
                listTypeId.AddRange(listType);
            }
            if (listTypeId.Count > 0)
                where = where.And(w => listTypeId.Contains(w.TrayTypeId));

            if (!search.Keyword.IsNullOrEmpty())
                where = where.And(w => w.Name.Contains(search.Keyword) || w.Code.Contains(search.Keyword));

            if (!search.Id.IsNullOrEmpty())
                where = where.Or(w => w.Id == search.Id);

            return await q.Where(where).OrderBy(o => o.Name).Take(search.Take).ToListAsync();
        }

        public async Task AddDataAsync(PB_Tray data)
        {
            if (data.Code.IsNullOrEmpty())
            {
                var type = await Service.GetIQueryable<PB_TrayType>().SingleAsync(w => w.Id == data.TrayTypeId);
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