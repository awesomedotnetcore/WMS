using Coldairarrow.Entity.IT;
using Coldairarrow.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.IT
{
    public partial interface IIT_LocalDetailBusiness
    {
        Task AddDataAsync(List<IT_LocalDetail> list);
        Task UpdateDataAsync(List<IT_LocalDetail> list);
    }
}