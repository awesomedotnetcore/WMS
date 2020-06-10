using Coldairarrow.Entity.PB;
using Coldairarrow.Util;
using EFCore.Sharding;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Coldairarrow.Business.PB
{
    public partial class PB_BarCodeSerialBusiness : BaseBusiness<PB_BarCodeSerial>, IPB_BarCodeSerialBusiness, ITransientDependency
    {
        public async Task AddDataAsync(List<PB_BarCodeSerial> list)
        {
            await InsertAsync(list);
        }

        public async Task UpdateDataAsync(List<PB_BarCodeSerial> list)
        {
            await UpdateAsync(list);
        }
    }
}