using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Control
{
    public class MouseMoveInteraction : BaseEventArgs
    {
        public Pixel Position { get; }
        public MouseMoveInteraction(Pixel position)
        {
            Position = position;
        }

    }
}
