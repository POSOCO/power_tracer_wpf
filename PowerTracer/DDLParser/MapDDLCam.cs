using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerTracer.DDLParser
{
    class MapDDLCam
    {
        public bool isExportable { get; set; } = true;
        public string name { get; set; }
        public Dictionary<string, string> compositeKey { get; set; } = new Dictionary<string, string>();
    }
}
