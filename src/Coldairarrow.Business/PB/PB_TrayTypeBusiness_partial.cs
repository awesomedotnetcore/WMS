using Coldairarrow.Entity.PB;
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
    public partial class PB_TrayTypeBusiness : BaseBusiness<PB_TrayType>, IPB_TrayTypeBusiness, ITransientDependency
    {
        public PB_TrayTypeBusiness(IDbAccessor db, IServiceProvider svcProvider)
            : base(db)
        {
            _ServiceProvider = svcProvider;
        }
        readonly IServiceProvider _ServiceProvider;

        [DataAddLog(UserLogType.托盘类型, "Name", "托盘类型")]
        [DataRepeatValidate(new string[] { "Code", "Name" }, new string[] { "编号", "名称" })]
        public async Task AddDataAsync(PB_TrayType data)
        {
            if (data.Code.IsNullOrEmpty())
            {
                var codeSvc = _ServiceProvider.GetRequiredService<IPB_BarCodeTypeBusiness>();
                data.Code = await codeSvc.Generate("PB_TrayType");
            }
            await InsertAsync(data);
        }
        /// <summary>
        /// 根据物料，找到对应的托盘类型
        /// </summary>
        /// <param name="materialId">物料ID</param>
        /// <returns></returns>
        public async Task<List<string>> GetByMaterial(string materialId)
        {
            var query = Db.GetIQueryable<PB_TrayMaterial>();
            var listType = await query.Where(w => w.MaterialId == materialId).Select(s => s.TrayTypeId).Distinct().ToListAsync();
            return listType;
        }

        /// <summary>
        /// 根据货位，找到对应的托盘类型
        /// </summary>
        /// <param name="locationId">货位ID</param>
        /// <returns></returns>
        public async Task<List<string>> GetByLocation(string locationId)
        {
            var query = Db.GetIQueryable<PB_LocalTray>();
            var listlocal = await query.Where(w => w.LocalId == locationId).Select(s => s.TrayTypeId).Distinct().ToListAsync();
            return listlocal;
        }

        /// <summary>
        /// 返回托盘类型编号
        /// </summary>
        /// <param name="typecode">类型编号</param>
        /// <returns></returns>
        public async Task<PB_TrayType> GetByTypeCode(string typecode)
        {
            return await this.GetIQueryable().SingleOrDefaultAsync(w => w.Id == typecode);
        }

        ///// <summary>
        ///// 根据托盘类型，找对应货位
        ///// </summary>
        ///// <param name="LocalId">货位ID</param>
        ///// <returns></returns>
        //public async Task<List<string>> GetByTrayType(string LocalId)
        //{
        //    var query = Db.GetIQueryable<PB_LocalTray>();
        //    var listType = await query.Where(w => w.LocalId == locationId).Select(s => s.TrayTypeId).Distinct().ToListAsync();
        //    return listType;
        //}
    }
}