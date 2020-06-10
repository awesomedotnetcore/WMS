using Coldairarrow.Entity.PB;
using Coldairarrow.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.PB
{
    public partial interface IPB_BarCodeSerialBusiness
    {
        Task AddDataAsync(List<PB_BarCodeSerial> list);
        Task UpdateDataAsync(List<PB_BarCodeSerial> list);
    }
}