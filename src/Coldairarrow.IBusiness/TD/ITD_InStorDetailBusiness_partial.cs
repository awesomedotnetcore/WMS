using Coldairarrow.Entity.TD;
using Coldairarrow.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.TD
{
    public partial interface ITD_InStorDetailBusiness
    {
        Task AddDataAsync(List<TD_InStorDetail> list);
        Task UpdateDataAsync(List<TD_InStorDetail> list);
    }
}