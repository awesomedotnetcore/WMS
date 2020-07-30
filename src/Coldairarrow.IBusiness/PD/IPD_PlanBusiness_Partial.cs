using Coldairarrow.Entity.PD;
using Coldairarrow.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.PD
{
    public partial interface IPD_PlanBusiness
    {
        Task<List<PD_Plan>> QueryAllDataAsync();
        Task Status(string id, bool status);

        Task<PD_Plan> GetByCode(string code);
    }
}