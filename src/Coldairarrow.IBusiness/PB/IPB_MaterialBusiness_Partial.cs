using Coldairarrow.Entity.PB;
using Coldairarrow.IBusiness.DTO;
using Coldairarrow.Util;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Coldairarrow.Business.PB
{
    public partial interface IPB_MaterialBusiness
    {
        Task<List<PB_Material>> GetQueryData(SelectQueryDTO search);

        Task<PageResult<PB_Material>> QueryDataListAsync(PageInput<PBMaterialConditionDTO> input);

        Task<PB_Material> GetByCode(string code);
        Task<PB_Material> GetByBarcode(string code);

        IQueryable<T> GetQueryable<T>() where T : class, new();
    }
}