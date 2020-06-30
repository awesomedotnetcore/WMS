using System;
using System.Collections.Generic;
using System.Text;

namespace Coldairarrow.IBusiness.DTO
{
    public class TDCheckQueryDTO
    {
        public TDCheckQueryDTO()
        {
            IsComplete = -1;
            Status = -1;
        }

        public string Type { set; get; }

        public int IsComplete { set; get; }

        public int Status { set; get; }

        public string RefCode { set; get; }

        public string[] RangeDate { set; get; }
    }
}
