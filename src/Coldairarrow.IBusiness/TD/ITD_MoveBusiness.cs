using Coldairarrow.Entity.TD;
using Coldairarrow.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.TD
{
    public interface ITD_MoveBusiness
    {
        Task<PageResult<TD_Move>> GetDataListAsync(PageInput<SearchCondition> input, string storageId);
        Task<List<TD_Move>> GetDataListAsync(List<string> ids);
        Task<TD_Move> GetTheDataAsync(string id);
        Task AddDataAsync(TD_Move data);
        Task UpdateDataAsync(TD_Move data);
        Task DeleteDataAsync(List<string> ids);
        Task ApproveDataAsync(string id, string userId);
        Task RejectDataAsync(string id, string userId);
        Task ApproveDatasAsync(List<string> ids, string userId);
        Task RejectDatasAsync(List<string> ids, string userId);
    }

    public class SearchCondition
    {
        public string Type { get; set; }
        public string Keyword { get; set; }
    }

    public class UpdateMoveInfo
    {
        public string moveId { get; set; }
        public List<double> localNums { get; set; }
        public List<double> amounts { get; set; }
    }
}