using Coldairarrow.Entity.PB;
using Coldairarrow.IBusiness.DTO;
using Coldairarrow.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.PB
{
    public partial interface IPB_TrayBusiness
    {
        Task<PageResult<PB_Tray>> GetDataListAsync(PageInput<PB_TrayQM> input);
        Task<List<PB_Tray>> GetQueryData(TraySelectQueryDTO search);

        Task AddDataAsync(List<PB_Tray> list);
        Task UpdateDataAsync(List<PB_Tray> list);

        Task<PB_Tray> GetByCode(string code);
    }
    public class PB_TrayQM
    {
        public string Keyword { get; set; }
        public string TypeName { get; set; }
    }
}