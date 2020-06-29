using Coldairarrow.Entity;
using Coldairarrow.Util;
using System.Threading.Tasks;

namespace Coldairarrow.Business.Cache
{
    public interface IBase_UserCache : IBaseCache<Base_UserDTO>
    {
        Task UpdateCacheAsync(string id, Base_UserDTO user);
    }
}
