using Coldairarrow.Entity.PB;
using Coldairarrow.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.PB
{
    public partial interface IPB_FeedPointBusiness
    {
        Task<PageResult<PB_FeedPoint>> GetDataListAsync(PageInput<ConditionDTO> input);
        Task<PB_FeedPoint> GetTheDataAsync(string id);
        Task AddDataAsync(PB_FeedPoint data);
        Task UpdateDataAsync(PB_FeedPoint data);
        Task DeleteDataAsync(List<string> ids);
        Task AddDataExlAsync(List<PB_FeedPoint> list);
    }
}