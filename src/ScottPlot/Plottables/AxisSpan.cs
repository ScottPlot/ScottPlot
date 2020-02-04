using ScottPlot.Config;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Plottables
{
    public abstract class AxisSpan : Plottable, IDraggable
    {
        public bool isDragEnabled { get; set; }
        public double position1 { get; protected set; }
        public double position2 { get; protected set; }
        public AxisLimits2D limits = new AxisLimits2D();
        public abstract Cursor dragCursor { get; }
        public abstract void SetLimits(double? x1, double? x2, double? y1, double? y2);
        public abstract bool IsUnderMouse(double coordinateX, double coordinateY, double snapX, double snapY);
        public abstract void DragTo(double coordinateX, double coordinateY);
    }
}
