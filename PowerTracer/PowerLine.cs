using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PowerTracer
{
    public class PowerLine
    {
        // todo implement color display strategies also like severity, voltage, plain
        public Polyline lineObj_;
        public PowerLine(Polyline line)
        {
            lineObj_ = line;
            colorBrush_ = new SolidColorBrush(color_);
            lineObj_.Stroke = colorBrush_;
            lineObj_.MouseEnter += delegate (object sender, MouseEventArgs e) { ShowHideHighlight(sender, e, EnterOrLeave.Enter); };
            lineObj_.MouseLeave += delegate (object sender, MouseEventArgs e) { ShowHideHighlight(sender, e, EnterOrLeave.Leave); };
            // bind line thickness
            Binding thicknessBinding = new Binding("thickness_");
            thicknessBinding.Source = this;
            lineObj_.SetBinding(Line.StrokeThicknessProperty, thicknessBinding);
            // todo bind line color
        }
        public double power_ { get; set; }
        public Color color_ { get; set; } = Color.FromRgb(255, 0, 0); // default red color
        public SolidColorBrush colorBrush_ { get; set; }
        public Color highlightColor_ { get; set; } = Color.FromRgb(255,255,255); // default white highlight color
        public int voltage_ { get; set; }
        public string name_ { get; set; } = "Unknown"; // default line name as unknown
        public string address_ { get; set; }
        public double nominalFlow_ { get; set; }
        public List<float> alertFlows_ { get; set; } = new List<float>(); // default empty alert flows list
        public DisplayStrategy displayStrategy_ { get; set; } = DisplayStrategy.AbsolutePower;
        public double pixelsPerMW_ { get; set; } = 0.02;
        public double pixelsPerNominalPower_ { get; set; } = 2;

        public double thickness_
        {
            get
            {
                if (displayStrategy_ == DisplayStrategy.AbsolutePower)
                {
                    return pixelsPerMW_ * power_;
                }
                else // displayStrategy_ == DisplayStrategy.NominalPower
                {
                    return pixelsPerNominalPower_ * power_ / nominalFlow_;
                }
            }
        }

        private static void ShowHideHighlight(object sender, MouseEventArgs e, EnterOrLeave enterOrLeave)
        {
            // http://www.c-sharpcorner.com/blogs/passing-parameters-to-events-c-sharp1
            var powerLine = sender as PowerLine;
            if (powerLine.lineObj_ != null)
            {
                if (enterOrLeave == EnterOrLeave.Enter)
                {
                    powerLine.lineObj_.Stroke = new SolidColorBrush(powerLine.highlightColor_);
                }
                else
                {
                    powerLine.lineObj_.Stroke = new SolidColorBrush(powerLine.color_);
                }
            }
        }

        private enum EnterOrLeave
        {
            Enter,
            Leave
        }

        public enum DisplayStrategy
        {
            AbsolutePower,
            NominalPower
        }
    }
}
