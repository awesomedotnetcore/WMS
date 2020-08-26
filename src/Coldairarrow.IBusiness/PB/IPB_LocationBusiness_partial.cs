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
        Task<List<PB_Location>> GetQueryData(PB_LocationSelectQueryDTO search);
        Task<PB_Location> GetDefaultLocal(string storId, string storAreaId);
        IQueryable<T> GetQueryable<T>() where T : class, new();
        Task<PageResult<PB_Location>> GetDataListAsync(PageInput<PB_LocationQM> input);
        Task<List<PB_Location>> QueryAsync();

        Task UpdateDataAsync(List<PB_Location> list);

        Task<PB_Location> GetByCode(string storId, string code);
        Task<PB_Location> GetBylocal(string id);
    }
    public class PB_LocationSelectQueryDTO : SelectQueryDTO
    {
        public string StorId { get; set; }
    }
    public class PB_LocationQM
    {
        public string Keyword { get; set; }
        public string AreaName { get; set; }
        public string StorId { get; set; }
    }
}