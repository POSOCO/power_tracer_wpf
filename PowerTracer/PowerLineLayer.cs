using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace PowerTracer
{
    public class PowerLineLayer
    {
        public string name_ { get; set; }
        public List<PowerLine> layerLines_ { get; set; }
        public bool isVisible_ { get; set; } = true;
    }
}
