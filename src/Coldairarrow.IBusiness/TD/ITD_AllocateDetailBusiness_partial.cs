using Coldairarrow.Entity.TD;
using Coldairarrow.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.TD
{
    public partial interface ITD_AllocateDetailBusiness
    {
        Task AddDataAsync(List<TD_AllocateDetail> list);
        Task UpdateDataAsync(List<TD_AllocateDetail> list);
    }
}