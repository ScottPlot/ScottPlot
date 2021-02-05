using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.Control
{
    public class DisplayScale
    {
        public float ScaleFactor { get; private set; }

        public DisplayScale()
        {
            Measure();
        }

        public void Measure() => ScaleFactor = ScottPlot.Drawing.GDI.GetDPIScale();
    }
}
