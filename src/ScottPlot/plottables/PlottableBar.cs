using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
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

        private bool verticalBars = true;

        public PlottableBar(double[] xs, double[] ys, string label, 
            double barWidth, double xOffset,
            bool fill, Color fillColor,
            double outlineWidth, Color outlineColor,
            double[] yErr, double errorLineWidth, double errorCapSize, Color errorColor
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

            this.barWidth = barWidth;
            this.errorCapSize = errorCapSize;

            this.fill = fill;
            this.fillColor = fillColor;
            this.label = label;

            fillBrush = new SolidBrush(fillColor);
            outlinePen = new Pen(outlineColor, (float)outlineWidth);
            errorPen = new Pen(errorColor, (float)errorLineWidth);
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
                double position = xs[i] + xOffset;
                double edge1 = position - barWidth / 2;
                double value1 = Math.Min(valueBase, ys[i]);
                double value2 = Math.Max(valueBase, ys[i]);
                double valueSpan = value2 - value1;

                var rect = new RectangleF(
                    x: (float)settings.GetPixelX(edge1),
                    y: (float)settings.GetPixelY(value2),
                    width: (float)(barWidth * settings.xAxisScale),
                    height: (float)(valueSpan * settings.yAxisScale));

                settings.gfxData.FillRectangle(fillBrush, rect.X, rect.Y, rect.Width, rect.Height);

                if (outlinePen.Width > 0)
                    settings.gfxData.DrawRectangle(outlinePen, rect.X, rect.Y, rect.Width, rect.Height);

                if (errorPen.Width > 0 && yErr[i] > 0)
                {
                    double error1 = value2 - Math.Abs(yErr[i]);
                    double error2 = value2 + Math.Abs(yErr[i]);

                    float centerPx = (float)settings.GetPixelX(position);
                    float capPx1 = (float)settings.GetPixelX(position - errorCapSize * barWidth / 2);
                    float capPx2 = (float)settings.GetPixelX(position + errorCapSize * barWidth / 2);
                    float errorPx2 = (float)settings.GetPixelY(error2);
                    float errorPx1 = (float)settings.GetPixelY(error1);

                    settings.gfxData.DrawLine(errorPen, centerPx, errorPx1, centerPx, errorPx2);
                    settings.gfxData.DrawLine(errorPen, capPx1, errorPx1, capPx2, errorPx1);
                    settings.gfxData.DrawLine(errorPen, capPx1, errorPx2, capPx2, errorPx2);
                }
            }
        }

        public override string ToString()
        {
            return $"PlottableBar with {GetPointCount()} points";
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
