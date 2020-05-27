using Coldairarrow.Entity.PB;
using Coldairarrow.IBusiness.DTO;
using Coldairarrow.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.PB
{
    public partial interface IPB_MaterialBusiness
    {
        Task<List<PB_Material>> GetQueryData(SelectQueryDTO search);

        Task<PageResult<PB_Material>> QueryDataListAsync(PageInput<PBMaterialConditionDTO> input);
    }
}