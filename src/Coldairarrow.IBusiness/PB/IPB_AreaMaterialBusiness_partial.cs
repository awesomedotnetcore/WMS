using Coldairarrow.Entity.PB;
using Coldairarrow.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.PB
{
    public partial interface IPB_AreaMaterialBusiness
    {
        Task<PageResult<PB_AreaMaterial>> GetDataListAsync(PB_AreaMaterialPageInput input);
    }
    public class PB_AreaMaterialQM
    {
        public string MaterialName { get; set; }
    }
    public class PB_AreaMaterialPageInput : PageInput<PB_AreaMaterialQM>
    {
        public string AreaId { get; set; }
    }
}