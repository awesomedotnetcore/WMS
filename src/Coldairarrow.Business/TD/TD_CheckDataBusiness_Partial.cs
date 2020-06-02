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
    public partial class TD_CheckDataBusiness : BaseBusiness<TD_CheckData>, ITD_CheckDataBusiness, ITransientDependency
    {
        public async Task ClearDataAsync(string checkId)
        {
            await DeleteAsync(p => p.CheckId == checkId);
        }

        public async Task PushDataAsync(List<TD_CheckData> data)
        {
            await base.InsertAsync(data);
        }
    }
}