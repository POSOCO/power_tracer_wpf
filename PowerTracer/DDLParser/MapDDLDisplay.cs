using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerTracer.DDLParser
{
    class MapDDLDisplay
    {
        public string name { get; set; }
        public List<MapDDLLayer> layers { get; set; } = new List<MapDDLLayer>();
    }
}
