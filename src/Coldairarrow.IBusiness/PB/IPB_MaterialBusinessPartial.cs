using Coldairarrow.Entity.PB;
using Coldairarrow.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.PB
{
    public partial interface IPB_MaterialBusiness
    {
        Task<List<PB_Material>> GetQueryData(string id, string keyword);
    }
}