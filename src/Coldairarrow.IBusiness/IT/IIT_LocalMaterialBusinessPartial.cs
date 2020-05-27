using Coldairarrow.Entity.IT;
using Coldairarrow.Entity.TD;
using Coldairarrow.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.IT
{
    public partial interface IIT_LocalMaterialBusiness
    {
        Task<List<IT_LocalMaterial>> LoadCheckDataByAreaIdAsync(string id);
    }
}