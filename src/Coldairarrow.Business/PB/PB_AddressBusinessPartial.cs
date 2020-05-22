using Coldairarrow.Entity.PB;
using Coldairarrow.IBusiness.DTO;
using Coldairarrow.Util;
using EFCore.Sharding;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Quartz.Util;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Coldairarrow.Business.PB
{
    public partial class PB_AddressBusiness 
    {
        public async Task<PageResult<PB_Address>> QueryDataListAsync(PageInput<PBAddressConditionDTO> input)
        {
            var q = GetIQueryable();
            var where = LinqHelper.True<PB_Address>();
            var search = input.Search;

            //筛选
            if(!search.CusId.IsNullOrWhiteSpace())
            {
                where = where.And(p => p.CusId == search.CusId);
            }
            else if(!search.SupId.IsNullOrWhiteSpace())
            {
                where = where.And(p => p.SupId == search.SupId);
            }
            else
            {
                where = where.And(p => 1 == 0);
            }

            return await q.Where(where).GetPageResultAsync(input);
        }

        public async Task ModifyDefaultAsync(string Id)
        {
            var entity = await GetEntityAsync(Id);
            if(entity.IsDefault)
            {
                entity.IsDefault = false;
                await UpdateAsync(entity);
            }
            else
            {
                var defaultData = await GetIQueryable().Where(p => p.IsDefault == true && p.SupId==entity.SupId && p.CusId==entity.CusId).FirstOrDefaultAsync();
                if(defaultData!=null && !string.IsNullOrWhiteSpace(defaultData.Id))
                {
                    defaultData.IsDefault = false;
                    await UpdateAsync(defaultData);
                }

                entity.IsDefault = true;
                await UpdateAsync(entity);
            }
        }

        public async Task ModifyEnableAsync(string Id)
        {
            var entity = await GetEntityAsync(Id);
            if (entity.IsEnable)
            {
                entity.IsEnable = false;
                if (entity.IsDefault) entity.IsDefault = false;
            }
            else
            {
                entity.IsEnable = true;
            }

            await UpdateAsync(entity);
        }
    }
}