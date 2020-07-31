using Coldairarrow.Entity.PB;
using Coldairarrow.Util;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Coldairarrow.Business.PB
{
    public partial interface IPB_FeedPointBusiness
    {
        Task<PageResult<PB_FeedPoint>> GetDataListAsync(PageInput<PB_FeedPointQM> input);
        Task Enable(string id, bool enable);
        IQueryable<T> GetQueryable<T>() where T : class, new();
    }
    public class PB_FeedPointQM
    {
        public string Keyword { get; set; }
        public string StorId { get; set; }
        public string Type { get; set; }
    }
}