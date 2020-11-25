using ScottPlot.Config;
using ScottPlot.Diagnostic.Attributes;
using ScottPlot.Drawing;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Reflection;
using System.Text;

namespace ScottPlot
{
    public class PlottableBar : Plottable, IPlottable
    {
        public double[] xs;
        public double xOffset;
        public double[] ys;
        public double[] yErr;
        public double[] yOffsets;

        public bool fill = true;
        public Color fillColor = Color.Green;
        public Color hatchColor = Color.Blue;
        public Color negativeColor = Color.Red;
        public Color borderColor = Color.Black;
        public Color errorColor = Color.Black;
        public float borderLineWidth = 1;
        public float errorLineWidth = 1;
        public string label;

        public string FontName;
        public float FontSize = 12;
        public bool FontBold;
        public Color FontColor = Color.Black;
        public double errorCapSize = .4;
        public double barWidth = .8;
        public double valueBase = 0;
        public bool verticalBars = true;
        public bool showValues;

        public Drawing.HatchStyle hatchStyle = Drawing.HatchStyle.None;

        public PlottableBar(double[] xs, double[] ys, double[] yErr, double[] yOffsets)
        {
            if (ys is null || ys.Length == 0)
                throw new ArgumentException("ys must be an array that contains elements");

            this.ys = ys;
            this.xs = xs ?? DataGen.Consecutive(ys.Length);
            this.yErr = yErr ?? DataGen.Zeros(ys.Length);
            this.yOffsets = yOffsets ?? DataGen.Zeros(ys.Length);
        }

        public override AxisLimits2D GetLimits()
        {
            double valueMin = double.PositiveInfinity;
            double valueMax = double.NegativeInfinity;
            double positionMin = double.PositiveInfinity;
            double positionMax = double.NegativeInfinity;

            for (int i = 0; i < xs.Length; i++)
            {
                valueMin = Math.Min(valueMin, ys[i] - yErr[i] + yOffsets[i]);
                valueMax = Math.Max(valueMax, ys[i] + yErr[i] + yOffsets[i]);
                positionMin = Math.Min(positionMin, xs[i]);
                positionMax = Math.Max(positionMax, xs[i]);
            }

            valueMin = Math.Min(valueMin, valueBase);
            valueMax = Math.Max(valueMax, valueBase);

            if (showValues)
                valueMax += (valueMax - valueMin) * .1; // increase by 10% to accomodate label

            positionMin -= barWidth / 2;
            positionMax += barWidth / 2;

            positionMin += xOffset;
            positionMax += xOffset;

            if (verticalBars)
                return new AxisLimits2D(positionMin, positionMax, valueMin, valueMax);
            else
                return new AxisLimits2D(valueMin, valueMax, positionMin, positionMax);
        }

        public string ValidationErrorMessage { get; private set; }
        public bool IsValidData(bool deepValidation = false)
        {
            try
            {
                Validate.AssertHasElements("xs", xs);
                Validate.AssertHasElements("ys", ys);
                Validate.AssertHasElements("yErr", yErr);
                Validate.AssertHasElements("yOffsets", yOffsets);
                Validate.AssertEqualLength("xs, ys, yErr, and yOffsets", xs, ys, yErr, yOffsets);

                if (deepValidation)
                {
                    Validate.AssertAllReal("xs", xs);
                    Validate.AssertAllReal("ys", ys);
                    Validate.AssertAllReal("yErr", yErr);
                    Validate.AssertAllReal("yOffsets", yOffsets);
                }
            }
            catch (ArgumentException e)
            {
                ValidationErrorMessage = e.Message;
                return false;
            }

            ValidationErrorMessage = null;
            return true;
        }

        public override void Render(Settings settings) => throw new InvalidOperationException("Use new Render() method");

        public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            using (Graphics gfx = Graphics.FromImage(bmp))
            {
                gfx.SmoothingMode = lowQuality ? SmoothingMode.HighSpeed : SmoothingMode.AntiAlias;
                gfx.TextRenderingHint = lowQuality ? TextRenderingHint.SingleBitPerPixelGridFit : TextRenderingHint.AntiAliasGridFit;

                for (int barIndex = 0; barIndex < ys.Length; barIndex++)
                {
                    if (verticalBars)
                        RenderBarVertical(dims, gfx, xs[barIndex] + xOffset, ys[barIndex], yErr[barIndex], yOffsets[barIndex]);
                    else
                        RenderBarHorizontal(dims, gfx, xs[barIndex] + xOffset, ys[barIndex], yErr[barIndex], yOffsets[barIndex]);
                }
            }
        }

        private void RenderBarVertical(PlotDimensions dims, Graphics gfx, double position, double value, double valueError, double yOffset)
        {
            // bar body
            float centerPx = dims.GetPixelX(position);
            double edge1 = position - barWidth / 2;
            double value1 = Math.Min(valueBase, value) + yOffset;
            double value2 = Math.Max(valueBase, value) + yOffset;
            double valueSpan = value2 - value1;
            var rect = new RectangleF(
                x: dims.GetPixelX(edge1),
                y: dims.GetPixelY(value2),
                width: (float)(barWidth * dims.PxPerUnitX),
                height: (float)(valueSpan * dims.PxPerUnitY));

            // errorbar
            double error1 = value > 0 ? value2 - Math.Abs(valueError) : value1 - Math.Abs(valueError);
            double error2 = value > 0 ? value2 + Math.Abs(valueError) : value1 + Math.Abs(valueError);
            float capPx1 = dims.GetPixelX(position - errorCapSize * barWidth / 2);
            float capPx2 = dims.GetPixelX(position + errorCapSize * barWidth / 2);
            float errorPx2 = dims.GetPixelY(error2);
            float errorPx1 = dims.GetPixelY(error1);

            if (fill)
                using (var fillBrush = GDI.Brush((value < 0) ? negativeColor : fillColor, hatchColor, hatchStyle))
                    gfx.FillRectangle(fillBrush, rect.X, rect.Y, rect.Width, rect.Height);

            if (borderLineWidth > 0)
                using (var outlinePen = new Pen(borderColor, borderLineWidth))
                    gfx.DrawRectangle(outlinePen, rect.X, rect.Y, rect.Width, rect.Height);

            if (errorLineWidth > 0 && valueError > 0)
            {
                using (var errorPen = new Pen(errorColor, errorLineWidth))
                {
                    gfx.DrawLine(errorPen, centerPx, errorPx1, centerPx, errorPx2);
                    gfx.DrawLine(errorPen, capPx1, errorPx1, capPx2, errorPx1);
                    gfx.DrawLine(errorPen, capPx1, errorPx2, capPx2, errorPx2);
                }
            }

            if (showValues)
                using (var valueTextFont = GDI.Font(FontName, FontSize, FontBold))
                using (var valueTextBrush = GDI.Brush(FontColor))
                using (var sf = new StringFormat() { LineAlignment = StringAlignment.Far, Alignment = StringAlignment.Center })
                    gfx.DrawString((value + yOffset).ToString(), valueTextFont, valueTextBrush, centerPx, rect.Y, sf);
        }

        private void RenderBarHorizontal(PlotDimensions dims, Graphics gfx, double position, double value, double valueError, double yOffset)
        {
            // bar body
            float centerPx = dims.GetPixelY(position);
            double edge2 = position + barWidth / 2;
            double value1 = Math.Min(valueBase, value) + yOffset;
            double value2 = Math.Max(valueBase, value) + yOffset;
            double valueSpan = value2 - value1;
            var rect = new RectangleF(
                x: dims.GetPixelX(value1),
                y: dims.GetPixelY(edge2),
                height: (float)(barWidth * dims.PxPerUnitY),
                width: (float)(valueSpan * dims.PxPerUnitX));

            // errorbar
            double error1 = value > 0 ? value2 - Math.Abs(valueError) : value1 - Math.Abs(valueError);
            double error2 = value > 0 ? value2 + Math.Abs(valueError) : value1 + Math.Abs(valueError);
            float capPx1 = dims.GetPixelY(position - errorCapSize * barWidth / 2);
            float capPx2 = dims.GetPixelY(position + errorCapSize * barWidth / 2);
            float errorPx2 = dims.GetPixelX(error2);
            float errorPx1 = dims.GetPixelX(error1);

            if (fill)
                using (var fillBrush = GDI.Brush((value < 0) ? negativeColor : fillColor, hatchColor, hatchStyle))
                    gfx.FillRectangle(fillBrush, rect.X, rect.Y, rect.Width, rect.Height);

            if (borderLineWidth > 0)
                using (var outlinePen = new Pen(borderColor, borderLineWidth))
                    gfx.DrawRectangle(outlinePen, rect.X, rect.Y, rect.Width, rect.Height);

            if (errorLineWidth > 0 && valueError > 0)
            {
                using (var errorPen = new Pen(errorColor, errorLineWidth))
                {
                    gfx.DrawLine(errorPen, errorPx1, centerPx, errorPx2, centerPx);
                    gfx.DrawLine(errorPen, errorPx1, capPx2, errorPx1, capPx1);
                    gfx.DrawLine(errorPen, errorPx2, capPx2, errorPx2, capPx1);
                }
            }

            if (showValues)
                using (var valueTextFont = GDI.Font(FontName, FontSize, FontBold))
                using (var valueTextBrush = GDI.Brush(FontColor))
                using (var sf = new StringFormat() { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Near })
                    gfx.DrawString((value + yOffset).ToString(), valueTextFont, valueTextBrush, rect.X + rect.Width, centerPx, sf);
        }

        public override string ToString()
        {
            string label = string.IsNullOrWhiteSpace(this.label) ? "" : $" ({this.label})";
            return $"PlottableBar{label} with {GetPointCount()} points";
        }

        public override int GetPointCount() => ys is null ? 0 : ys.Length;

        public override LegendItem[] GetLegendItems()
        {
            LegendItem singleLegendItem = new LegendItem()
            {
                label = label,
                color = fillColor,
                lineWidth = 10,
                markerShape = MarkerShape.none,
                hatchColor = hatchColor,
                hatchStyle = hatchStyle,
                borderColor = borderColor,
                borderWith = borderLineWidth
            };
            return new LegendItem[] { singleLegendItem };
        }
    }
}
