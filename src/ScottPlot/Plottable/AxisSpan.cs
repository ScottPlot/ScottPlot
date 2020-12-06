using ScottPlot.Ticks;
using ScottPlot.Drawing;
using System;
using System.Diagnostics;
using System.Drawing;

namespace ScottPlot.Plottable
{
    public class HSpan : AxisSpan { public HSpan() { IsHorizontal = true; } }
    public class VSpan : AxisSpan { public VSpan() { IsHorizontal = false; } }

    public abstract class AxisSpan : IPlottable, IDraggable
    {
        public double position1;
        public double position2;
        private double Min { get => Math.Min(position1, position2); }
        private double Max { get => Math.Max(position1, position2); }
        public int HorizontalAxisIndex { get; set; } = 0;
        public int VerticalAxisIndex { get; set; } = 0;
        public bool IsVisible { get; set; } = true;

        public Color color;
        public string label;
        public bool IsHorizontal = true;
        public bool DragEnabled { get; set; }
        public bool DragFixedSize { get; set; }
        public double DragLimitMin = double.NegativeInfinity;
        public double DragLimitMax = double.PositiveInfinity;
        public Cursor DragCursor => IsHorizontal ? Cursor.WE : Cursor.NS;

        public override string ToString()
        {
            string label = string.IsNullOrWhiteSpace(this.label) ? "" : $" ({this.label})";
            return IsHorizontal ?
                $"PlottableVSpan{label} from X={position1} to X={position2}" :
                $"PlottableVSpan{label} from Y={position1} to Y={position2}";
        }

        public void ValidateData(bool deep = false)
        {
            if (double.IsNaN(position1) || double.IsInfinity(position1))
                throw new InvalidOperationException("position1 must be a valid number");

            if (double.IsNaN(position2) || double.IsInfinity(position2))
                throw new InvalidOperationException("position2 must be a valid number");
        }

        public LegendItem[] GetLegendItems()
        {
            var singleItem = new LegendItem()
            {
                label = label,
                color = color,
                markerSize = 0,
                lineWidth = 10
            };
            return new LegendItem[] { singleItem };
        }

        public AxisLimits GetAxisLimits() =>
            IsHorizontal ?
            new AxisLimits(Min, Max, double.NaN, double.NaN) :
            new AxisLimits(double.NaN, double.NaN, Min, Max);

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

            if (IsHorizontal)
            {
                coordinateX = Math.Max(coordinateX, DragLimitMin);
                coordinateX = Math.Min(coordinateX, DragLimitMax);
            }
            else
            {
                coordinateY = Math.Max(coordinateY, DragLimitMin);
                coordinateY = Math.Min(coordinateY, DragLimitMax);
            }

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
            double belowLimit = DragLimitMin - position1;
            double aboveLimit = position2 - DragLimitMax;
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

        public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
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
            using (var brush = GDI.Brush(color))
            {
                gfx.FillRectangle(brush, rect);
            }
        }
    }
}
