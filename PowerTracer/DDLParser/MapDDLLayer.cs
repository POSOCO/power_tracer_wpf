using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerTracer.DDLParser
{
    class MapDDLLayer
    {
        public List<MapDDLPolyline> polylines { get; set; } = new List<MapDDLPolyline>();
        public List<MapDDLText> texts { get; set; } = new List<MapDDLText>();
        public List<MapDDLCircle> circles { get; set; } = new List<MapDDLCircle>();
    }
}
