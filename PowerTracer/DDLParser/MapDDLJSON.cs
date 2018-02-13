using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PowerTracer.DDLParser
{
    class MapDDLJSON
    {
        public string name { get; set; } = "_unknown_";
        public List<MapDDLJSONLayer> layers { get; set; } = new List<MapDDLJSONLayer>();
    }

    class MapDDLJSONLayer
    {
        public string name { get; set; } = "_unknown_";
        public List<MapDDLJSONPolyline> lines { get; set; } = new List<MapDDLJSONPolyline>();
    }

    class MapDDLJSONPolyline
    {
        public string gab { get; set; }
        public string cam { get; set; }
        public string ednaId { get; set; }
        public List<MapJSONPoint> points { get; set; } = new List<MapJSONPoint>();
        public List<Dictionary<string, string>> meta { get; set; } = new List<Dictionary<string, string>>();
    }

    class MapJSONPoint
    {
        public double x { get; set; }
        public double y { get; set; }
        public MapJSONPoint(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
    }

}
