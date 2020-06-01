using Coldairarrow.Entity.IT;
using Coldairarrow.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.IT
{
    public partial interface IIT_RecordBookBusiness
    {
        Task AddDataAsync(List<IT_RecordBook> list);
        Task UpdateDataAsync(List<IT_RecordBook> list);
    }
}