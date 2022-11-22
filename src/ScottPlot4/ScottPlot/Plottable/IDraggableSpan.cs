using System;

namespace ScottPlot.Plottable
{
    public interface IDraggableSpan
    {
        public event EventHandler<double> Edge1Dragged;
        public event EventHandler<double> Edge2Dragged;
        public event EventHandler<double> MinDragged;
        public event EventHandler<double> MaxDragged;
    }
}
