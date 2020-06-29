using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Coldairarrow.IBusiness.Report
{
    public interface IReport_IntroduceBussiness
    {
        Task<IntroduceDTO> GetMaterialSummary(DateTime date, string storId);
        Task<IntroduceDTO> GetInStorageSummary(DateTime date, string storId);
        Task<IntroduceDTO> GetOutStorageSummary(DateTime date, string storId);
        Task<List<IntroduceHistoryDTO>> GetInStorageList(DateTime date, string storId);
        Task<List<IntroduceHistoryDTO>> GetOutStorageList(DateTime date, string storId);
    }
    public class IntroduceDTO
    {
        public double Total { get; set; }
        public double Storage { get; set; }
    }
    public class IntroduceHistoryDTO
    {
        public string Name { get; set; }
        public double Day0 { get; set; }
        public double Day1 { get; set; }
        public double Day2 { get; set; }
        public double Day3 { get; set; }
        public double Day4 { get; set; }
        public double Day5 { get; set; }
        public double Day6 { get; set; }
    }
}
