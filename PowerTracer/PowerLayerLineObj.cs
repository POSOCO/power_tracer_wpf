using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Shapes;

namespace PowerTracer
{
    class PowerLayerLineObj : INotifyPropertyChanged
    {
        /*
        The Power line object has a data object and a polyline(ui object).
        Data object properties are power, voltage, linePoints, name, address, nominal flow, alertFlows
        Polyline (UI Object) additional property stored is IsHighLighted
        The changes we notify for changes in mapboard are power, linePoints, IsHighLighted
        Changes to notify in future - voltage, name, address, nominal Flow, alertFlows. 
        Notifying above changes is not required since we are assume that they are set only once
            */
        public PowerLayerLineDataObj lineDataObj_ { get; set; } = new PowerLayerLineDataObj();
        public Polyline polyLine_ { get; set; } = new Polyline();

        public bool isHighLighted_ = false;

        public bool IsHighLighted
        {
            get { return isHighLighted_; }
            set
            {
                isHighLighted_ = value;
                // notify this event for the painter
                NotifyPropertyChanged("IsHighLighted");
            }
        }

        public PowerLayerLineObj()
        {
            //setting the lineObj_ MouseEnter and MouseLeave events for hover highlight effect
            polyLine_.MouseEnter += delegate (object sender, MouseEventArgs e) { ShowHideHighlight(sender, e, EnterOrLeave.Enter, this); };
            polyLine_.MouseLeave += delegate (object sender, MouseEventArgs e) { ShowHideHighlight(sender, e, EnterOrLeave.Leave, this); };

            // subscribe to the property changes in lineDataObject so that we can notify the painter
            lineDataObj_.PropertyChanged += HandlePropertyChanged;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        // listener that subscribes for line object change event
        public void HandlePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Power":
                    // notify this event for the painter
                    NotifyPropertyChanged("Power");
                    break;
                case "LinePoints":
                    // notify this event for the painter
                    NotifyPropertyChanged("LinePoints");
                    break;
            }
        }

        private static void ShowHideHighlight(object sender, MouseEventArgs e, EnterOrLeave enterOrLeave, PowerLayerLineObj lineObj)
        {
            // http://www.c-sharpcorner.com/blogs/passing-parameters-to-events-c-sharp1
            if (lineObj != null)
            {
                if (enterOrLeave == EnterOrLeave.Enter)
                {
                    lineObj.IsHighLighted = true;
                }
                else
                {
                    lineObj.IsHighLighted = false;
                }
            }
        }

        private enum EnterOrLeave
        {
            Enter,
            Leave
        }
    }
}
