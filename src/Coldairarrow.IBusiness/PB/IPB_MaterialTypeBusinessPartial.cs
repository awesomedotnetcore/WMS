using Coldairarrow.Entity.PB;
using Coldairarrow.IBusiness.DTO;
using Coldairarrow.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.PB
{
    public partial interface IPB_MaterialTypeBusiness
    {
        Task<List<MaterialTypeTreeDTO>> GetTreeDataListAsync(string parentId = null);

        Task<List<string>> GetChildrenIdsAsync(string typeId);
    }
}