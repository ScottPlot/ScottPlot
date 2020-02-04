using ScottPlot.Config;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlot.Plottables
{
    public abstract class AxisSpan : IPlottable, IDraggable
    {
        // interface stuff
        public bool visible { get; set; } = true;
        public int pointCount { get { return 1; } }
        public string label { get; set; }
        public Color color { get; set; }
        public MarkerShape markerShape { get; set; }
        public LineStyle lineStyle { get; set; }
        public abstract void Render(Context renderContext);
        public abstract AxisLimits2D GetLimits();

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
