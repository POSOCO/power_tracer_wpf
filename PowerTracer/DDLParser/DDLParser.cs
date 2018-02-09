using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

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
        public static Regex pictureBlockStartRegex_ = new Regex(@"^\s*picture\s+""(?<picture>.+)""\s*$", RegexOptions.Compiled);
        public static Regex diameterRegex_ = new Regex(@"^\s*diameter\s*\((?<diameter>\d+)\)\s*$", RegexOptions.Compiled);

        public static void parseDDLTestPrint()
        {
            string currBlockType = "";
            bool includePictures = false;
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
                if (includePictures)
                {
                    // search for pictureBlock start
                    matches = pictureBlockStartRegex_.Matches(line);
                    if (matches.Count > 0)
                    {
                        currBlockType = "picture";
                        string pictureName = matches[0].Groups["picture"].Value;
                        Console.WriteLine("\t\tPicture {0}", pictureName);
                        continue;
                    }
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

        public static MapDDL parseDDLToObject()
        {
            // string currBlockType = "";
            MapDDLDisplay level1Display = null;
            MapDDLLayer level2Layer = null;
            object level3Obj = null;
            object level4Obj = null;

            bool includePictures = false;
            // initilaize the mapDDL
            MapDDL mapDDL = new MapDDL();
            foreach (string lineText in File.ReadLines("mapboard.ddl"))
            {
                // search for displays
                MatchCollection matches = displayBlockStartRegex_.Matches(lineText);
                if (matches.Count > 0)
                {
                    // wrap up below levels
                    insertLevel4ObjIntoLevel3(level3Obj, level4Obj);
                    level4Obj = null;
                    insertLevel3ObjIntoLevel2(level3Obj, level2Layer);
                    level3Obj = null;
                    insertLevel2ObjIntoLevel1(level2Layer, level1Display);
                    level2Layer = null;
                    // add previous display object to the mapDDL displays array
                    insertLevel1ObjIntoMap(level1Display, mapDDL);
                    string displayName = matches[0].Groups["display"].Value;
                    Console.WriteLine("Display {0}", displayName);
                    level1Display = new MapDDLDisplay();
                    level1Display.name = displayName;
                    continue;
                }
                if (level1Display != null)
                {
                    // search for layers only if we have a non null display or level 1 objects
                    matches = layerBlockStartRegex_.Matches(lineText);
                    if (matches.Count > 0)
                    {
                        // wrap up below levels
                        insertLevel4ObjIntoLevel3(level3Obj, level4Obj);
                        level4Obj = null;
                        insertLevel3ObjIntoLevel2(level3Obj, level2Layer);
                        level3Obj = null;
                        insertLevel2ObjIntoLevel1(level2Layer, level1Display);

                        string layerName = matches[0].Groups["layer"].Value;
                        Console.WriteLine("\tlayer {0}", layerName);
                        level2Layer = new MapDDLLayer();
                        level2Layer.name = layerName;
                        continue;
                    }
                }
                if (level2Layer != null)
                {
                    // search for polylines only if we have a non null layer or level 2 objects
                    matches = polylineBlockStartRegex_.Matches(lineText);
                    if (matches.Count > 0)
                    {
                        // add previous level 3 object to the mapDDL display layer level3 objects array
                        // wrap up below levels
                        insertLevel4ObjIntoLevel3(level3Obj, level4Obj);
                        level4Obj = null;
                        insertLevel3ObjIntoLevel2(level3Obj, level2Layer);
                        level3Obj = new MapDDLPolyline();
                        Console.WriteLine("\t\tPolyline");
                        continue;
                    }

                    // search for circles only if we have a non null layer or level 2 objects
                    matches = circleBlockStartRegex_.Matches(lineText);
                    if (matches.Count > 0)
                    {
                        // add previous level 3 object to the mapDDL display layer level3 objects array
                        // wrap up below levels
                        insertLevel4ObjIntoLevel3(level3Obj, level4Obj);
                        level4Obj = null;
                        insertLevel3ObjIntoLevel2(level3Obj, level2Layer);
                        level3Obj = new MapDDLCircle();
                        Console.WriteLine("\t\tCircle");
                        continue;
                    }

                    // search for texts only if we have a non null layer or level 2 objects
                    matches = textBlockStartRegex_.Matches(lineText);
                    if (matches.Count > 0)
                    {
                        // add previous level 3 object to the mapDDL display layer level3 objects array
                        // wrap up below levels
                        insertLevel4ObjIntoLevel3(level3Obj, level4Obj);
                        level4Obj = null;
                        insertLevel3ObjIntoLevel2(level3Obj, level2Layer);
                        level3Obj = new MapDDLText();
                        Console.WriteLine("\t\tText Block");
                        continue;
                    }

                    if (includePictures)
                    {
                        // search for pictures only if we have a non null layer or level 2 objects
                        matches = pictureBlockStartRegex_.Matches(lineText);
                        if (matches.Count > 0)
                        {
                            // add previous level 3 object to the mapDDL display layer level3 objects array
                            // wrap up below levels
                            insertLevel4ObjIntoLevel3(level3Obj, level4Obj);
                            level4Obj = null;
                            insertLevel3ObjIntoLevel2(level3Obj, level2Layer);
                            string pictureName = matches[0].Groups["picture"].Value;
                            level3Obj = new MapDDLPicture();
                            ((MapDDLPicture)level3Obj).name = pictureName;
                            Console.WriteLine("\t\tPicture {0}", pictureName);
                            continue;
                        }
                    }
                }

                if (level3Obj != null)
                {
                    // find level 3 object origin
                    Point origin = getLevel3ObjOrigin(level3Obj);
                    // search for origin only if we have a non null level 3 object (Polyline, Text, Circle, Picture)
                    matches = originRegex_.Matches(lineText);
                    if (matches.Count > 0)
                    {
                        if (!Double.IsNaN(origin.X))
                        {
                            // Whenever we find the origin, wrap up the level 3 element that has the origin and add it to the respective array.
                            // wrap up below levels
                            insertLevel4ObjIntoLevel3(level3Obj, level4Obj);
                            level4Obj = null;
                            insertLevel3ObjIntoLevel2(level3Obj, level2Layer);
                            level3Obj = null;
                            continue;
                        }
                        // Assign level 3 object the origin                        
                        string x_num = matches[0].Groups["x_num"].Value;
                        string y_num = matches[0].Groups["y_num"].Value;
                        assignOriginToLevel3Obj(level3Obj, new Point(Double.Parse(x_num), Double.Parse(y_num)));
                        Console.WriteLine("\t\t\tOrigin = {0}, {1}", x_num, y_num);
                        continue;
                    }

                    // Donot regex check for level 4+ elements till we initialize origin.
                    if (!Double.IsNaN(origin.X))
                    {
                        // search for gab
                        matches = gabRegex_.Matches(lineText);
                        if (matches.Count > 0)
                        {
                            // wrap up below levels
                            insertLevel4ObjIntoLevel3(level3Obj, level4Obj);
                            level4Obj = null;
                            // add gab to the level3Object
                            string gabText = matches[0].Groups["gab"].Value;
                            assignGabToLevel3Obj(level3Obj, gabText);
                            Console.WriteLine("\t\t\tGab {0}", gabText);
                            continue;
                        }

                        // search for camBlockStart
                        matches = camBlockStartRegex_.Matches(lineText);
                        if (matches.Count > 0)
                        {
                            // wrap up the level 4 object
                            insertLevel4ObjIntoLevel3(level3Obj, level4Obj);
                            level4Obj = new MapDDLCam();
                            string camName = matches[0].Groups["cam"].Value;
                            ((MapDDLCam)level4Obj).name = camName;
                            Console.WriteLine("\t\t\tCam {0}", camName);
                            continue;
                        }

                        // point is present in only polyline element. So the level 3 element should be polyline
                        if (level3Obj.GetType().FullName == typeof(MapDDLPolyline).FullName)
                        {
                            matches = pointRegex_.Matches(lineText);
                            if (matches.Count > 0)
                            {
                                // wrap up the level 4 object
                                insertLevel4ObjIntoLevel3(level3Obj, level4Obj);
                                level4Obj = null;
                                string x_num = matches[0].Groups["x_num"].Value;
                                string y_num = matches[0].Groups["y_num"].Value;
                                // add points to the polyline points list
                                ((MapDDLPolyline)level3Obj).points.Add(new Point(Double.Parse(x_num), Double.Parse(y_num)));
                                Console.WriteLine("\t\t\tPoint = {0}, {1}", x_num, y_num);
                                continue;
                            }
                        }
                        // diameter is present in only circle element. So the level 3 element should be circle
                        if (level3Obj.GetType().FullName == typeof(MapDDLCircle).FullName)
                        {
                            // search for diameter
                            matches = diameterRegex_.Matches(lineText);
                            if (matches.Count > 0)
                            {
                                // wrap up the level 4 object
                                insertLevel4ObjIntoLevel3(level3Obj, level4Obj);
                                level4Obj = null;
                                string diameter = matches[0].Groups["diameter"].Value;
                                // assign diameter to the circle
                                ((MapDDLCircle)level3Obj).diameter = Double.Parse(diameter);
                                Console.WriteLine("\t\t\tDiameter {0}", diameter);
                                continue;
                            }
                        }

                        // localize is present in only text element. So the level 3 element should be text
                        if (level3Obj.GetType().FullName == typeof(MapDDLText).FullName)
                        {
                            // search for text localize
                            matches = textStringRegex_.Matches(lineText);
                            if (matches.Count > 0)
                            {
                                // wrap up the level 4 object
                                insertLevel4ObjIntoLevel3(level3Obj, level4Obj);
                                level4Obj = null;
                                string textContent = matches[0].Groups["text_content"].Value;
                                Console.WriteLine("\t\t\tText Content {0}", textContent);
                                // assign diameter to the circle
                                ((MapDDLText)level3Obj).localize = textContent;
                                continue;
                            }
                        }

                    }
                }

                if (level4Obj != null)
                {
                    // search for record key
                    matches = recorKeyRegex_.Matches(lineText);
                    if (matches.Count > 0)
                    {
                        string record = matches[0].Groups["record"].Value;
                        string record_key = matches[0].Groups["record_key"].Value;
                        ((MapDDLCam)level4Obj).compositeKey.Add(record, record_key);
                        Console.WriteLine("\t\t\t\t\tRecord {0} = {1}", record, record_key);
                        continue;
                    }
                }
                /*
                // search for compositeKeyBlock start
                matches = compositeKeyBlockStartRegex_.Matches(lineText);
                if (matches.Count > 0)
                {
                    currBlockType = "compositeKey";
                    Console.WriteLine("\t\t\t\tComposite Key");
                    continue;
                }
                */
            }
            // Now wrap up everything left into the mapDDL
            insertLevel4ObjIntoLevel3(level3Obj, level4Obj);
            level4Obj = null;
            insertLevel3ObjIntoLevel2(level3Obj, level2Layer);
            level3Obj = null;
            insertLevel2ObjIntoLevel1(level2Layer, level1Display);
            level2Layer = null;
            // add previous display object to the mapDDL displays array
            insertLevel1ObjIntoMap(level1Display, mapDDL);
            // level1Display = null;
            return mapDDL;
        }

        private static Point getLevel3ObjOrigin(object level3Obj)
        {
            if (level3Obj.GetType().FullName == typeof(MapDDLPolyline).FullName)
            {
                return ((MapDDLPolyline)level3Obj).origin;
            }
            else if (level3Obj.GetType().FullName == typeof(MapDDLText).FullName)
            {
                return ((MapDDLText)level3Obj).origin;
            }
            else if (level3Obj.GetType().FullName == typeof(MapDDLCircle).FullName)
            {
                return ((MapDDLCircle)level3Obj).origin;
            }
            else if (level3Obj.GetType().FullName == typeof(MapDDLPicture).FullName)
            {
                return ((MapDDLPicture)level3Obj).origin;
            }
            return new Point(Double.NaN, Double.NaN);
        }

        private static void assignOriginToLevel3Obj(object level3Obj, Point origin)
        {
            if (level3Obj.GetType().FullName == typeof(MapDDLPolyline).FullName)
            {
                ((MapDDLPolyline)level3Obj).origin = new Point(origin.X, origin.Y);
            }
            else if (level3Obj.GetType().FullName == typeof(MapDDLText).FullName)
            {
                ((MapDDLText)level3Obj).origin = new Point(origin.X, origin.Y);
            }
            else if (level3Obj.GetType().FullName == typeof(MapDDLCircle).FullName)
            {
                ((MapDDLCircle)level3Obj).origin = new Point(origin.X, origin.Y);
            }
            else if (level3Obj.GetType().FullName == typeof(MapDDLPicture).FullName)
            {
                ((MapDDLPicture)level3Obj).origin = new Point(origin.X, origin.Y);
            }
        }

        public static void assignGabToLevel3Obj(object level3Obj, string gabText)
        {
            if (level3Obj.GetType().FullName == typeof(MapDDLPolyline).FullName)
            {
                ((MapDDLPolyline)level3Obj).gab = gabText;
            }
            else if (level3Obj.GetType().FullName == typeof(MapDDLText).FullName)
            {
                ((MapDDLText)level3Obj).gab = gabText;
            }
            else if (level3Obj.GetType().FullName == typeof(MapDDLCircle).FullName)
            {
                ((MapDDLCircle)level3Obj).gab = gabText;
            }
            else if (level3Obj.GetType().FullName == typeof(MapDDLPicture).FullName)
            {
                ((MapDDLPicture)level3Obj).gab = gabText;
            }
        }

        public static void insertLevel4ObjIntoLevel3(object level3Obj, object level4Obj)
        {
            if (level3Obj != null && level4Obj != null && level4Obj.GetType().FullName == typeof(MapDDLCam).FullName)
            {
                // if level 4 object is a cam
                if (level3Obj.GetType().FullName == typeof(MapDDLPolyline).FullName)
                {
                    ((MapDDLPolyline)level3Obj).cam = ((MapDDLCam)level4Obj);
                }
                else if (level3Obj.GetType().FullName == typeof(MapDDLCircle).FullName)
                {
                    ((MapDDLCircle)level3Obj).cam = ((MapDDLCam)level4Obj);
                }
                else if (level3Obj.GetType().FullName == typeof(MapDDLPicture).FullName)
                {
                    ((MapDDLPicture)level3Obj).cam = ((MapDDLCam)level4Obj);
                }
            }
        }

        private static void insertLevel3ObjIntoLevel2(object level3Obj, MapDDLLayer level2Layer)
        {
            // add previous level 3 object to the mapDDL display layer level3 objects array
            if (level3Obj != null && level2Layer != null)
            {
                if (level3Obj.GetType().FullName == typeof(MapDDLPolyline).FullName)
                {
                    // the level 3 object was a polyline
                    level2Layer.polylines.Add((MapDDLPolyline)level3Obj);
                }
                else if (level3Obj.GetType().FullName == typeof(MapDDLCircle).FullName)
                {
                    // the level 3 object was a circle
                    level2Layer.circles.Add((MapDDLCircle)level3Obj);
                }
                else if (level3Obj.GetType().FullName == typeof(MapDDLText).FullName)
                {
                    // the level 3 object was a circle
                    level2Layer.texts.Add((MapDDLText)level3Obj);
                }
                else if (level3Obj.GetType().FullName == typeof(MapDDLPicture).FullName)
                {
                    // the level 3 object was a circle
                    level2Layer.pictures.Add((MapDDLPicture)level3Obj);
                }
            }
        }

        public static void insertLevel2ObjIntoLevel1(MapDDLLayer level2Layer, MapDDLDisplay level1Display)
        {
            if (level2Layer != null)
            {
                level1Display.layers.Add(level2Layer);
            }
        }

        public static void insertLevel1ObjIntoMap(MapDDLDisplay level1Display, MapDDL mapDDL)
        {
            if (level1Display != null)
            {
                mapDDL.displays.Add(level1Display);
            }
        }
    }
}
