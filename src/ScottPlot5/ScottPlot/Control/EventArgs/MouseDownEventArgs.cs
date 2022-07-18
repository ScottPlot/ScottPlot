using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Control.EventArgs
{
    public class MouseDownEventArgs : BaseEventArgs
    {
        public Pixel Position { get; }
        public MouseButton Button { get; }
        public AxisLimits AxisLimits { get; }
        public IReadOnlyCollection<Key> PressedKeys { get; }

        public MouseDownEventArgs(Pixel position, MouseButton button, AxisLimits axisLimits, IReadOnlyCollection<Key> pressedKeys)
        {
            Position = position;
            Button = button;
            AxisLimits = axisLimits;
            PressedKeys = pressedKeys;
        }
    }
}
