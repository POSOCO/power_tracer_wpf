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
        string gab;
        double diameter;
        public Point origin { get; set; }
        public MapDDLCam cam { get; set; }
        public string eDNAId { get; set; }
    }
}
