using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PowerTracer
{
    class PowerLayerPainter : INotifyPropertyChanged
    {
        /*
        The Power Layer Painter has a painting parameters, canvasZoom, canvasPan and listeners to changes in dataObjects so as to update the UI objects
            */
        // set the painter parameters
        public Color color_ = Color.FromRgb(255, 0, 0); // default red color
        public Color highlightColor_ { get; set; } = Color.FromRgb(255, 255, 255); // default white highlight color
        public DisplayStrategyEnum displayStrategy_ = DisplayStrategyEnum.AbsolutePower;
        public double pixelsPerMW_ = 0.02;
        public double pixelsPerNominalPower_ = 2;
        public Point zoom_ = new Point(0.2, 0.2);
        public Point pan_ = new Point(0, 0);

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        public Canvas canvas_ { get; set; }
        public Point Zoom
        {
            get { return zoom_; }
            set
            {
                zoom_ = value;
                // todo recalculate layer polylines points
                NotifyPropertyChanged("Zoom");
            }
        }

        public Point Pan
        {
            get { return pan_; }
            set
            {
                pan_ = value;
                // todo recalculate layer polylines points
                NotifyPropertyChanged("Pan");
            }
        }

        public Color getPowerLineColor(PowerLayerLineObj lineObj)
        {
            // determine color
            Color lineColor = color_;
            
            if (lineObj.isHighLighted_)
            {
                lineColor = highlightColor_;
                return lineColor;
            }

            // todo use line alert flows to decide line color instead of returning only one color
            return lineColor;
        }

        public double getPowerLineThickness(PowerLayerLineObj lineObj)
        {
            // determine thickness
            double lineThickness = 0;
            if (displayStrategy_ == DisplayStrategyEnum.AbsolutePower)
            {
                lineThickness = pixelsPerMW_ * lineObj.lineDataObj_.Power;
            }
            else // displayStrategy_ == DisplayStrategy.NominalPower
            {
                lineThickness = pixelsPerNominalPower_ * lineObj.lineDataObj_.Power / lineObj.lineDataObj_.NominalFlow;
            }
            return lineThickness;
        }

        public PointCollection getPowerLineCanvasPoints(PowerLayerLineObj lineObj)
        {
            // determine line points
            PointCollection linePnts = lineObj.lineDataObj_.LinePoints;

            // transform them according to the zoom and pan values
            for (int i = 0; i < linePnts.Count; i++)
            {
                double newX = (linePnts.ElementAt(i).X + pan_.X) * zoom_.X;
                double newY = (linePnts.ElementAt(i).Y + pan_.Y) * zoom_.Y;
                linePnts[i] = new Point(newX, newY);
            }

            return linePnts;
        }

        // todo determine if line is in canvas bounds so that the line can be removed from canvas children
        // todo perform line updates only if line is visible, i.e., present on the canvas

        public void addElementToCanvas(Polyline line)
        {
            canvas_.Children.Add(line);
        }

        public void removeElementFromCanvas(Polyline line)
        {
            canvas_.Children.Remove(line);
        }

        // listener that subscribes for line and layer object prperty change events
        public void HandlePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Power":
                    // get the lineDataObject and update the Polyline thickness
                    var lineObj = sender as PowerLayerLineObj;
                    lineObj.polyLine_.StrokeThickness = getPowerLineThickness(lineObj);
                    lineObj.polyLine_.Stroke = new SolidColorBrush(getPowerLineColor(lineObj));
                    break;
                case "IsHighLighted":
                    // get the lineDataObject and update the Polyline color
                    lineObj = sender as PowerLayerLineObj;
                    lineObj.polyLine_.Stroke = new SolidColorBrush(getPowerLineColor(lineObj));
                    break;
                case "LinePoints":
                    // get the lineDataObject and update the Polyline points
                    lineObj = sender as PowerLayerLineObj;
                    lineObj.polyLine_.Points = getPowerLineCanvasPoints(lineObj);
                    break;
                case "LayerName":
                    // do something
                    break;
                case "IsLayerVisible":
                    var layerObj = sender as PowerLayerObj;
                    bool isVisible = layerObj.IsLayerVisible;
                    LinesWindow.addLinesToConsole("Changing visibility of "+layerObj.layerName_);
                    foreach (PowerLayerLineObj powerLayerLineObj in layerObj.powerLayerLineObjs_)
                    {
                        if (isVisible)
                        {
                            // add all the layer lines to the canvas if isVisible is true
                            addElementToCanvas(powerLayerLineObj.polyLine_);
                        }
                        else
                        {
                            // remove all the layer lines from the canvas if isVisible is false
                            removeElementFromCanvas(powerLayerLineObj.polyLine_);
                        }
                    }
                    break;
                case "LineAdded":
                    // do something
                    break;
                case "LineRemoved":
                    // do something
                    break;
            }
        }

        public enum DisplayStrategyEnum
        {
            AbsolutePower,
            NominalPower
        }
    }
}
