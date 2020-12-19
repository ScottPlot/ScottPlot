using ScottPlot.Drawing;
using System;
using System.Drawing;
using System.Data;

namespace ScottPlot.Plottable
{
    /// <summary>
    /// Bar plots display a series of bars. 
    /// Positions are defined by Xs.
    /// Heights are defined by Ys (relative to BaseValue and YOffsets).
    /// </summary>
    public class BarPlot : IPlottable
    {
        // data
        public double[] Xs;
        public double XOffset;
        public double[] Ys;
        public double[] YErrors;
        public double[] YOffsets;

        // customization
        public bool IsVisible { get; set; } = true;
        public int XAxisIndex { get; set; } = 0;
        public int YAxisIndex { get; set; } = 0;
        public string Label;
        public Color FillColor = Color.Green;
        public Color FillColorNegative = Color.Red;
        public Color FillColorHatch = Color.Blue;
        public HatchStyle HatchStyle = HatchStyle.None;
        public Color ErrorColor = Color.Black;
        public float ErrorLineWidth = 1;
        public double ErrorCapSize = .4;
        public Color BorderColor = Color.Black;
        public float BorderLineWidth = 1;

        public readonly Drawing.Font Font = new Drawing.Font();
        public string FontName { set => Font.Name = value; }
        public float FontSize { set => Font.Size = value; }
        public bool FontBold { set => Font.Bold = value; }
        public Color FontColor { set => Font.Color = value; }

        public double BarWidth = .8;
        public double BaseValue = 0;
        public bool VerticalOrientation = true;
        public bool HorizontalOrientation { get => !VerticalOrientation; set => VerticalOrientation = !value; }
        public bool ShowValuesAboveBars;

        public BarPlot(double[] xs, double[] ys, double[] yErr, double[] yOffsets)
        {
            if (ys is null || ys.Length == 0)
                throw new InvalidOperationException("ys must be an array that contains elements");

            Ys = ys;
            Xs = xs ?? DataGen.Consecutive(ys.Length);
            YErrors = yErr ?? DataGen.Zeros(ys.Length);
            YOffsets = yOffsets ?? DataGen.Zeros(ys.Length);
        }

        public AxisLimits GetAxisLimits()
        {
            double valueMin = double.PositiveInfinity;
            double valueMax = double.NegativeInfinity;
            double positionMin = double.PositiveInfinity;
            double positionMax = double.NegativeInfinity;

            for (int i = 0; i < Xs.Length; i++)
            {
                valueMin = Math.Min(valueMin, Ys[i] - YErrors[i] + YOffsets[i]);
                valueMax = Math.Max(valueMax, Ys[i] + YErrors[i] + YOffsets[i]);
                positionMin = Math.Min(positionMin, Xs[i]);
                positionMax = Math.Max(positionMax, Xs[i]);
            }

            valueMin = Math.Min(valueMin, BaseValue);
            valueMax = Math.Max(valueMax, BaseValue);

            if (ShowValuesAboveBars)
                valueMax += (valueMax - valueMin) * .1; // increase by 10% to accomodate label

            positionMin -= BarWidth / 2;
            positionMax += BarWidth / 2;

            positionMin += XOffset;
            positionMax += XOffset;

            return VerticalOrientation ?
                new AxisLimits(positionMin, positionMax, valueMin, valueMax) :
                new AxisLimits(valueMin, valueMax, positionMin, positionMax);
        }

        public void ValidateData(bool deep = false)
        {
            Validate.AssertHasElements("xs", Xs);
            Validate.AssertHasElements("ys", Ys);
            Validate.AssertHasElements("yErr", YErrors);
            Validate.AssertHasElements("yOffsets", YOffsets);
            Validate.AssertEqualLength("xs, ys, yErr, and yOffsets", Xs, Ys, YErrors, YOffsets);

            if (deep)
            {
                Validate.AssertAllReal("xs", Xs);
                Validate.AssertAllReal("ys", Ys);
                Validate.AssertAllReal("yErr", YErrors);
                Validate.AssertAllReal("yOffsets", YOffsets);
            }
        }

        public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            using (Graphics gfx = GDI.Graphics(bmp, dims, lowQuality))
            {
                for (int barIndex = 0; barIndex < Ys.Length; barIndex++)
                {
                    if (VerticalOrientation)
                        RenderBarVertical(dims, gfx, Xs[barIndex] + XOffset, Ys[barIndex], YErrors[barIndex], YOffsets[barIndex]);
                    else
                        RenderBarHorizontal(dims, gfx, Xs[barIndex] + XOffset, Ys[barIndex], YErrors[barIndex], YOffsets[barIndex]);
                }
            }
        }

        private void RenderBarVertical(PlotDimensions dims, Graphics gfx, double position, double value, double valueError, double yOffset)
        {
            // bar body
            float centerPx = dims.GetPixelX(position);
            double edge1 = position - BarWidth / 2;
            double value1 = Math.Min(BaseValue, value) + yOffset;
            double value2 = Math.Max(BaseValue, value) + yOffset;
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

            using (var fillBrush = GDI.Brush((value < 0) ? FillColorNegative : FillColor, FillColorHatch, HatchStyle))
                gfx.FillRectangle(fillBrush, rect.X, rect.Y, rect.Width, rect.Height);

            if (BorderLineWidth > 0)
                using (var outlinePen = new Pen(BorderColor, BorderLineWidth))
                    gfx.DrawRectangle(outlinePen, rect.X, rect.Y, rect.Width, rect.Height);

            if (ErrorLineWidth > 0 && valueError > 0)
            {
                using (var errorPen = new Pen(ErrorColor, ErrorLineWidth))
                {
                    gfx.DrawLine(errorPen, centerPx, errorPx1, centerPx, errorPx2);
                    gfx.DrawLine(errorPen, capPx1, errorPx1, capPx2, errorPx1);
                    gfx.DrawLine(errorPen, capPx1, errorPx2, capPx2, errorPx2);
                }
            }

            if (ShowValuesAboveBars)
                using (var valueTextFont = GDI.Font(Font))
                using (var valueTextBrush = GDI.Brush(Font.Color))
                using (var sf = new StringFormat() { LineAlignment = StringAlignment.Far, Alignment = StringAlignment.Center })
                    gfx.DrawString(value.ToString(), valueTextFont, valueTextBrush, centerPx, rect.Y, sf);
        }

        private void RenderBarHorizontal(PlotDimensions dims, Graphics gfx, double position, double value, double valueError, double yOffset)
        {
            // bar body
            float centerPx = dims.GetPixelY(position);
            double edge2 = position + BarWidth / 2;
            double value1 = Math.Min(BaseValue, value) + yOffset;
            double value2 = Math.Max(BaseValue, value) + yOffset;
            double valueSpan = value2 - value1;
            var rect = new RectangleF(
                x: dims.GetPixelX(value1),
                y: dims.GetPixelY(edge2),
                height: (float)(BarWidth * dims.PxPerUnitY),
                width: (float)(valueSpan * dims.PxPerUnitX));

            // errorbar
            double error1 = value > 0 ? value2 - Math.Abs(valueError) : value1 - Math.Abs(valueError);
            double error2 = value > 0 ? value2 + Math.Abs(valueError) : value1 + Math.Abs(valueError);
            float capPx1 = dims.GetPixelY(position - ErrorCapSize * BarWidth / 2);
            float capPx2 = dims.GetPixelY(position + ErrorCapSize * BarWidth / 2);
            float errorPx2 = dims.GetPixelX(error2);
            float errorPx1 = dims.GetPixelX(error1);

            using (var fillBrush = GDI.Brush((value < 0) ? FillColorNegative : FillColor, FillColorHatch, HatchStyle))
                gfx.FillRectangle(fillBrush, rect.X, rect.Y, rect.Width, rect.Height);

            if (BorderLineWidth > 0)
                using (var outlinePen = new Pen(BorderColor, BorderLineWidth))
                    gfx.DrawRectangle(outlinePen, rect.X, rect.Y, rect.Width, rect.Height);

            if (ErrorLineWidth > 0 && valueError > 0)
            {
                using (var errorPen = new Pen(ErrorColor, ErrorLineWidth))
                {
                    gfx.DrawLine(errorPen, errorPx1, centerPx, errorPx2, centerPx);
                    gfx.DrawLine(errorPen, errorPx1, capPx2, errorPx1, capPx1);
                    gfx.DrawLine(errorPen, errorPx2, capPx2, errorPx2, capPx1);
                }
            }

            if (ShowValuesAboveBars)
                using (var valueTextFont = GDI.Font(Font))
                using (var valueTextBrush = GDI.Brush(Font.Color))
                using (var sf = new StringFormat() { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Near })
                    gfx.DrawString(value.ToString(), valueTextFont, valueTextBrush, rect.X + rect.Width, centerPx, sf);
        }

        public override string ToString()
        {
            string label = string.IsNullOrWhiteSpace(this.Label) ? "" : $" ({this.Label})";
            return $"PlottableBar{label} with {PointCount} points";
        }

        public int PointCount { get => Ys is null ? 0 : Ys.Length; }

        public LegendItem[] GetLegendItems()
        {
            var singleItem = new LegendItem()
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
            return new LegendItem[] { singleItem };
        }
    }
}
