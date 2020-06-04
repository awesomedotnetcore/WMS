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
    public partial class TD_BadDetailBusiness : BaseBusiness<TD_BadDetail>, ITD_BadDetailBusiness, ITransientDependency
    {


        public async Task AddDataAsync(List<TD_BadDetail> list)
        {
            await InsertAsync(list);
        }

        public async Task UpdateDataAsync(List<TD_BadDetail> list)
        {
            await UpdateAsync(list);
        }
    }
}