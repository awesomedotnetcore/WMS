using Coldairarrow.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace Coldairarrow.IBusiness.DTO
{
    public class MaterialTypeTreeDTO : TreeModel
    {
        public object children { get => Children; }
        public string title { get => Text; }
        public string value { get => Id; }
        public string key { get => Id; }
        public string Code { get; set; }
        public string Remark { get; set; }
    }
}
