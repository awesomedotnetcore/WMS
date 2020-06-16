using Coldairarrow.Entity.PB;
using Coldairarrow.IBusiness.DTO;
using Coldairarrow.Util;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Coldairarrow.Business.PB
{
    public partial interface IPB_LocationBusiness
    {
        Task<List<PB_Location>> GetQueryData(SelectQueryDTO search, string storId);
        Task<PB_Location> GetDefaultLocal(string storId, string storAreaId);
        IQueryable<T> GetQueryable<T>() where T : class, new();
    }
}