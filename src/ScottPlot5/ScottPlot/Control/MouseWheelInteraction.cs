using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Control
{
    public class MouseWheelInteraction : BaseEventArgs
    {
        public Pixel Position { get; }
        public float DeltaX { get; }
        public float DeltaY { get; }
        
        public MouseWheelInteraction(Pixel position, float deltaX, float deltaY)
        {
            Position = position;
            DeltaX = deltaX;
            DeltaY = deltaY;
        }
    }
}
