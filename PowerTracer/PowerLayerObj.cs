using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerTracer
{
    class PowerLayerObj : INotifyPropertyChanged
    {
        /*
        The Power Layer object has a LayerName, LayerVisibility PowerLayerLines array
        The changes we notify for changes in mapboard are layerName, layerVisibility, property changes of each line in the layerLinesArray
            */

        public string layerName_ { get; set; } = "Unknown";
        bool isLayerVisible_ { get; set; } = true;
        public ObservableCollection<PowerLayerLineObj> powerLayerLineObjs_ { get; set; } = new ObservableCollection<PowerLayerLineObj>();

        public event PropertyChangedEventHandler PropertyChanged;

        public PowerLayerObj()
        {
            powerLayerLineObjs_.CollectionChanged += new NotifyCollectionChangedEventHandler (CollectionChangedMethod);
        }

        public string LayerName
        {
            get { return layerName_; }
            set
            {
                layerName_ = value;
                // notify this event for the painter
                NotifyPropertyChanged(this, new PropertyChangedEventArgs("LayerName"));
            }
        }

        public bool IsLayerVisible
        {
            get { return isLayerVisible_; }
            set
            {
                isLayerVisible_ = value;
                // notify this event for the painter
                NotifyPropertyChanged(this, new PropertyChangedEventArgs("IsLayerVisible"));
            }
        }

        private void CollectionChangedMethod(object sender, NotifyCollectionChangedEventArgs e)
        {
            // https://stackoverflow.com/questions/7316591/collectionchanged-event-of-observablecollection-in-c-sharp
            // https://www.codeproject.com/Tips/694370/How-to-Listen-to-Property-Chang

            //different kind of changes that may have occurred in collection

            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                RegisterLinePropertyChanged(e.NewItems);

            }
            if (e.Action == NotifyCollectionChangedAction.Replace)
            {
                UnRegisterlinePropertyChanged(e.OldItems);
                RegisterLinePropertyChanged(e.NewItems);
            }
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                UnRegisterlinePropertyChanged(e.OldItems);
            }
            if (e.Action == NotifyCollectionChangedAction.Move)
            {
                // our code
            }
        }

        private void RegisterLinePropertyChanged(IList items)
        {
            foreach (PowerLayerLineObj item in items)
            {
                if (item != null)
                {
                    // subscribe to the property changes in lineDataObject so that we can notify the painter
                    item.PropertyChanged += NotifyPropertyChanged;
                    // notify the painter that a new line is added to the layer for canvas painting
                    // NotifyPropertyChanged(item, new PropertyChangedEventArgs("LineAdded"));
                }
            }
        }

        private void UnRegisterlinePropertyChanged(IList items)
        {
            foreach (INotifyPropertyChanged item in items)
            {
                if (item != null)
                {
                    // unsubscribe to the property changes in lineDataObject
                    item.PropertyChanged -= NotifyPropertyChanged;
                    // notify the painter that a new line is added to the layer for canvas painting
                    // NotifyPropertyChanged(item, new PropertyChangedEventArgs("LineRemoved"));
                }
            }
        }

        // listener that subscribes for line object change event
        private void NotifyPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // call the event handler which will be set by the painter
            PropertyChanged(sender, e);
        }

    }
}
