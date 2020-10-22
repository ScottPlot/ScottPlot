using ScottPlot.Config;
using ScottPlot.Drawing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Threading;

namespace ScottPlot
{
    public class PlottableHSpan : PlottableAxisSpan { public PlottableHSpan() { IsHorizontal = true; } }
    public class PlottableVSpan : PlottableAxisSpan { public PlottableVSpan() { IsHorizontal = false; } }

    public abstract class PlottableAxisSpan : Plottable, IDraggable, IPlottable
    {
        public double position1;
        public double position2;
        private double Min { get => Math.Min(position1, position2); }
        private double Max { get => Math.Max(position1, position2); }

        public Color color;
        public Color colorWithAlpha => Color.FromArgb((byte)(255 * alpha), color);
        public double alpha = .35;
        public string label;
        public override int GetPointCount() => 1;
        public bool IsHorizontal = true;
        public bool DragEnabled { get; set; }
        public bool DragFixedSize { get; set; }
        public Cursor DragCursor => IsHorizontal ? Cursor.WE : Cursor.NS;

        public override string ToString()
        {
            string label = string.IsNullOrWhiteSpace(this.label) ? "" : $" ({this.label})";
            return IsHorizontal ?
                $"PlottableVSpan{label} from X={position1} to X={position2}" :
                $"PlottableVSpan{label} from Y={position1} to Y={position2}";
        }

        public override LegendItem[] GetLegendItems() =>
             new LegendItem[] { new LegendItem(label, colorWithAlpha, markerSize: 0, lineWidth: 10) };

        public override AxisLimits2D GetLimits() =>
            IsHorizontal ? new AxisLimits2D(Min, Max, null, null) : new AxisLimits2D(null, null, Min, Max);

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

        public override void Render(Settings settings) =>
            throw new InvalidOperationException("use new render method");

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

            using (var gfx = GDI.Graphics(bmp, lowQuality))
            using (var brush = GDI.Brush(colorWithAlpha))
            {
                gfx.FillRectangle(brush, rect);
            }
        }

        public string ValidationErrorMessage { get; private set; }
        public bool IsValidData(bool deepValidation = false)
        {
            if (double.IsInfinity(position1) || double.IsNaN(position1) ||
                double.IsInfinity(position2) || double.IsNaN(position2))
            {
                ValidationErrorMessage = "positions must be finite";
                return false;
            }

            ValidationErrorMessage = null;
            return true;
        }
    }
}
