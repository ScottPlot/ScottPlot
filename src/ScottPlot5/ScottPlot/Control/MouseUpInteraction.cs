using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Control
{
    public struct MouseUpInteraction
    {
        public Pixel Position { get; set; }
        public MouseButton Button { get; set; }
        public AxisLimits AxisLimits { get; set; }
        public bool CancelledDrag { get; set; }
    }
}
