using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PowerTracer.DDLParser
{
    class MapDDLPolyline
    {
        public Point origin { get; set; }
        public List<Point> points { get; set; }
        public string gab { get; set; }
        public MapDDLCam cam { get; set; }
        public string eDNAId { get; set; }
    }
}
