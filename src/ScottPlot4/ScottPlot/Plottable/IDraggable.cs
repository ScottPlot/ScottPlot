using System;

namespace ScottPlot.Plottable
{
    public interface IDraggable
    {
        bool DragEnabled { get; set; }
        Cursor DragCursor { get; }
        bool IsUnderMouse(double coordinateX, double coordinateY, double snapX, double snapY);
        void DragTo(double coordinateX, double coordinateY, bool fixedSize);
        public SnapLogic.ISnap2D DragSnap { get; set; }
        event EventHandler Dragged;
    }
}
