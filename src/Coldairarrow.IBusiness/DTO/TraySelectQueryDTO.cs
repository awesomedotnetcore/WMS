using System;
using System.Collections.Generic;
using System.Text;

namespace Coldairarrow.IBusiness.DTO
{
    public class TraySelectQueryDTO : SelectQueryDTO
    {
        public string MaterialId { get; set; }
        public string LocationId { get; set; }
    }
}
