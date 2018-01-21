using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerTracer
{
    class PowerLayerObj
    {
        string layerName_ { get; set; } = "Unknown";
        ObservableCollection<PowerLayerLineObj> powerLayerLineObjs_ { get; set; }
        // todo subscribe to changes in powerLayerLineObjs_
    }
}
