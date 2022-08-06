using System;

namespace ScottPlot.Plottable
{
    public interface IDraggable
    {
        bool DragEnabled { get; set; }
        Cursor DragCursor { get; }
        bool IsUnderMouse(double coordinateX, double coordinateY, double snapX, double snapY);
        void DragTo(double coordinateX, double coordinateY, bool fixedSize);
        public Func<double, double> DragSnapX { get; set; }
        public Func<double, double> DragSnapY { get; set; }
        event EventHandler Dragged;
    }
}
