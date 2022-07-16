using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Control
{
    public enum MouseButton
    {
        Mouse1,
        Mouse2,
        Mouse3,
    }

    public struct MouseInteraction
    {
        public Pixel Position { get; set; }
        public MouseButton Button { get; set; }
        public AxisLimits AxisLimits { get; set; }
    }
}
