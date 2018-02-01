using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
        public static Color color_400_kv_ = Color.FromRgb(255, 0, 0); // 400 KV color
        public static Color color_765_kv_ = Color.FromRgb(255, 255, 0); // 765 KV color
        public static Color defBorderColor_ = Color.FromRgb(255, 228, 181);
        public Color highlightColor_ { get; set; } = Color.FromRgb(255, 255, 255); // default white highlight color
        public DisplayStrategyEnum displayStrategy_ = DisplayStrategyEnum.AbsolutePower;
        public double pixelsPerMW_ = 0.02;
        public double pixelsPerNominalPower_ = 2;
        public static Point defZoom_ = new Point(0.16, 0.16);
        public static Point defPan_ = new Point(161, -32);
        public Point zoom_ = new Point(defZoom_.X, defZoom_.Y);
        public Point pan_ = new Point(defPan_.X, defPan_.Y);
        Nullable<Point> dragStart_ = null;

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        public PowerLayerPainter()
        {

        }

        public Canvas canvas_ { get; set; }

        public void setCanvas(Canvas canvas)
        {
            canvas_ = canvas;
            canvas_.MouseDown += Canvas_MouseDown;
            canvas_.MouseMove += Canvas_MouseMove;
            canvas_.MouseUp += Canvas_MouseUp;
        }

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
            Color lineColor = color_400_kv_;

            if (lineObj.isHighLighted_)
            {
                lineColor = highlightColor_;
                return lineColor;
            }

            if (lineObj.lineDataObj_.voltage_ == 765)
            {
                lineColor = color_765_kv_;
                return lineColor;
            }

            // todo use line alert flows to decide line color instead of returning only one color
            return lineColor;
        }

        public Color getBorderColor(PowerLayerBorderObj borderLineObj)
        {
            // determine color
            Color lineColor = defBorderColor_;

            if (borderLineObj.isHighLighted_)
            {
                lineColor = highlightColor_;
                return lineColor;
            }

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

        public PointCollection getPowerLineCanvasPoints(PointCollection lineObjPnts)
        {
            // determine line points
            PointCollection linePnts = new PointCollection(lineObjPnts);

            // transform them according to the zoom and pan values
            for (int i = 0; i < linePnts.Count; i++)
            {
                double newX = (linePnts.ElementAt(i).X) * zoom_.X + pan_.X;
                double newY = (linePnts.ElementAt(i).Y) * zoom_.Y + pan_.Y;
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
                    if (lineObj != null)
                    {
                        lineObj.polyLine_.StrokeThickness = getPowerLineThickness(lineObj);
                        lineObj.polyLine_.Stroke = new SolidColorBrush(getPowerLineColor(lineObj));
                    }
                    break;
                case "IsHighLighted":
                    // get the lineDataObject and update the Polyline color
                    lineObj = sender as PowerLayerLineObj;
                    if (lineObj != null)
                    {
                        lineObj.polyLine_.Stroke = new SolidColorBrush(getPowerLineColor(lineObj));
                        break;
                    }
                    var borderObj = sender as PowerLayerBorderObj;
                    if (borderObj != null)
                    {
                        borderObj.polyLine_.Stroke = new SolidColorBrush(getBorderColor(borderObj));
                        break;
                    }
                    break;
                case "LinePoints":
                    // get the lineDataObject and update the Polyline points
                    lineObj = sender as PowerLayerLineObj;
                    if (lineObj != null)
                    {
                        lineObj.polyLine_.Points = getPowerLineCanvasPoints(lineObj.lineDataObj_.linePoints_);
                        break;
                    }
                    borderObj = sender as PowerLayerBorderObj;
                    if (borderObj != null)
                    {
                        borderObj.polyLine_.Points = getPowerLineCanvasPoints(borderObj.borderDataObj_.linePoints_);
                        break;
                    }
                    break;
                case "LayerName":
                    // do something
                    break;
                case "IsLayerVisible":
                    var layerObj = sender as PowerLayerObj;
                    if (layerObj != null)
                    {
                        bool isVisible = layerObj.IsLayerVisible;
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
                        foreach (PowerLayerBorderObj powerLayerBorderObj in layerObj.powerLayerBorderObjs_)
                        {
                            if (isVisible)
                            {
                                // add all the layer lines to the canvas if isVisible is true
                                addElementToCanvas(powerLayerBorderObj.polyLine_);
                            }
                            else
                            {
                                // remove all the layer lines from the canvas if isVisible is false
                                removeElementFromCanvas(powerLayerBorderObj.polyLine_);
                            }
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

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender != null)
            {
                dragStart_ = e.GetPosition((Canvas)sender);
            }
        }

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            dragStart_ = null;
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && dragStart_ != null)
            {
                Point newPosition = e.GetPosition((Canvas)sender);
                Pan = new Point(Pan.X + (newPosition.X - dragStart_.Value.X) * 1, Pan.Y + (newPosition.Y - dragStart_.Value.Y) * 1);
                dragStart_ = newPosition;
            }
        }

        public enum DisplayStrategyEnum
        {
            AbsolutePower,
            NominalPower
        }
    }
}
