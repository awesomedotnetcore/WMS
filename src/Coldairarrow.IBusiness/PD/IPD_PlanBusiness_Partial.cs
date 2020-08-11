using System.Threading.Tasks;

namespace Coldairarrow.Business.PD
{
    public partial interface IPD_PlanBusiness
    {
        Task Status(string id, int status);
    }
}