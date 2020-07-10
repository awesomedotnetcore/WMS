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
    public partial class TD_SendDetailBusiness : BaseBusiness<TD_SendDetail>, ITD_SendDetailBusiness, ITransientDependency
    {

        #region 外部接口

        public async Task AddDataAsync(List<TD_SendDetail> list)
        {
            await InsertAsync(list);
        }

        public async Task UpdateDataAsync(List<TD_SendDetail> list)
        {
            await UpdateAsync(list);
        }
        #endregion


    }
}