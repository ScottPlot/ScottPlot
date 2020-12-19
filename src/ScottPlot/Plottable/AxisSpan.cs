using ScottPlot.Ticks;
using ScottPlot.Drawing;
using System;
using System.Diagnostics;
using System.Drawing;

namespace ScottPlot.Plottable
{
    /// <summary>
    /// Shaded horizontal region between two X values
    /// </summary>
    public class HSpan : AxisSpan
    {
        public double X1 { get => Position1; set => Position1 = value; }
        public double X2 { get => Position2; set => Position2 = value; }
        public HSpan() : base(true) { }
        public override string ToString() => $"Horizontal span between Y1={X1} and Y2={X2}";
    }

    /// <summary>
    /// Shade the region between two Y values
    /// </summary>
    public class VSpan : AxisSpan
    {
        public double Y1 { get => Position1; set => Position1 = value; }
        public double Y2 { get => Position2; set => Position2 = value; }
        public VSpan() : base(false) { }
        public override string ToString() => $"Vertical span between X1={Y1} and X2={Y2}";
    }

    public abstract class AxisSpan : IPlottable, IDraggable
    {
        // location and orientation
        protected double Position1;
        protected double Position2;
        private double Min { get => Math.Min(Position1, Position2); }
        private double Max { get => Math.Max(Position1, Position2); }
        readonly bool IsHorizontal;

        // configuration
        public int XAxisIndex { get; set; } = 0;
        public int YAxisIndex { get; set; } = 0;
        public bool IsVisible { get; set; } = true;
        public Color Color = Color.FromArgb(128, Color.Magenta);
        public string Label;

        // mouse interaction
        public bool DragEnabled { get; set; }
        public bool DragFixedSize { get; set; }
        public double DragLimitMin = double.NegativeInfinity;
        public double DragLimitMax = double.PositiveInfinity;
        public Cursor DragCursor => IsHorizontal ? Cursor.WE : Cursor.NS;
        public event EventHandler Dragged = delegate { };

        public AxisSpan(bool isHorizontal)
        {
            IsHorizontal = isHorizontal;
        }

        public void ValidateData(bool deep = false)
        {
            if (double.IsNaN(Position1) || double.IsInfinity(Position1))
                throw new InvalidOperationException("position1 must be a valid number");

            if (double.IsNaN(Position2) || double.IsInfinity(Position2))
                throw new InvalidOperationException("position2 must be a valid number");
        }

        public LegendItem[] GetLegendItems()
        {
            var singleItem = new LegendItem()
            {
                label = Label,
                color = Color,
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
                if (Math.Abs(Position1 - coordinateX) <= snapX)
                    edgeUnderMouse = Edge.Edge1;
                else if (Math.Abs(Position2 - coordinateX) <= snapX)
                    edgeUnderMouse = Edge.Edge2;
                else
                    edgeUnderMouse = Edge.Neither;
            }
            else
            {
                if (Math.Abs(Position1 - coordinateY) <= snapY)
                    edgeUnderMouse = Edge.Edge1;
                else if (Math.Abs(Position2 - coordinateY) <= snapY)
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

            double sizeBeforeDrag = Position2 - Position1;
            if (edgeUnderMouse == Edge.Edge1)
            {
                Position1 = IsHorizontal ? coordinateX : coordinateY;
                if (DragFixedSize || isShiftDown)
                    Position2 = Position1 + sizeBeforeDrag;
            }
            else if (edgeUnderMouse == Edge.Edge2)
            {
                Position2 = IsHorizontal ? coordinateX : coordinateY;
                if (DragFixedSize || isShiftDown)
                    Position1 = Position2 - sizeBeforeDrag;
            }
            else
            {
                Debug.WriteLine("DragTo() called but no side selected. Call IsUnderMouse() to select a side.");
            }

            // ensure fixed-width spans stay entirely inside the allowable range
            double belowLimit = DragLimitMin - Position1;
            double aboveLimit = Position2 - DragLimitMax;
            if (belowLimit > 0)
            {
                Position1 += belowLimit;
                Position2 += belowLimit;
            }
            if (aboveLimit > 0)
            {
                Position1 -= aboveLimit;
                Position2 -= aboveLimit;
            }

            Dragged(this, EventArgs.Empty);
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
            using (var brush = GDI.Brush(Color))
            {
                gfx.FillRectangle(brush, rect);
            }
        }
    }
}
