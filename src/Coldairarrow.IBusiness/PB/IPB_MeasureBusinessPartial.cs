using Coldairarrow.Entity.PB;
using Coldairarrow.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.PB
{
    public partial interface IPB_MeasureBusiness
    {
        Task<List<PB_Measure>> QueryAllDataAsync();
    }
}