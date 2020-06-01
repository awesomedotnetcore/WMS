using Coldairarrow.Entity.TD;
using Coldairarrow.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.TD
{
    public partial interface ITD_OutStorDetailBusiness
    {
        Task AddDataAsync(List<TD_OutStorDetail> list);
        Task UpdateDataAsync(List<TD_OutStorDetail> list);
    }
}