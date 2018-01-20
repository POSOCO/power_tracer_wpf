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
            LinePoints = points;
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
            get { return linePoints_; }
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
            get { return isHighLighted_; }
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

        public enum DisplayStrategyEnum
        {
            AbsolutePower,
            NominalPower
        }
    }
}
