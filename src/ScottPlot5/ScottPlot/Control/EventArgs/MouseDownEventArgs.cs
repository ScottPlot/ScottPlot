using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Control.EventArgs
{
    public class MouseDownEventArgs : BaseEventArgs
    {
        /// <summary>
        /// Location of the mouse
        /// </summary>
        public Pixel Position { get; }

        /// <summary>
        /// Which button was pressed or released
        /// </summary>
        public MouseButton Button { get; }

        /// <summary>
        /// Axis limits when the mouse was first engaged.
        /// If click-dragging, these are the limits before the drag.
        /// </summary>
        public AxisLimits AxisLimits { get; }

        /// <summary>
        /// Keys currently held down
        /// </summary>
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
