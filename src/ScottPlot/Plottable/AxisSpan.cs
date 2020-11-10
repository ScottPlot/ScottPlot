using ScottPlot.Ticks;
using ScottPlot.Drawing;
using ScottPlot.Renderable;
using System;
using System.Diagnostics;
using System.Drawing;

namespace ScottPlot.Plottable
{
    public class HSpan : AxisSpan { public HSpan() { IsHorizontal = true; } }
    public class VSpan : AxisSpan { public VSpan() { IsHorizontal = false; } }

    public abstract class AxisSpan : IDraggable, IRenderable, IHasLegendItems, IUsesAxes
    {
        public double position1;
        public double position2;
        private double Min { get => Math.Min(position1, position2); }
        private double Max { get => Math.Max(position1, position2); }
        public int HorizontalAxisIndex { get; set; } = 0;
        public int VerticalAxisIndex { get; set; } = 0;

        public Color color;
        public Color colorWithAlpha => Color.FromArgb((byte)(255 * alpha), color);
        public double alpha = .35;
        public string label;
        public bool IsHorizontal = true;
        public bool DragEnabled { get; set; }
        public bool DragFixedSize { get; set; }
        public Cursor DragCursor => IsHorizontal ? Cursor.WE : Cursor.NS;
        public bool IsVisible { get; set; } = true;

        public override string ToString()
        {
            string label = string.IsNullOrWhiteSpace(this.label) ? "" : $" ({this.label})";
            return IsHorizontal ?
                $"PlottableVSpan{label} from X={position1} to X={position2}" :
                $"PlottableVSpan{label} from Y={position1} to Y={position2}";
        }

        public LegendItem[] LegendItems
        {
            get
            {
                var item = new LegendItem()
                {
                    label = label,
                    color = colorWithAlpha,
                    markerSize = 0,
                    lineWidth = 10
                };
                return new LegendItem[] { item };
            }
        }

        public (double xMin, double xMax, double yMin, double yMax) GetAxisLimits() =>
            IsHorizontal ? (Min, Max, double.NaN, double.NaN) : (double.NaN, double.NaN, Min, Max);

        private double dragLimitX1 = double.NegativeInfinity;
        private double dragLimitX2 = double.PositiveInfinity;
        private double dragLimitY1 = double.NegativeInfinity;
        private double dragLimitY2 = double.PositiveInfinity;
        public void SetLimits(double? x1, double? x2, double? y1, double? y2)
        {
            dragLimitX1 = x1 ?? dragLimitX1;
            dragLimitX2 = x2 ?? dragLimitX2;
            dragLimitY1 = y1 ?? dragLimitY1;
            dragLimitY2 = y2 ?? dragLimitY2;
        }

        private enum Edge { Edge1, Edge2, Neither };
        Edge edgeUnderMouse = Edge.Neither;
        public bool IsUnderMouse(double coordinateX, double coordinateY, double snapX, double snapY)
        {
            if (IsHorizontal)
            {
                if (Math.Abs(position1 - coordinateX) <= snapX)
                    edgeUnderMouse = Edge.Edge1;
                else if (Math.Abs(position2 - coordinateX) <= snapX)
                    edgeUnderMouse = Edge.Edge2;
                else
                    edgeUnderMouse = Edge.Neither;
            }
            else
            {
                if (Math.Abs(position1 - coordinateY) <= snapY)
                    edgeUnderMouse = Edge.Edge1;
                else if (Math.Abs(position2 - coordinateY) <= snapY)
                    edgeUnderMouse = Edge.Edge2;
                else
                    edgeUnderMouse = Edge.Neither;
            }

            return edgeUnderMouse != Edge.Neither;
        }

        public void DragTo(double coordinateX, double coordinateY, bool isShiftDown = false, bool isAltDown = false, bool isCtrlDown = false)
        {
            if (!DragEnabled)
                return;

            if (coordinateX < dragLimitX1) coordinateX = dragLimitX1;
            if (coordinateX > dragLimitX2) coordinateX = dragLimitX2;
            if (coordinateY < dragLimitY1) coordinateY = dragLimitY1;
            if (coordinateY > dragLimitY2) coordinateY = dragLimitY2;

            double sizeBeforeDrag = position2 - position1;
            if (edgeUnderMouse == Edge.Edge1)
            {
                position1 = IsHorizontal ? coordinateX : coordinateY;
                if (DragFixedSize || isShiftDown)
                    position2 = position1 + sizeBeforeDrag;
            }
            else if (edgeUnderMouse == Edge.Edge2)
            {
                position2 = IsHorizontal ? coordinateX : coordinateY;
                if (DragFixedSize || isShiftDown)
                    position1 = position2 - sizeBeforeDrag;
            }
            else
            {
                Debug.WriteLine("DragTo() called but no side selected. Call IsUnderMouse() to select a side.");
            }

            // ensure fixed-width spans stay entirely inside the allowable range
            double lowerLimit = IsHorizontal ? dragLimitX1 : dragLimitY1;
            double upperLimit = IsHorizontal ? dragLimitX2 : dragLimitY2;
            double belowLimit = lowerLimit - position1;
            double aboveLimit = position2 - upperLimit;
            if (belowLimit > 0)
            {
                position1 += belowLimit;
                position2 += belowLimit;
            }
            if (aboveLimit > 0)
            {
                position1 -= aboveLimit;
                position2 -= aboveLimit;
            }
        }

        public void Render(PlotDimensions2D dims, Bitmap bmp, bool lowQuality = false)
        {
            PointF p1, p2;
            if (IsHorizontal)
            {
                p1 = new PointF(dims.GetPixelX(Min), dims.GetPixelY(dims.YMin));
                p2 = new PointF(dims.GetPixelX(Max), dims.GetPixelY(dims.YMax));
            }
            else
            {
                p1 = new PointF(dims.GetPixelX(dims.XMin), dims.GetPixelY(Min));
                p2 = new PointF(dims.GetPixelX(dims.XMax), dims.GetPixelY(Max));
            }
            RectangleF rect = new RectangleF(p1.X - 1, p2.Y - 1, p2.X - p1.X + 1, p1.Y - p2.Y + 1);

            using (var gfx = GDI.Graphics(bmp, dims, lowQuality))
            using (var brush = GDI.Brush(colorWithAlpha))
            {
                gfx.FillRectangle(brush, rect);
            }
        }
    }
}
