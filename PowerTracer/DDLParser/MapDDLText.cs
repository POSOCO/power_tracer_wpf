using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PowerTracer.DDLParser
{
    class MapDDLText
    {
        public Point origin { get; set; } = new Point(Double.NaN, Double.NaN);
        public string gab { get; set; }
        public string localize { get; set; }
    }
}
