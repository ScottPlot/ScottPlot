using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScottPlot.Config;

namespace ScottPlot
{
    public class PlottableBar : Plottable
    {
        public double[] xs;
        public double[] ys;
        public double[] yErr;
        public double xOffset;

        public LineStyle lineStyle;
        public Color fillColor;
        public string label;

        private double errorCapSize;

        private double barWidth;
        private double valueBase = 0;

        public bool fill;
        private Brush fillBrush;
        private Pen errorPen;
        private Pen outlinePen;

        public bool verticalBars;
        public bool showValues;

        Font valueTextFont;
        Brush valueTextBrush;

        public PlottableBar(double[] xs, double[] ys, string label,
            double barWidth, double xOffset,
            bool fill, Color fillColor,
            double outlineWidth, Color outlineColor,
            double[] yErr, double errorLineWidth, double errorCapSize, Color errorColor,
            bool horizontal, bool showValues
            )
        {
            if (ys is null || ys.Length == 0)
                throw new ArgumentException("ys must contain data values");

            if (xs is null)
                xs = DataGen.Consecutive(ys.Length);

            if (xs.Length != ys.Length)
                throw new ArgumentException("xs and ys must have same number of elements");

            if (yErr is null)
                yErr = DataGen.Zeros(ys.Length);

            if (yErr.Length != ys.Length)
                throw new ArgumentException("yErr and ys must have same number of elements");

            this.xs = xs;
            this.ys = ys;
            this.yErr = yErr;
            this.xOffset = xOffset;
            this.label = label;
            this.verticalBars = !horizontal;
            this.showValues = showValues;

            this.barWidth = barWidth;
            this.errorCapSize = errorCapSize;

            this.fill = fill;
            this.fillColor = fillColor;

            fillBrush = new SolidBrush(fillColor);
            outlinePen = new Pen(outlineColor, (float)outlineWidth);
            errorPen = new Pen(errorColor, (float)errorLineWidth);

            valueTextFont = new Font(Fonts.GetDefaultFontName(), 12);
            valueTextBrush = new SolidBrush(Color.Black);
        }

        public override AxisLimits2D GetLimits()
        {
            double valueMin = double.PositiveInfinity;
            double valueMax = double.NegativeInfinity;
            double positionMin = double.PositiveInfinity;
            double positionMax = double.NegativeInfinity;

            for (int i = 0; i < xs.Length; i++)
            {
                valueMin = Math.Min(valueMin, ys[i] - yErr[i]);
                valueMax = Math.Max(valueMax, ys[i] + yErr[i]);
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

        public override void Render(Settings settings)
        {
            for (int i = 0; i < ys.Length; i++)
            {
                if (verticalBars)
                    RenderBarVertical(settings, xs[i] + xOffset, ys[i], yErr[i]);
                else
                    RenderBarHorizontal(settings, xs[i] + xOffset, ys[i], yErr[i]);
            }
        }

        private void RenderBarVertical(Settings settings, double position, double value, double valueError)
        {
            float centerPx = (float)settings.GetPixelX(position);
            double edge1 = position - barWidth / 2;
            double value1 = Math.Min(valueBase, value);
            double value2 = Math.Max(valueBase, value);
            double valueSpan = value2 - value1;

            var rect = new RectangleF(
                x: (float)settings.GetPixelX(edge1),
                y: (float)settings.GetPixelY(value2),
                width: (float)(barWidth * settings.xAxisScale),
                height: (float)(valueSpan * settings.yAxisScale));

            settings.gfxData.FillRectangle(fillBrush, rect.X, rect.Y, rect.Width, rect.Height);

            if (outlinePen.Width > 0)
                settings.gfxData.DrawRectangle(outlinePen, rect.X, rect.Y, rect.Width, rect.Height);

            if (errorPen.Width > 0 && valueError > 0)
            {
                double error1 = value2 - Math.Abs(valueError);
                double error2 = value2 + Math.Abs(valueError);

                float capPx1 = (float)settings.GetPixelX(position - errorCapSize * barWidth / 2);
                float capPx2 = (float)settings.GetPixelX(position + errorCapSize * barWidth / 2);
                float errorPx2 = (float)settings.GetPixelY(error2);
                float errorPx1 = (float)settings.GetPixelY(error1);

                settings.gfxData.DrawLine(errorPen, centerPx, errorPx1, centerPx, errorPx2);
                settings.gfxData.DrawLine(errorPen, capPx1, errorPx1, capPx2, errorPx1);
                settings.gfxData.DrawLine(errorPen, capPx1, errorPx2, capPx2, errorPx2);
            }

            if (showValues)
                settings.gfxData.DrawString(value.ToString(), valueTextFont, valueTextBrush, centerPx, rect.Y, settings.misc.sfSouth);
        }

        private void RenderBarHorizontal(Settings settings, double position, double value, double valueError)
        {
            float centerPx = (float)settings.GetPixelY(position);
            double edge2 = position + barWidth / 2;
            double value1 = Math.Min(valueBase, value);
            double value2 = Math.Max(valueBase, value);
            double valueSpan = value2 - value1;

            var rect = new RectangleF(
                x: (float)settings.GetPixelX(valueBase),
                y: (float)settings.GetPixelY(edge2),
                height: (float)(barWidth * settings.yAxisScale),
                width: (float)(valueSpan * settings.xAxisScale));

            settings.gfxData.FillRectangle(fillBrush, rect.X, rect.Y, rect.Width, rect.Height);

            if (outlinePen.Width > 0)
                settings.gfxData.DrawRectangle(outlinePen, rect.X, rect.Y, rect.Width, rect.Height);

            if (errorPen.Width > 0 && valueError > 0)
            {
                double error1 = value2 - Math.Abs(valueError);
                double error2 = value2 + Math.Abs(valueError);

                float capPx1 = (float)settings.GetPixelY(position - errorCapSize * barWidth / 2);
                float capPx2 = (float)settings.GetPixelY(position + errorCapSize * barWidth / 2);
                float errorPx2 = (float)settings.GetPixelX(error2);
                float errorPx1 = (float)settings.GetPixelX(error1);

                settings.gfxData.DrawLine(errorPen, errorPx1, centerPx, errorPx2, centerPx);
                settings.gfxData.DrawLine(errorPen, errorPx1, capPx2, errorPx1, capPx1);
                settings.gfxData.DrawLine(errorPen, errorPx2, capPx2, errorPx2, capPx1);
            }

            if (showValues)
                settings.gfxData.DrawString(value.ToString(), valueTextFont, valueTextBrush, rect.X + rect.Width, centerPx, settings.misc.sfWest);
        }

        public override string ToString()
        {
            string label = string.IsNullOrWhiteSpace(this.label) ? "" : $" ({this.label})";
            return $"PlottableBar{label} with {GetPointCount()} points";
        }

        public override int GetPointCount()
        {
            return ys.Length;
        }

        public override LegendItem[] GetLegendItems()
        {
            var singleLegendItem = new LegendItem(label, fillColor, lineWidth: 10, markerShape: MarkerShape.none);
            return new LegendItem[] { singleLegendItem };
        }
    }
}
