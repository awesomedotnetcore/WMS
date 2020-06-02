using Coldairarrow.Entity.TD;
using Coldairarrow.IBusiness.DTO;
using Coldairarrow.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.TD
{
    public partial interface ITD_CheckBusiness
    {
        Task<PageResult<TD_Check>> QueryDataListAsync(string storId, PageInput<TDCheckQueryDTO> input);
    }
}