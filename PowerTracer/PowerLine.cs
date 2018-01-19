using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PowerTracer
{
    public class PowerLine : INotifyPropertyChanged
    {
        // todo implement color display strategies also like severity, voltage, plain
        public Polyline lineObj_;

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        public PowerLine(PointCollection points)
        {
            // Iniliatise the polyline and set the bindings and event listeners

            lineObj_ = new Polyline();

            lineObj_.MouseEnter += delegate (object sender, MouseEventArgs e) { ShowHideHighlight(sender, e, EnterOrLeave.Enter, this); };
            lineObj_.MouseLeave += delegate (object sender, MouseEventArgs e) { ShowHideHighlight(sender, e, EnterOrLeave.Leave, this); };

            // bind line points
            Binding pointsBinding = new Binding("LinePoints");
            pointsBinding.Source = this;
            lineObj_.SetBinding(Polyline.PointsProperty, pointsBinding);

            LinePoints = points;

            // bind line thickness
            Binding thicknessBinding = new Binding("Thickness");
            thicknessBinding.Source = this;
            lineObj_.SetBinding(Polyline.StrokeThicknessProperty, thicknessBinding);

            // bind line color
            Binding colorBinding = new Binding("Color");
            colorBinding.Source = this;
            colorBinding.Converter = new ColorBrushConverter();
            lineObj_.SetBinding(Polyline.StrokeProperty, colorBinding);
        }

        public PointCollection linePoints_;
        public double power_;
        public Color color_ = Color.FromRgb(255, 0, 0); // default red color
        public bool isHighLighted_;
        public Color highlightColor_ { get; set; } = Color.FromRgb(255, 255, 255); // default white highlight color
        public int voltage_ { get; set; }
        public string name_ = "Unknown"; // default line name as unknown
        public string address_ { get; set; }
        public double nominalFlow_ = 1.0;
        public List<float> alertFlows_ { get; set; } = new List<float>(); // default empty alert flows list
        public DisplayStrategyEnum displayStrategy_ = DisplayStrategyEnum.AbsolutePower;
        public double pixelsPerMW_ = 0.02;
        public double pixelsPerNominalPower_ = 2;

        public PointCollection LinePoints
        {
            get
            {
                return linePoints_;
            }
            set
            {
                linePoints_ = value;
                NotifyPropertyChanged("LinePoints");
            }
        }

        public double Power
        {
            get { return power_; }
            set
            {
                power_ = value;
                NotifyPropertyChanged("Power");
                NotifyPropertyChanged("Thickness");
            }
        }

        public bool IsHighLighted
        {
            get
            {
                return isHighLighted_;
            }
            set
            {
                isHighLighted_ = value;
                NotifyPropertyChanged("IsHighLighted");
                NotifyPropertyChanged("Color");
            }
        }
        public Color Color
        {
            get
            {
                if (isHighLighted_)
                {
                    return highlightColor_;
                }
                return color_;
            }
            set
            {
                color_ = value;
                NotifyPropertyChanged("Color");
            }
        }

        public string Name
        {
            get { return name_; }
            set
            {
                name_ = value;
                NotifyPropertyChanged("Name");
            }
        }

        public double PixelsPerMW
        {
            get { return pixelsPerMW_; }
            set
            {
                pixelsPerMW_ = value;
                NotifyPropertyChanged("PixelsPerMW");
                NotifyPropertyChanged("Thickness");
            }
        }

        public double PixelsPerNominalPower
        {
            get { return pixelsPerNominalPower_; }
            set
            {
                pixelsPerNominalPower_ = value;
                NotifyPropertyChanged("PixelsPerNominalPower");
                NotifyPropertyChanged("Thickness");
            }
        }

        public double NominalFlow
        {
            get { return nominalFlow_; }
            set
            {
                nominalFlow_ = value;
                NotifyPropertyChanged("NominalFlow");
                NotifyPropertyChanged("Thickness");
            }
        }

        public DisplayStrategyEnum DisplayStrategy
        {
            get { return displayStrategy_; }
            set
            {
                displayStrategy_ = value;
                NotifyPropertyChanged("DisplayStrategy");
                NotifyPropertyChanged("Thickness");
            }
        }

        public double Thickness
        {
            get
            {
                if (displayStrategy_ == DisplayStrategyEnum.AbsolutePower)
                {
                    return pixelsPerMW_ * power_;
                }
                else // displayStrategy_ == DisplayStrategy.NominalPower
                {
                    return pixelsPerNominalPower_ * power_ / nominalFlow_;
                }
            }
        }

        private static void ShowHideHighlight(object sender, MouseEventArgs e, EnterOrLeave enterOrLeave, PowerLine powerLine)
        {
            // http://www.c-sharpcorner.com/blogs/passing-parameters-to-events-c-sharp1
            Polyline line = sender as Polyline;
            if (powerLine.lineObj_ != null)
            {
                if (enterOrLeave == EnterOrLeave.Enter)
                {
                    powerLine.IsHighLighted = true;
                }
                else
                {
                    powerLine.IsHighLighted = false;
                }
            }
        }

        public class ColorBrushConverter : IValueConverter
        {
            // https://stackoverflow.com/questions/3309709/how-do-i-convert-a-color-to-a-brush-in-xaml
            public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                if (value == null)
                    return null;

                if (value is Color)
                    return new SolidColorBrush((Color)value);

                throw new InvalidOperationException("Unsupported type [" + value.GetType().Name + "], ColorToSolidColorBrushValueConverter.Convert()");
            }

            public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                // useful incase of two way binding. Right it doesnot work
                if (value == null)
                    return null;

                if (value is SolidColorBrush)
                    return ((SolidColorBrush)value).Color;

                throw new InvalidOperationException("Unsupported type [" + value.GetType().Name + "], ColorToSolidColorBrushValueConverter.ConvertBack()");
            }
        }

        private enum EnterOrLeave
        {
            Enter,
            Leave
        }

        public enum DisplayStrategyEnum
        {
            AbsolutePower,
            NominalPower
        }
    }
}
