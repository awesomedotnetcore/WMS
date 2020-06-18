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
    public partial class TD_MoveDetailBusiness : BaseBusiness<TD_MoveDetail>, ITD_MoveDetailBusiness, ITransientDependency
    {


        public async Task AddDataAsync(List<TD_MoveDetail> list)
        {
            await InsertAsync(list);
        }

        public async Task UpdateDataAsync(List<TD_MoveDetail> list)
        {
            await UpdateAsync(list);
        }
    }
}