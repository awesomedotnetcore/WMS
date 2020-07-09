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
    public partial class TD_RecDetailBusiness : BaseBusiness<TD_RecDetail>, ITD_RecDetailBusiness, ITransientDependency
    {
        public async Task AddDataAsync(List<TD_RecDetail> list)
        {
            await InsertAsync(list);
        }

        public async Task UpdateDataAsync(List<TD_RecDetail> list)
        {
            await UpdateAsync(list);
        }
    }
}