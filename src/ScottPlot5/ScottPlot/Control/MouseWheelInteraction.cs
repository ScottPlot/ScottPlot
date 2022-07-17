using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Control
{
    public struct MouseWheelInteraction
    {
        public Pixel Position { get; set; }
        public float DeltaY { get; set; }
        public float DeltaX { get; set; }
    }
}
