using Coldairarrow.Entity.TD;
using Coldairarrow.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.TD
{
    public partial interface ITD_CheckAreaBusiness
    {
        Task<List<string>> QueryAsync(string checkId);

        Task ClearDataAsync(string checkId);

        Task PushAsync(List<TD_CheckArea> data);
    }
}