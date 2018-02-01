using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PowerTracer
{
    class PowerLayerBorderDataObj : INotifyPropertyChanged
    {
        /*
        Data object properties are linePoints
        The changes we notify for changes in mapboard are linePoints
            */

        public PowerLayerBorderDataObj()
        {

        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        public PointCollection linePoints_ = new PointCollection();

        public PointCollection LinePoints
        {
            get { return linePoints_; }
            set
            {
                linePoints_ = value;
                NotifyPropertyChanged("LinePoints");
            }
        }
    }
}
