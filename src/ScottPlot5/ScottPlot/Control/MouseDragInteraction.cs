using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Control
{
    public struct MouseDragInteraction
    {
        public MouseInteraction MouseDown { get; set; }
        public Pixel From => MouseDown.Position;
        public Pixel To { get; set; }
        public MouseButton Button { get; set; }
    }
}
