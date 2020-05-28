using Coldairarrow.Entity.PB;
using Coldairarrow.Entity.TD;
using Coldairarrow.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.TD
{
    public partial interface ITD_CheckMaterialBusiness
    {
        Task ClearDataAsync(string checkId);

        Task PushAsync(List<TD_CheckMaterial> data);

        Task<List<PB_Material>> QueryAsync(string checkId);
    }
}