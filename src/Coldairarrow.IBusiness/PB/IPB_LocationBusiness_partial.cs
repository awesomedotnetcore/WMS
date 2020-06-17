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
        Task<List<PB_Location>> GetQueryData(SelectQueryDTO search);
        Task<PB_Location> GetDefaultLocal(string storId, string storAreaId);
        IQueryable<T> GetQueryable<T>() where T : class, new();

        Task<PageResult<PB_Location>> GetDataListAsync(PageInput<PB_LocationQM> input);
    }

    public class PB_LocationQM
    {
        public string Keyword { get; set; }
        public string StorName { get; set; }
        public string AreaName { get; set; }
    }
}