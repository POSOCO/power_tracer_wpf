using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerTracer.DDLParser
{
    class MapDDLCam
    {
        public string name { get; set; }
        public IDictionary<String, String> compositeKey { get; set; }
    }
}
