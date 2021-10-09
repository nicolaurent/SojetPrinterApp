using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RnMarkApp.Model
{
    public class WorkOrderModel
    {
        public string BoxId { get; set; }

        public string SkuName { get; set; }

        public string Side { get; set; }

        public string Weight { get; set; }

        public string Data1 { get; set; }

        public string Data2 { get; set; }

        public string Data3 { get; set; }

        public string Data4 { get; set; }

        public string Data5 { get; set; }

        public List<string> Symbols { get; set; }
    }
}
