using Coldairarrow.Entity.TD;
using Coldairarrow.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.TD
{
    public partial interface ITD_CheckMaterialBusiness
    {
        Task<PageResult<TD_CheckMaterial>> GetDataListAsync(PageInput<ConditionDTO> input);
        Task<TD_CheckMaterial> GetTheDataAsync(string id);
        Task AddDataAsync(TD_CheckMaterial data);
        Task UpdateDataAsync(TD_CheckMaterial data);
        Task DeleteDataAsync(List<string> ids);
    }
}