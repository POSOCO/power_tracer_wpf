using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PowerTracer.DDLParser
{
    class MapDDLPicture
    {
        public bool isExportable { get; set; } = true;
        public string name { get; set; }
        public Point origin { get; set; } = new Point(Double.NaN, Double.NaN);
        public string gab { get; set; }
        public MapDDLCam cam { get; set; }
        public string eDNAId { get; set; }
    }
}
