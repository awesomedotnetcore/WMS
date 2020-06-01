using Coldairarrow.Entity.IT;
using Coldairarrow.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.IT
{
    public partial interface IIT_RecordBookBusiness
    {
        Task<PageResult<IT_RecordBook>> GetDataListAsync(PageInput<ConditionDTO> input);
        Task<IT_RecordBook> GetTheDataAsync(string id);
        Task AddDataAsync(IT_RecordBook data);
        Task UpdateDataAsync(IT_RecordBook data);
        Task DeleteDataAsync(List<string> ids);
    }
}