using ScottPlot.Drawing;
using System;
using System.Drawing;
using System.Data;
using System.Linq;

namespace ScottPlot.Plottable
{
    /// <summary>
    /// Bar plots display a series of bars. 
    /// Positions are defined by Xs.
    /// Heights are defined by Ys (relative to BaseValue and YOffsets).
    /// </summary>
    public class BarPlot : BarPlotBase, IPlottable, IHasColor
    {
        public string Label { get; set; }
        public Color Color { get => FillColor; set { FillColor = value; } }
        public Color FillColor { get; set; } = Color.Green;
        public Color FillColorNegative { get; set; } = Color.Red;
        public Color FillColorHatch { get; set; } = Color.Blue;
        public HatchStyle HatchStyle { get; set; } = HatchStyle.None;
        public float BorderLineWidth { get; set; } = 1;

        public BarPlot(double[] xs, double[] ys, double[] yErr, double[] yOffsets) : base()
        {
            if (ys is null || ys.Length == 0)
                throw new InvalidOperationException("ys must be an array that contains elements");

            if (xs != null && ys.Length != xs.Length)
                throw new ArgumentException("bar plot Xs and Ys must have the same number of elements");

            Values = ys;
            Positions = xs ?? DataGen.Consecutive(ys.Length);
            ValueErrors = yErr ?? DataGen.Zeros(ys.Length);
            ValueOffsets = yOffsets ?? DataGen.Zeros(ys.Length);
        }

        public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            using Graphics gfx = GDI.Graphics(bmp, dims, lowQuality);
            for (int barIndex = 0; barIndex < Values.Length; barIndex++)
            {
                if (Orientation == Orientation.Vertical)
                    RenderBarVertical(dims, gfx, Positions[barIndex] + PositionOffset, Values[barIndex], ValueErrors[barIndex], ValueOffsets[barIndex]);
                else
                    RenderBarHorizontal(dims, gfx, Positions[barIndex] + PositionOffset, Values[barIndex], ValueErrors[barIndex], ValueOffsets[barIndex]);
            }
        }

        private void RenderBarVertical(PlotDimensions dims, Graphics gfx, double position, double value, double valueError, double yOffset)
        {
            // bar body
            float centerPx = dims.GetPixelX(position);
            double edge1 = position - BarWidth / 2;
            double value1 = Math.Min(ValueBase, value) + yOffset;
            double value2 = Math.Max(ValueBase, value) + yOffset;
            double valueSpan = value2 - value1;

            var rect = new RectangleF(
                x: dims.GetPixelX(edge1),
                y: dims.GetPixelY(value2),
                width: (float)(BarWidth * dims.PxPerUnitX),
                height: (float)(valueSpan * dims.PxPerUnitY));

            // errorbar
            double error1 = value > 0 ? value2 - Math.Abs(valueError) : value1 - Math.Abs(valueError);
            double error2 = value > 0 ? value2 + Math.Abs(valueError) : value1 + Math.Abs(valueError);
            float capPx1 = dims.GetPixelX(position - ErrorCapSize * BarWidth / 2);
            float capPx2 = dims.GetPixelX(position + ErrorCapSize * BarWidth / 2);
            float errorPx2 = dims.GetPixelY(error2);
            float errorPx1 = dims.GetPixelY(error1);

            RenderBarFromRect(ClipRectToDataArea(rect, dims), value < 0, gfx);

            if (ErrorLineWidth > 0 && valueError > 0)
            {
                using var errorPen = new Pen(ErrorColor, ErrorLineWidth);
                gfx.DrawLine(errorPen, centerPx, errorPx1, centerPx, errorPx2);
                gfx.DrawLine(errorPen, capPx1, errorPx1, capPx2, errorPx1);
                gfx.DrawLine(errorPen, capPx1, errorPx2, capPx2, errorPx2);
            }

            if (ShowValuesAboveBars)
            {
                bool belowBar = value < 0;

                using var valueTextFont = GDI.Font(Font);
                using var valueTextBrush = GDI.Brush(Font.Color);
                using var sf = new StringFormat()
                {
                    LineAlignment = belowBar ? StringAlignment.Near : StringAlignment.Far,
                    Alignment = StringAlignment.Center
                };

                var y = belowBar ? rect.Bottom : rect.Top;
                gfx.DrawString(ValueFormatter(value), valueTextFont, valueTextBrush, centerPx, y, sf);
            }
        }

        private void RenderBarHorizontal(PlotDimensions dims, Graphics gfx, double position, double value, double valueError, double yOffset)
        {
            // bar body
            float centerPx = dims.GetPixelY(position);
            double edge2 = position + BarWidth / 2;
            double value1 = Math.Min(ValueBase, value) + yOffset;
            double value2 = Math.Max(ValueBase, value) + yOffset;
            double valueSpan = value2 - value1;
            var rect = new RectangleF(
                x: dims.GetPixelX(value1),
                y: dims.GetPixelY(edge2),
                height: (float)(BarWidth * dims.PxPerUnitY),
                width: (float)(valueSpan * dims.PxPerUnitX));

            RenderBarFromRect(ClipRectToDataArea(rect, dims), value < 0, gfx);

            // errorbar
            double error1 = value > 0 ? value2 - Math.Abs(valueError) : value1 - Math.Abs(valueError);
            double error2 = value > 0 ? value2 + Math.Abs(valueError) : value1 + Math.Abs(valueError);
            float capPx1 = dims.GetPixelY(position - ErrorCapSize * BarWidth / 2);
            float capPx2 = dims.GetPixelY(position + ErrorCapSize * BarWidth / 2);
            float errorPx2 = dims.GetPixelX(error2);
            float errorPx1 = dims.GetPixelX(error1);

            if (ErrorLineWidth > 0 && valueError > 0)
            {
                using var errorPen = new Pen(ErrorColor, ErrorLineWidth);
                gfx.DrawLine(errorPen, errorPx1, centerPx, errorPx2, centerPx);
                gfx.DrawLine(errorPen, errorPx1, capPx2, errorPx1, capPx1);
                gfx.DrawLine(errorPen, errorPx2, capPx2, errorPx2, capPx1);
            }

            if (ShowValuesAboveBars)
            {
                bool belowBar = value < 0;

                using var valueTextFont = GDI.Font(Font);
                using var valueTextBrush = GDI.Brush(Font.Color);
                using var sf = new StringFormat()
                {
                    LineAlignment = StringAlignment.Center,
                    Alignment = belowBar ? StringAlignment.Far : StringAlignment.Near
                };

                var x = belowBar ? rect.Left : rect.Right;
                gfx.DrawString(ValueFormatter(value), valueTextFont, valueTextBrush, x, centerPx, sf);
            }
        }

        protected void RenderBarFromRect(RectangleF rect, bool negative, Graphics gfx)
        {
            using (var outlinePen = new Pen(BorderColor, BorderLineWidth))
            using (var fillBrush = GDI.Brush(negative ? FillColorNegative : FillColor, FillColorHatch, HatchStyle))
            {
                gfx.FillRectangle(fillBrush, rect.X, rect.Y, rect.Width, rect.Height);
                if (BorderLineWidth > 0)
                {
                    gfx.DrawRectangle(outlinePen, rect.X, rect.Y, rect.Width, rect.Height);
                }
            }
        }

        public override string ToString()
        {
            string label = string.IsNullOrWhiteSpace(Label) ? "" : $" ({Label})";
            return $"PlottableBar{label} with {Values.Length} bars";
        }

        public LegendItem[] GetLegendItems()
        {
            var singleItem = new LegendItem(this)
            {
                label = Label,
                color = FillColor,
                lineWidth = 10,
                markerShape = MarkerShape.none,
                hatchColor = FillColorHatch,
                hatchStyle = HatchStyle,
                borderColor = BorderColor,
                borderWith = BorderLineWidth
            };
            return LegendItem.Single(singleItem);
        }

        private static RectangleF ClipRectToDataArea(RectangleF rect, PlotDimensions dims)
        {
            float left = Math.Max(rect.Left, dims.DataOffsetX - 1);
            float right = Math.Min(rect.Right, dims.DataOffsetX + dims.DataWidth + 1);
            float top = Math.Max(rect.Top, dims.DataOffsetY - 1);
            float bottom = Math.Min(rect.Bottom, dims.DataOffsetY + dims.DataHeight + 1);

            return new RectangleF(left, top, right - left, bottom - top);
        }

        public void ValidateData(bool deep = false) { }
    }
}
