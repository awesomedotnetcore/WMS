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
    public partial class TD_InStorageBusiness : BaseBusiness<TD_InStorage>, ITD_InStorageBusiness, ITransientDependency
    {
        public TD_InStorageBusiness(IRepository repository)
            : base(repository)
        {
        }

        #region 外部接口

        public async Task AddDataAsync(TD_InStorage data)
        {
            await InsertAsync(data);
        }

        public async Task UpdateDataAsync(TD_InStorage data)
        {
            await UpdateAsync(data);
        }

        public async Task DeleteDataAsync(List<string> ids)
        {
            await DeleteAsync(ids);
        }

        #endregion

        #region 私有成员

        #endregion
    }
}