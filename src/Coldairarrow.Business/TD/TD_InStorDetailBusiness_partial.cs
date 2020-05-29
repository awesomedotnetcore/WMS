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
    public partial class TD_InStorDetailBusiness : BaseBusiness<TD_InStorDetail>, ITD_InStorDetailBusiness, ITransientDependency
    {
        public async Task AddDataAsync(List<TD_InStorDetail> list)
        {
            await InsertAsync(list);
        }

        public async Task UpdateDataAsync(List<TD_InStorDetail> list)
        {
            await UpdateAsync(list);
        }
    }
}