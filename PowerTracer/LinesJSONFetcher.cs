using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerTracer
{
    class LinesJSONFetcher
    {
        public List<PowerLineLayer> fetchLayers(string str)
        {
            // https://weblog.west-wind.com/posts/2012/Aug/30/Using-JSONNET-for-dynamic-JSON-parsing
            List<PowerLineLayer> layers = new List<PowerLineLayer>();
            if (str != null)
            {
                // todo deal with parsing errors
                dynamic jsonLayers = JValue.Parse(File.ReadAllText("lines_ddl.json"));
                foreach (dynamic jsonLayer in jsonLayers.layers)
                {
                    Console.WriteLine("\t" + jsonLayer.name);
                    foreach (dynamic layerLine in jsonLayer.polyLines)
                    {
                        Console.WriteLine("\t\t" + layerLine.ednaId);
                        foreach (dynamic linePoint in layerLine.points)
                        {
                            Console.WriteLine("\t\t\t (" + Convert.ToDouble(linePoint.x) + ", " + Convert.ToDouble(linePoint.y) + ")");
                        }
                    }
                }
            }

            return layers;
        }
    }
}
