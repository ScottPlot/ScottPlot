using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Control
{
    public struct MouseDownInteraction
    {
        public Pixel Position { get; set; }
        public MouseButton Button { get; set; }
        public AxisLimits AxisLimits { get; set; }
    }
}
