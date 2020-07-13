using Coldairarrow.Entity.Base;
using Coldairarrow.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.Base
{
    public interface IBase_ParameterBusiness
    {
        Task<PageResult<Base_Parameter>> GetDataListAsync(PB_ParameterPageInput input);
        Task<Base_Parameter> GetTheDataAsync(string id);
        Task<Dictionary<string, string>> GetConfig();
        Task AddDataAsync(Base_Parameter data);
        Task UpdateDataAsync(Base_Parameter data);
        Task DeleteDataAsync(List<string> ids);
    }
    public class PB_ParameterQM
    {
        public string Keyword { get; set; }
    }
    public class PB_ParameterPageInput : PageInput<PB_ParameterQM>
    {
        public string Id { get; set; }
    }
}