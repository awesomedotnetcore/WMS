using Coldairarrow.Entity.TD;
using Coldairarrow.Util;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.TD
{
    public partial interface ITD_MoveDetailBusiness
    {
        Task AddDataAsync(List<TD_MoveDetail> list);
        Task UpdateDataAsync(List<TD_MoveDetail> list);
    }
}