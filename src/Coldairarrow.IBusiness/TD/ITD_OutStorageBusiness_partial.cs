using Coldairarrow.Entity.TD;
using Coldairarrow.Util;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.TD
{
    public partial interface ITD_OutStorageBusiness
    {
        Task<PageResult<TD_OutStorage>> GetDataListAsync(TD_OutStoragePageInput input);
    }
}