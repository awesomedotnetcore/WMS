using Coldairarrow.Entity.IT;
using Coldairarrow.Util;
using EFCore.Sharding;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Coldairarrow.Business.IT
{
    public partial class IT_LocalDetailBusiness : BaseBusiness<IT_LocalDetail>, IIT_LocalDetailBusiness, ITransientDependency
    {
        public async Task AddDataAsync(List<IT_LocalDetail> list)
        {
            await InsertAsync(list);
        }

        public async Task UpdateDataAsync(List<IT_LocalDetail> list)
        {
            await UpdateAsync(list);
        }
    }
}