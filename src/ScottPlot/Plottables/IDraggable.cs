using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Plottables
{
    public interface IDraggable
    {
        bool isDragEnabled { get; set; }
        Config.Cursor dragCursor { get; }
        void SetLimits(double? x1, double? x2, double? y1, double? y2);
        bool IsUnderMouse(double coordinateX, double coordinateY, double snapX, double snapY);
        void DragTo(double coordinateX, double coordinateY);
    }
}
