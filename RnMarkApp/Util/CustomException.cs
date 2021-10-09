using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RnMarkApp.Util
{
    public class CustomException : Exception
    {
        public string BoxId { get; set; }

        public string SkuName { get; set; }

        public string Side { get; set; }

        public string ErrorMessage { get; set; }
    }
}
