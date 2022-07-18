using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Control.EventArgs
{
    public class MouseUpEventArgs : BaseEventArgs
    {
        public Pixel Position { get; }
        public MouseButton Button { get; }
        public AxisLimits AxisLimits { get; }
        public bool CancelledDrag { get; }

        public MouseUpEventArgs(Pixel position, MouseButton button, AxisLimits axisLimits, bool cancelledDrag)
        {
            Position = position;
            Button = button;
            AxisLimits = axisLimits;
            CancelledDrag = cancelledDrag;
        }
    }
}
