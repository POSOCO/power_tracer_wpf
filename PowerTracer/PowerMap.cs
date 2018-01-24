﻿using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media;

namespace PowerTracer
{
    class PowerMap
    {
        ObservableCollection<PowerLayerObj> powerLayers_ { get; set; } = new ObservableCollection<PowerLayerObj>();
        public PowerLayerPainter painter_ { get; set; }
        LinesJSONFetcher linesJSONFetcher_;

        public PowerMap(Canvas canvas)
        {
            painter_ = new PowerLayerPainter();
            linesJSONFetcher_ = new LinesJSONFetcher();

            painter_.canvas_ = canvas;

            powerLayers_.CollectionChanged += new NotifyCollectionChangedEventHandler(CollectionChangedMethod);

            // initialte the layers from the json file
            List<PowerLayerObj> layerObjs = linesJSONFetcher_.fetchPowerLayerObjs(File.ReadAllText("lines_ddl.json"));
            foreach (PowerLayerObj layerObj in layerObjs)
            {
                powerLayers_.Add(layerObj);
            }            

            // temporary
            recalculatePolylines();
            assignLinePowers();
            assignLineColors();
            paintLayers();
        }

        public void paintLayers()
        {
            foreach (PowerLayerObj powerLayerObj in powerLayers_)
            {
                bool isVisible = powerLayerObj.IsLayerVisible;
                foreach (PowerLayerLineObj powerLayerLineObj in powerLayerObj.powerLayerLineObjs_)
                {
                    if (isVisible)
                    {
                        // add all the layer lines to the canvas if isVisible is true
                        painter_.addElementToCanvas(powerLayerLineObj.polyLine_);
                    }
                    else
                    {
                        // remove all the layer lines from the canvas if isVisible is false
                        painter_.removeElementFromCanvas(powerLayerLineObj.polyLine_);
                    }
                }
            }
        }

        public void recalculatePolylines()
        {
            foreach (PowerLayerObj powerLayerObj in powerLayers_)
            {
                foreach (PowerLayerLineObj powerLayerLineObj in powerLayerObj.powerLayerLineObjs_)
                {
                    powerLayerLineObj.polyLine_.Points = painter_.getPowerLineCanvasPoints(powerLayerLineObj);
                }
            }
        }

        public void assignLinePowers()
        {
            foreach (PowerLayerObj powerLayerObj in powerLayers_)
            {
                foreach (PowerLayerLineObj powerLayerLineObj in powerLayerObj.powerLayerLineObjs_)
                {
                    powerLayerLineObj.lineDataObj_.Power = 100.0;
                }
            }
        }

        public void assignLineColors()
        {
            foreach (PowerLayerObj powerLayerObj in powerLayers_)
            {
                foreach (PowerLayerLineObj powerLayerLineObj in powerLayerObj.powerLayerLineObjs_)
                {
                    powerLayerLineObj.polyLine_.Stroke = new SolidColorBrush(painter_.color_);
                }
            }
        }

        private void CollectionChangedMethod(object sender, NotifyCollectionChangedEventArgs e)
        {
            // https://stackoverflow.com/questions/7316591/collectionchanged-event-of-observablecollection-in-c-sharp
            // https://www.codeproject.com/Tips/694370/How-to-Listen-to-Property-Chang

            //different kind of changes that may have occurred in collection

            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                RegisterPropertyChanged(e.NewItems);

            }
            if (e.Action == NotifyCollectionChangedAction.Replace)
            {
                UnRegisterPropertyChanged(e.OldItems);
                RegisterPropertyChanged(e.NewItems);
            }
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                UnRegisterPropertyChanged(e.OldItems);
            }
            if (e.Action == NotifyCollectionChangedAction.Move)
            {
                // our code
            }
        }

        private void RegisterPropertyChanged(IList items)
        {
            foreach (INotifyPropertyChanged item in items)
            {
                if (item != null)
                {
                    // subscribe to the property changes in lineDataObject so that we can notify the painter
                    item.PropertyChanged += HandlePropertyChanged;
                }
            }
        }

        private void UnRegisterPropertyChanged(IList items)
        {
            foreach (INotifyPropertyChanged item in items)
            {
                if (item != null)
                {
                    // unsubscribe to the property changes in lineDataObject
                    item.PropertyChanged -= HandlePropertyChanged;
                }
            }
        }

        // listener that subscribes for line object change event
        private void HandlePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // todo if the property changes are not related to drawing, then handle them here instead of sending to painter
            // call the event handler of the painter
            painter_.HandlePropertyChanged(sender, e);
        }
    }
}
