using Coldairarrow.Entity.Base;
using Coldairarrow.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.Base
{
    public interface IBase_EnumBusiness
    {
        Task<PageResult<Base_Enum>> GetDataListAsync(PageInput<Base_EnumQM> input);
        Task<Base_Enum> GetTheDataAsync(string id);
        Task<Base_Enum> GetByCodeAsync(string code);
        Task AddDataAsync(Base_Enum data);
        Task UpdateDataAsync(Base_Enum data);
        Task DeleteDataAsync(List<string> ids);
    }

    public class Base_EnumQM
    {
        public string Keyword { get; set; }
    }
}