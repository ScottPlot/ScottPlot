using ScottPlot.Ticks;
using ScottPlot.Drawing;
using ScottPlot.Renderable;
using System;
using System.Drawing;

namespace ScottPlot.Plottable
{
    public class HLine : AxisLine { public HLine() { IsHorizontal = true; } }

    public class VLine : AxisLine { public VLine() { IsHorizontal = false; } }

    public abstract class AxisLine : IRenderable, IDraggable, IHasLegendItems, IUsesAxes
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

        public bool IsVisible { get; set; } = true;
        public override string ToString()
        {
            string label = string.IsNullOrWhiteSpace(this.label) ? "" : $" ({this.label})";
            return IsHorizontal ?
                $"PlottableHLine{label} at Y={position}" :
                $"PlottableVLine{label} at X={position}";
        }

        public AxisLimits2D GetAxisLimits() =>
            IsHorizontal ?
            new AxisLimits2D(double.NaN, double.NaN, position, position) :
            new AxisLimits2D(position, position, double.NaN, double.NaN);

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

        public void DragTo(double coordinateX, double coordinateY, bool isShiftDown = false, bool isAltDown = false, bool isCtrlDown = false)
        {
            if (!DragEnabled)
                return;

            if (IsHorizontal)
            {
                if (coordinateY < dragLimitY1) coordinateY = dragLimitY1;
                if (coordinateY > dragLimitY2) coordinateY = dragLimitY2;
                position = coordinateY;
            }
            else
            {
                if (coordinateX < dragLimitX1) coordinateX = dragLimitX1;
                if (coordinateX > dragLimitX2) coordinateX = dragLimitX2;
                position = coordinateX;
            }
        }

        public bool IsUnderMouse(double coordinateX, double coordinateY, double snapX, double snapY) =>
            IsHorizontal ?
            Math.Abs(position - coordinateY) <= snapY :
            Math.Abs(position - coordinateX) <= snapX;

        public LegendItem[] LegendItems
        {
            get
            {
                var item = new LegendItem()
                {
                    label = label,
                    color = color,
                    lineStyle = lineStyle,
                    lineWidth = lineWidth,
                    markerShape = MarkerShape.none
                };
                return new LegendItem[] { item };
            }
        }
    }
}
