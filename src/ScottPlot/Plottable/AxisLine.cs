using ScottPlot.Ticks;
using ScottPlot.Drawing;
using System;
using System.Drawing;

namespace ScottPlot.Plottable
{
    public class HLine : AxisLine { public HLine() { IsHorizontal = true; } }

    public class VLine : AxisLine { public VLine() { IsHorizontal = false; } }

    public abstract class AxisLine : IDraggable, IPlottable
    {
        public double position;
        public int HorizontalAxisIndex { get; set; } = 0;
        public int VerticalAxisIndex { get; set; } = 0;

        public LineStyle lineStyle = LineStyle.Solid;
        public float lineWidth = 1;
        public Color color = Color.Black;
        public string label;
        public bool IsHorizontal = true;

        public bool DragEnabled { get; set; } = false;
        public Cursor DragCursor => IsHorizontal ? Cursor.NS : Cursor.WE;
        public double DragLimitMin = double.NegativeInfinity;
        public double DragLimitMax = double.PositiveInfinity;

        public bool IsVisible { get; set; } = true;

        public override string ToString()
        {
            string label = string.IsNullOrWhiteSpace(this.label) ? "" : $" ({this.label})";
            return IsHorizontal ?
                $"PlottableHLine{label} at Y={position}" :
                $"PlottableVLine{label} at X={position}";
        }

        public AxisLimits GetAxisLimits() =>
            IsHorizontal ?
            new AxisLimits(double.NaN, double.NaN, position, position) :
            new AxisLimits(position, position, double.NaN, double.NaN);

        public void ValidateData(bool deep = false)
        {
            if (double.IsNaN(position) || double.IsInfinity(position))
                throw new InvalidOperationException("position must be a valid number");
        }

        public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            using (var gfx = GDI.Graphics(bmp, dims, lowQuality))
            using (var pen = GDI.Pen(color, lineWidth, lineStyle, true))
            {
                if (IsHorizontal)
                {
                    float pixelX1 = dims.GetPixelX(dims.XMin);
                    float pixelX2 = dims.GetPixelX(dims.XMax);
                    float pixelY = dims.GetPixelY(position);
                    gfx.DrawLine(pen, pixelX1, pixelY, pixelX2, pixelY);
                }
                else
                {
                    float pixelX = dims.GetPixelX(position);
                    float pixelY1 = dims.GetPixelY(dims.YMin);
                    float pixelY2 = dims.GetPixelY(dims.YMax);
                    gfx.DrawLine(pen, pixelX, pixelY1, pixelX, pixelY2);
                }
            }
        }

        public void DragTo(double coordinateX, double coordinateY, bool isShiftDown = false, bool isAltDown = false, bool isCtrlDown = false)
        {
            if (!DragEnabled)
                return;

            if (IsHorizontal)
            {
                if (coordinateY < DragLimitMin) coordinateY = DragLimitMin;
                if (coordinateY > DragLimitMax) coordinateY = DragLimitMax;
                position = coordinateY;
            }
            else
            {
                if (coordinateX < DragLimitMin) coordinateX = DragLimitMin;
                if (coordinateX > DragLimitMax) coordinateX = DragLimitMax;
                position = coordinateX;
            }
        }

        public bool IsUnderMouse(double coordinateX, double coordinateY, double snapX, double snapY) =>
            IsHorizontal ?
            Math.Abs(position - coordinateY) <= snapY :
            Math.Abs(position - coordinateX) <= snapX;

        public LegendItem[] GetLegendItems()
        {
            var singleItem = new LegendItem()
            {
                label = label,
                color = color,
                lineStyle = lineStyle,
                lineWidth = lineWidth,
                markerShape = MarkerShape.none
            };
            return new LegendItem[] { singleItem };
        }
    }
}
