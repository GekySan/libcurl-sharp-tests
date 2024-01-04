using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Curly.LibCURL
{
    public class CurlResult
    {
        public string Response { get; set; }
        public string ResponseUrl { get; set; }
        public string IpAddress { get; set; }
        public long ResponseCode { get; set; }
    }
}
