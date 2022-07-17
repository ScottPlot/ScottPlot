using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Control
{
    public class MouseDownInteraction : BaseEventArgs
    {
        public Pixel Position { get; }
        public MouseButton Button { get; }
        public AxisLimits AxisLimits { get; }

        public MouseDownInteraction(Pixel position, MouseButton button, AxisLimits axisLimits)
        {
            Position = position;
            Button = button;
            AxisLimits = axisLimits;
        }
    }
}
