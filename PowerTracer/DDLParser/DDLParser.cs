using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PowerTracer.DDLParser
{
    class DDLParser
    {
        public static Regex blockStartRegex_ = new Regex(@"^\s*(?<block_name>.+)\s*$", RegexOptions.Compiled);
        public static Regex displayBlockStartRegex_ = new Regex(@"^\s*display\s+""(?<display>.+)""\s*$", RegexOptions.Compiled);
        public static Regex layerBlockStartRegex_ = new Regex(@"^\s*simple_layer\s+""(?<layer>.+)""\s*$", RegexOptions.Compiled);
        public static Regex polylineBlockStartRegex_ = new Regex(@"^\s*polyline\s*$", RegexOptions.Compiled);
        public static Regex textBlockStartRegex_ = new Regex(@"^\s*text\s*$", RegexOptions.Compiled);
        public static Regex circleBlockStartRegex_ = new Regex(@"^\s*circle\s*$", RegexOptions.Compiled);
        public static Regex gabBlockStartRegex_ = new Regex(@"^\s*gab\s*\(\s*$", RegexOptions.Compiled);
        public static Regex camBlockStartRegex_ = new Regex(@"^\s*cam\s*""(?<cam>.+)""\s*$", RegexOptions.Compiled);
        public static Regex compositeKeyBlockStartRegex_ = new Regex(@"^\s*composite_key\s*$", RegexOptions.Compiled);
        public static Regex gabRegex_ = new Regex(@"^\s*gab\s+""(?<gab>.+)""\s*$", RegexOptions.Compiled);
        public static Regex originRegex_ = new Regex(@"^\s*origin\s*\((?<x_num>[\+-]?\s*\d+)\s+(?<y_num>[\+-]?\s*\d+)\s*\)\s*$", RegexOptions.Compiled);
        public static Regex pointRegex_ = new Regex(@"^\s*point\s*\((?<x_num>[\+-]?\s*\d+)\s+(?<y_num>[\+-]?\s*\d+)\s*\)\s*$", RegexOptions.Compiled);
        public static Regex recorKeyRegex_ = new Regex(@"^\s*record\s*\(""(?<record>.+)""\)\s+record_key\s*\(""(?<record_key>.+)""\)\s*$", RegexOptions.Compiled);
        public static Regex textStringRegex_ = new Regex(@"^\s*localize\s*""(?<text_content>.+)""\s*$", RegexOptions.Compiled);
        public static Regex diameterRegex_ = new Regex(@"^\s*diameter\s*\((?<diameter>\d+)\)\s*$", RegexOptions.Compiled);

        public static void parseDDLToJSON()
        {
            string currBlockType = "";
            foreach (string line in File.ReadLines("mapboard.ddl"))
            {
                // search for display start
                MatchCollection matches = displayBlockStartRegex_.Matches(line);
                if (matches.Count > 0)
                {
                    currBlockType = "display";
                    string displayName = matches[0].Groups["display"].Value;
                    Console.WriteLine("Display {0}", displayName);
                    continue;
                }
                // search for layer start
                matches = layerBlockStartRegex_.Matches(line);
                if (matches.Count > 0)
                {
                    currBlockType = "layer";
                    string layerName = matches[0].Groups["layer"].Value;
                    Console.WriteLine("\tlayer {0}", layerName);
                    continue;
                }
                // search for polyline start
                matches = polylineBlockStartRegex_.Matches(line);
                if (matches.Count > 0)
                {
                    currBlockType = "polyline";
                    Console.WriteLine("\t\tPolyline");
                    continue;
                }
                // search for origin
                matches = originRegex_.Matches(line);
                if (matches.Count > 0)
                {
                    string x_num = matches[0].Groups["x_num"].Value;
                    string y_num = matches[0].Groups["y_num"].Value;
                    Console.WriteLine("\t\t\tOrigin = {0}, {1}", x_num, y_num);
                    continue;
                }
                // search for point
                matches = pointRegex_.Matches(line);
                if (matches.Count > 0)
                {
                    string x_num = matches[0].Groups["x_num"].Value;
                    string y_num = matches[0].Groups["y_num"].Value;
                    Console.WriteLine("\t\t\tPoint = {0}, {1}", x_num, y_num);
                    continue;
                }
                // search for camBlock start
                matches = camBlockStartRegex_.Matches(line);
                if (matches.Count > 0)
                {
                    currBlockType = "cam";
                    string camName = matches[0].Groups["cam"].Value;
                    Console.WriteLine("\t\t\tCam {0}", camName);
                    continue;
                }
                // search for compositeKeyBlock start
                matches = compositeKeyBlockStartRegex_.Matches(line);
                if (matches.Count > 0)
                {
                    currBlockType = "compositeKey";
                    Console.WriteLine("\t\t\t\tComposite Key");
                    continue;
                }
                // search for record key
                matches = recorKeyRegex_.Matches(line);
                if (matches.Count > 0)
                {
                    string record = matches[0].Groups["record"].Value;
                    string record_key = matches[0].Groups["record_key"].Value;
                    Console.WriteLine("\t\t\t\t\tRecord {0} = {1}", record, record_key);
                    continue;
                }
                // search for textBlock start
                matches = textBlockStartRegex_.Matches(line);
                if (matches.Count > 0)
                {
                    currBlockType = "text";
                    Console.WriteLine("\t\tText Block");
                    continue;
                }
                // search for text localize
                matches = textStringRegex_.Matches(line);
                if (matches.Count > 0)
                {
                    string textContent = matches[0].Groups["text_content"].Value;
                    Console.WriteLine("\t\t\tText Content {0}", textContent);
                    continue;
                }
                // search for circleBlockStart
                matches = circleBlockStartRegex_.Matches(line);
                if (matches.Count > 0)
                {
                    currBlockType = "circle";
                    Console.WriteLine("\t\tCircle");
                    continue;
                }
                // search for diameter
                matches = diameterRegex_.Matches(line);
                if (matches.Count > 0)
                {
                    string diameter = matches[0].Groups["diameter"].Value;
                    Console.WriteLine("\t\t\tDiameter {0}", diameter);
                    continue;
                }
            }
        }
    }
}
