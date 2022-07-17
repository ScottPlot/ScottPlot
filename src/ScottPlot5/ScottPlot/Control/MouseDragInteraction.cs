using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Control
{
    public class MouseDragInteraction : BaseEventArgs
    {
        public MouseDownInteraction MouseDown { get; }
        public Pixel From => MouseDown.Position;
        public Pixel To { get; }
        public MouseButton Button { get; }

        public MouseDragInteraction(MouseDownInteraction mouseDown, Pixel to, MouseButton button)
        {
            MouseDown = mouseDown;
            To = to;
            Button = button;
        }
    }
}
