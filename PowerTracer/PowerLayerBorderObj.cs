using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Shapes;

namespace PowerTracer
{
    class PowerLayerBorderObj
    {

        /*
        The Power line border has a polyline(ui object), PowerLayerBorderDataObj.
        Polyline (UI Object) additional property stored is IsHighLighted
        The changes we notify for changes in mapboard are linePoints, IsHighLighted        
            */
        public PowerLayerBorderDataObj borderDataObj_ { get; set; } = new PowerLayerBorderDataObj();
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

        public PowerLayerBorderObj()
        {
            //setting the lineObj_ MouseEnter and MouseLeave events for hover highlight effect
            polyLine_.MouseEnter += delegate (object sender, MouseEventArgs e) { ShowHideHighlight(sender, e, EnterOrLeave.Enter, this); };
            polyLine_.MouseLeave += delegate (object sender, MouseEventArgs e) { ShowHideHighlight(sender, e, EnterOrLeave.Leave, this); };
            
            // subscribe to the property changes in lineDataObject so that we can notify the painter
            borderDataObj_.PropertyChanged += HandlePropertyChanged;
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
                case "LinePoints":
                    // notify this event for the painter
                    NotifyPropertyChanged("LinePoints");
                    break;
            }
        }

        private static void ShowHideHighlight(object sender, MouseEventArgs e, EnterOrLeave enterOrLeave, PowerLayerBorderObj lineObj)
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
