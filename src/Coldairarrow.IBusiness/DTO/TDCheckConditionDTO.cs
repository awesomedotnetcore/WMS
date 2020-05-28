using Coldairarrow.Entity.TD;
using System;
using System.Collections.Generic;
using System.Text;

namespace Coldairarrow.IBusiness.DTO
{
    public class TDCheckConditionDTO
    {
        public TD_Check Data { set; get; }

        public List<string> AreaIdList { set; get; }

        public List<string> MaterialIdList { set; get; }
    }
}
