using Coldairarrow.Entity.IT;
using Coldairarrow.Entity.PB;
using Coldairarrow.Entity.TD;
using Coldairarrow.Util;
using EFCore.Sharding;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Coldairarrow.Business.IT
{
    public partial class IT_LocalMaterialBusiness
    {
        public async Task<List<IT_LocalMaterial>> LoadCheckDataByAreaIdAsync(string id)
        {
            var q = GetIQueryable();
            q = q.Include(i => i.Location).Include(i => i.Material);

            q = q.Where(p => p.Location.AreaId == id);

            return await q.ToListAsync();

        }

        public async Task<List<IT_LocalMaterial>> LoadCheckDataByMaterialAsync(string storId, List<string> ids)
        {
            var q = GetIQueryable();
            q = q.Include(i => i.Location).Include(i => i.Material);

            q = q.Where(p => p.StorId== storId && ids.Contains(p.MaterialId));

            return await q.ToListAsync();
        }

        public async Task<List<PB_Material>> LoadMaterialByRandomAsync(string storId,int per)
        {
            var q = GetIQueryable();
            q = q.Include(i => i.Material).Include(i => i.Tray);

            q = q.Where(p => p.StorId == storId);

            var total= await q.Select(u=>u.Material).Distinct().ToListAsync();
            var num = total.Count;
            var tackNum = Convert.ToInt32(num * (Convert.ToDouble(per) / 100.0));
            var tackList = new List<string>();
            Random rnd = new Random();

            int idx = 0;
            while(idx<tackNum)
            {
                var randomIdx = rnd.Next(0, num - 1);
                var item = total[randomIdx].Id;
                if(!tackList.Contains(item))
                {
                    tackList.Add(item);
                    idx += 1;
                }
            }

            return (from u in total where tackList.Contains(u.Id) select u).ToList();
        }
    }
}