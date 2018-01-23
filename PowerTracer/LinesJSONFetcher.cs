using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
                dynamic jsonLayers = JValue.Parse(str);
                foreach (dynamic jsonLayer in jsonLayers.layers)
                {
                    PowerLineLayer layer = new PowerLineLayer();
                    layer.name_ = jsonLayer.name;
                    // Console.WriteLine("\t" + jsonLayer.name);
                    layer.layerLines_ = new List<PowerLine>();

                    foreach (dynamic layerLine in jsonLayer.polyLines)
                    {
                        PowerLine line = new PowerLine();
                        line.address_ = layerLine.ednaId;
                        // Console.WriteLine("\t\t" + layerLine.ednaId as string);
                        foreach (dynamic linePoint in layerLine.points)
                        {
                            // Console.WriteLine("\t\t\t (" + Convert.ToDouble(linePoint.x) + ", " + Convert.ToDouble(linePoint.y) + ")");
                            line.LinePoints.Add(new Point(Convert.ToDouble(linePoint.x), Convert.ToDouble(linePoint.y)));
                        }

                        // do the points modification for drawing
                        if (line.linePoints_.Count > 2)
                        {
                            line.linePoints_.RemoveAt(0);
                            Point origin = line.linePoints_.ElementAt(0);
                            for (int i = 1; i < line.linePoints_.Count; i++)
                            {
                                double newX = line.linePoints_.ElementAt(i).X + origin.X;
                                double newY = line.linePoints_.ElementAt(i).Y + origin.Y;
                                line.linePoints_[i] = new Point(newX, newY);
                                origin = line.linePoints_.ElementAt(i);
                            }
                            for (int i = 0; i < line.linePoints_.Count; i++)
                            {
                                double newX = line.linePoints_.ElementAt(i).X / 5.0;
                                double newY = line.linePoints_.ElementAt(i).Y / 5.0;
                                line.linePoints_[i] = new Point(newX, newY);
                            }

                            layer.layerLines_.Add(line);
                        }
                    }
                    if (layer.layerLines_.Count > 0)
                    {
                        layers.Add(layer);
                    }
                }
            }
            return layers;
        }

        public List<PowerLayerObj> fetchPowerLayerObjs(string str)
        {
            // https://weblog.west-wind.com/posts/2012/Aug/30/Using-JSONNET-for-dynamic-JSON-parsing
            List<PowerLayerObj> layers = new List<PowerLayerObj>();
            if (str != null)
            {
                // todo deal with parsing errors
                dynamic jsonLayers = JValue.Parse(str);
                foreach (dynamic jsonLayer in jsonLayers.layers)
                {
                    PowerLayerObj layer = new PowerLayerObj();
                    layer.LayerName = jsonLayer.name;
                    // Console.WriteLine("\t" + jsonLayer.name);
                    //stub
                    foreach (dynamic layerLine in jsonLayer.polyLines)
                    {
                        PowerLayerLineObj line = new PowerLayerLineObj();
                        line.lineDataObj_.address_ = layerLine.ednaId;
                        // Console.WriteLine("\t\t" + layerLine.ednaId as string);
                        foreach (dynamic linePoint in layerLine.points)
                        {
                            // Console.WriteLine("\t\t\t (" + Convert.ToDouble(linePoint.x) + ", " + Convert.ToDouble(linePoint.y) + ")");
                            line.lineDataObj_.linePoints_.Add(new Point(Convert.ToDouble(linePoint.x), Convert.ToDouble(linePoint.y)));
                        }

                        // do the points modification for drawing
                        if (line.lineDataObj_.linePoints_.Count > 2)
                        {
                            line.lineDataObj_.linePoints_.RemoveAt(0);
                            Point origin = line.lineDataObj_.linePoints_.ElementAt(0);
                            for (int i = 1; i < line.lineDataObj_.linePoints_.Count; i++)
                            {
                                double newX = line.lineDataObj_.linePoints_.ElementAt(i).X + origin.X;
                                double newY = line.lineDataObj_.linePoints_.ElementAt(i).Y + origin.Y;
                                line.lineDataObj_.linePoints_[i] = new Point(newX, newY);
                                origin = line.lineDataObj_.linePoints_.ElementAt(i);
                            }

                            layer.powerLayerLineObjs_.Add(line);
                        }
                    }
                    if (layer.powerLayerLineObjs_.Count > 0)
                    {
                        layers.Add(layer);
                    }
                }
            }
            return layers;
        }
    }
}
