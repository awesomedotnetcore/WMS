using Coldairarrow.Entity.PB;
using Coldairarrow.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.PB
{
    public partial interface IPB_BarCodeTypeBusiness
    {
        Task<PageResult<PB_BarCodeType>> GetDataListAsync(PageInput<PB_BarCodeTypeQM> input);
        Task<PB_BarCodeType> GetTheDataAsync(string id);
        Task AddDataAsync(PB_BarCodeType data);
        Task UpdateDataAsync(PB_BarCodeType data);
        Task DeleteDataAsync(List<string> ids);
        
    }

    public class PB_BarCodeTypeQM
    {
        public string Keyword { get; set; }
    }
}