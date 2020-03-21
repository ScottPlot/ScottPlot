using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ScottPlot
{
    public class PlottableBar : Plottable
    {
        public double[] xs;
        public double[] ys;
        float barWidth;
        float baseline = 0;
        double xOffset;
        Pen pen;
        Brush brush;

        public PlottableBar(double[] xs, double[] ys, double barWidth, double xOffset, Color color, string label, double barBorderWeight = 1)
        {
            if ((xs == null) || (ys == null))
                throw new Exception("X and Y data cannot be null");
            if (xs.Length != ys.Length)
                throw new ArgumentException("X positions must be the same length as Y values");

            this.xs = xs;
            this.ys = ys;
            this.barWidth = (float)barWidth;
            this.color = color;
            this.label = label;
            this.xOffset = xOffset;
            pointCount = ys.Length;

            pen = new Pen(color, (float)barBorderWeight)
            {
                // this prevents sharp corners
                StartCap = System.Drawing.Drawing2D.LineCap.Round,
                EndCap = System.Drawing.Drawing2D.LineCap.Round,
                LineJoin = System.Drawing.Drawing2D.LineJoin.Round
            };

            brush = new SolidBrush(color);
        }

        public override Config.AxisLimits2D GetLimits()
        {
            double[] limits = new double[4];
            limits[0] = xs.Min() - barWidth / 2;
            limits[1] = xs.Max() + barWidth / 2;
            limits[2] = ys.Min();
            limits[3] = ys.Max();

            if (baseline < limits[2])
                limits[2] = baseline;

            // TODO: use features of 2d axis
            return new Config.AxisLimits2D(limits);
        }

        public override void Render(Settings settings)
        {
            for (int i = 0; i < pointCount; i++)
            {
                PointF barTop;
                PointF barBot;

                if (ys[i] > baseline)
                {
                    barTop = settings.GetPixel(xs[i], ys[i]);
                    barBot = settings.GetPixel(xs[i], baseline);
                }
                else
                {
                    barBot = settings.GetPixel(xs[i], ys[i]);
                    barTop = settings.GetPixel(xs[i], baseline);
                }

                float barTopPx = barTop.Y;
                float barHeightPx = barTop.Y - barBot.Y;
                float barWidthPx = (float)(barWidth * settings.xAxisScale);
                float barLeftPx = barTop.X - barWidthPx / 2;
                float xOffsetPx = (float)(xOffset * settings.xAxisScale);
                barLeftPx += xOffsetPx;

                settings.gfxData.FillRectangle(brush, barLeftPx - (float).5, barTopPx, barWidthPx + (float).5, -barHeightPx);
                settings.gfxData.DrawRectangle(pen, barLeftPx - (float).5, barTopPx, barWidthPx + (float).5, -barHeightPx);
            }
        }

        public override string ToString()
        {
            return $"PlottableBar with {pointCount} points";
        }
    }
}
