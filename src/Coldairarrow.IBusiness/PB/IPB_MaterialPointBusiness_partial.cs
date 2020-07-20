using Coldairarrow.Entity.PB;
using Coldairarrow.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.PB
{
    public partial interface IPB_MaterialPointBusiness
    {
        Task<PageResult<PB_MaterialPoint>> GetDataListAsync(PB_MaterialPointPageInput input);
    }
    public class PB_MaterialPointQM
    {
        public string MaterialName { get; set; }
    }
    public class PB_MaterialPointPageInput : PageInput<PB_MaterialPointQM>
    {
        public string PointId { get; set; }
    }
}