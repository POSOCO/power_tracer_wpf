using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PowerTracer
{
    class PowerLayerLineDataObj : INotifyPropertyChanged
    {
        /*
        Data object properties are power, voltage, linePoints, name, address, nominal flow, alertFlows
        The changes we notify for changes in mapboard are power, linePoints, name, nominal flow, alertFlows
        Changes to notify in future - voltage, address 
        Notifying above changes is not required since we are assume that they are set only once
            */

        public PowerLayerLineDataObj()
        {

        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        public PointCollection linePoints_ = new PointCollection();
        public double power_;
        public int voltage_ { get; set; }
        public string name_ = "Unknown"; // default line name as unknown
        public string address_ { get; set; }
        public double nominalFlow_ = 1.0;
        public List<float> alertFlows_ { get; set; } = new List<float>(); // default empty alert flows list

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
            }
        }

        public List<float> AlertFlows
        {
            get { return alertFlows_; }
            set
            {
                alertFlows_ = value;
                NotifyPropertyChanged("AlertFlows");
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

        public double NominalFlow
        {
            get { return nominalFlow_; }
            set
            {
                nominalFlow_ = value;
                NotifyPropertyChanged("NominalFlow");
            }
        }

    }
}
