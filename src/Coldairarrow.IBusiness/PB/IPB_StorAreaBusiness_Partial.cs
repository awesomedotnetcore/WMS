using Coldairarrow.Entity.PB;
using Coldairarrow.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.PB
{
    public partial interface IPB_StorAreaBusiness
    {
        Task<List<PB_StorArea>> QueryAsync(string storageId);
        Task<PB_StorArea> GetInnerArea(string storageId);
    }
}