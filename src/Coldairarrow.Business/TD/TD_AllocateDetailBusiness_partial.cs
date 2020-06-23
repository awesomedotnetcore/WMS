using Coldairarrow.Entity.TD;
using Coldairarrow.Util;
using EFCore.Sharding;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Coldairarrow.Business.TD
{
    public partial class TD_AllocateDetailBusiness : BaseBusiness<TD_AllocateDetail>, ITD_AllocateDetailBusiness, ITransientDependency
    {
        public async Task AddDataAsync(List<TD_AllocateDetail> list)
        {
            await InsertAsync(list);
        }

        public async Task UpdateDataAsync(List<TD_AllocateDetail> list)
        {
            await UpdateAsync(list);
        }
    }
}