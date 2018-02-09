using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PowerTracer.DDLParser
{
    class MapDDLCircle
    {
        public bool isExportable { get; set; } = true;
        public string gab { get; set; }
        public double diameter { get; set; }
        public Point origin { get; set; } = new Point(Double.NaN, Double.NaN);
        public MapDDLCam cam { get; set; }
        public string eDNAId { get; set; }
    }
}
