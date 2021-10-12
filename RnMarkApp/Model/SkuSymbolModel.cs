using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RnMarkApp.Model
{
    public class SkuSymbolModel
    {
        public string SKU { get; set; }

        public string Side { get; set; }

        public string MessageSlot { get; set; }

        public List<string> SymbolList { get; set; }
    }
}
