using Coldairarrow.Entity.PB;
using Coldairarrow.Util;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Coldairarrow.Business.PB
{
    public partial interface IPB_TrayBusiness
    {
        
        Task<PB_Tray> GetTheDataAsync(string id);
        Task AddDataAsync(PB_Tray data);
        Task UpdateDataAsync(PB_Tray data);
        Task EnableTheData(string id);
        Task DisableTheData(string id);
        Task DeleteDataAsync(List<string> ids);
        IQueryable<T> GetQueryable<T>() where T : class, new();
        Task AddDataExlAsync(List<PB_Tray> list);
    }
    
}