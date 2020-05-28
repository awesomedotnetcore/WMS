﻿using Coldairarrow.Entity.PB;
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
    public partial class PB_TrayTypeBusiness : BaseBusiness<PB_TrayType>, IPB_TrayTypeBusiness, ITransientDependency
    {
        /// <summary>
        /// 根据物料，找到对应的托盘类型
        /// </summary>
        /// <param name="materialId">物料ID</param>
        /// <returns></returns>
        public async Task<List<string>> GetByMaterial(string materialId)
        {
            var query = Service.GetIQueryable<PB_TrayMaterial>();
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
            var query = Service.GetIQueryable<PB_LocalTray>();
            var listType = await query.Where(w => w.LocalId == locationId).Select(s => s.TrayTypeId).Distinct().ToListAsync();
            return listType;
        }
    }
}