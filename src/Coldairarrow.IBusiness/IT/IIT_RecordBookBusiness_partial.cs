using Coldairarrow.Entity.IT;
using Coldairarrow.Util;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.IT
{
    public partial interface IIT_RecordBookBusiness
    {
        Task AddDataAsync(List<IT_RecordBook> list);
        Task UpdateDataAsync(List<IT_RecordBook> list);

        Task<PageResult<IT_RecordBook>> GetDataListAsync(IT_RecordBookPageInput input);
    }

    public class IT_RecordBookQM
    {
        public string LocalName { get; set; }
        public string MaterialName { get; set; }
        public string RefCode { get; set; }
        public string Type { get; set; }
        public string BarCodeBatchNo { get; set; }
    }
    public class IT_RecordBookPageInput : PageInput<IT_RecordBookQM>
    {
        //public string StorId { get; set; }
    }
}