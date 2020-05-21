using Coldairarrow.Entity.PB;
using Coldairarrow.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.PB
{
    public interface IPB_StorageBusiness
    {
        Task<PageResult<PB_Storage>> GetDataListAsync(PB_StoragePageInput input);
        Task<PB_Storage> GetTheDataAsync(string id);
        Task AddDataAsync(PB_Storage data);
        Task UpdateDataAsync(PB_Storage data);
        Task ModifyDefaultAsync(PB_Storage data);
        Task DeleteDataAsync(List<string> ids);

        Task<List<PB_Storage>> GetListAsync();
    }
    public class PB_StorageQM
    {
        public string Name { get; set; }
        public string Code { get; set; }
    }
    public class PB_StoragePageInput : PageInput<PB_StorageQM>
    {
        public string Id { get; set; }
    }
}