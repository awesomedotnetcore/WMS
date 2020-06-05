using Coldairarrow.Entity.IT;
using Coldairarrow.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.IT
{
    public partial interface IIT_LocalDetailBusiness
    {
        Task<PageResult<IT_LocalDetail>> GetDataListAsync(PageInput<ConditionDTO> input);
        Task<IT_LocalDetail> GetTheDataAsync(string id);
        Task AddDataAsync(IT_LocalDetail data);
        Task UpdateDataAsync(IT_LocalDetail data);
        Task DeleteDataAsync(List<string> ids);
        //Task UpdateDetailAsync(IT_LocalDetail data);
    }
}