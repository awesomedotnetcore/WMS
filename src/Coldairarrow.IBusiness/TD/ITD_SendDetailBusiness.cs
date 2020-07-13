using Coldairarrow.Entity.TD;
using Coldairarrow.Util;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.TD
{
    public partial interface ITD_SendDetailBusiness
    {
        //Task<PageResult<TD_SendDetail>> GetDataListAsync(PageInput<ConditionDTO> input);
        Task<TD_SendDetail> GetTheDataAsync(string id);
        Task AddDataAsync(TD_SendDetail data);
        Task UpdateDataAsync(TD_SendDetail data);
        Task DeleteDataAsync(List<string> ids);
    }
    public class TD_SendDetailQM
    {
        public string Code { get; set; }
        public string Type { get; set; }
        public DateTime? OutTimeStart { get; set; }
        public DateTime? OutTimeEnd { get; set; }
        public string MateriaName { get; set; }
        public string MeasureName { get; set; }

    }
    public class TD_SendDetailPageInput : PageInput<TD_SendDetailQM>
    {
        public string StorId { get; set; }
    }
}