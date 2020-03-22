using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace ScottPlot
{
    public class PlottableErrorBars : Plottable
    {
        private readonly double[] xs;
        private readonly double[] ys;
        private readonly double[] xPositiveError;
        private readonly double[] xNegativeError;
        private readonly double[] yPositiveError;
        private readonly double[] yNegativeError;
        private readonly float capSize;
        private readonly Pen penLine;

        public PlottableErrorBars(double[] xs, double[] ys, double[] xPositiveError, double[] xNegativeError,
            double[] yPositiveError, double[] yNegativeError, Color color, double lineWidth, double capSize, string label)
        {
            //check input
            if (xs.Length != ys.Length)
                throw new ArgumentException("X and Y arrays must have the same length");

            //save properties
            this.xs = xs;
            this.ys = ys;
            this.xPositiveError = SanitizeErrors(xPositiveError, xs.Length);
            this.xNegativeError = SanitizeErrors(xNegativeError, xs.Length);
            this.yPositiveError = SanitizeErrors(yPositiveError, xs.Length);
            this.yNegativeError = SanitizeErrors(yNegativeError, xs.Length);
            this.capSize = (float)capSize;
            this.color = color;
            this.label = label;
            pointCount = xs.Length;

            penLine = new Pen(this.color, (float)lineWidth)
            {
                StartCap = System.Drawing.Drawing2D.LineCap.Round,
                EndCap = System.Drawing.Drawing2D.LineCap.Round,
                LineJoin = System.Drawing.Drawing2D.LineJoin.Round,
                DashStyle = StyleTools.DashStyle(lineStyle),
                DashPattern = StyleTools.DashPattern(lineStyle)
            };
        }

        private double[] SanitizeErrors(double[] errorArray, int expectedLength)
        {
            if (errorArray is null)
                return null;

            if (errorArray.Length != expectedLength)
                throw new ArgumentException("Point arrays and error arrays must have the same length");

            for (int i = 0; i < errorArray.Length; i++)
                if (errorArray[i] < 0)
                    errorArray[i] = -errorArray[i];

            return errorArray;
        }

        public override string ToString()
        {
            return $"PlottableErrorBars with {pointCount} points";
        }

        public override Config.AxisLimits2D GetLimits()
        {
            double xMin = double.PositiveInfinity;
            double yMin = double.PositiveInfinity;
            double xMax = double.NegativeInfinity;
            double yMax = double.NegativeInfinity;

            if (xNegativeError is null)
            {
                xMin = xs.Min();
            }
            else
            {
                for (int i = 0; i < xs.Length; i++)
                    xMin = Math.Min(xMin, xs[i] - xNegativeError[i]);
            }

            if (xPositiveError is null)
            {
                xMax = xs.Max();
            }
            else
            {
                for (int i = 0; i < xs.Length; i++)
                    xMax = Math.Max(xMax, xs[i] + xPositiveError[i]);
            }

            if (yNegativeError is null)
            {
                yMin = ys.Min();
            }
            else
            {
                for (int i = 0; i < xs.Length; i++)
                    yMin = Math.Min(yMin, ys[i] - yNegativeError[i]);
            }

            if (yPositiveError is null)
            {
                yMax = ys.Max();
            }
            else
            {
                for (int i = 0; i < xs.Length; i++)
                    yMax = Math.Max(yMax, ys[i] + yPositiveError[i]);
            }

            return new Config.AxisLimits2D(new double[] { xMin, xMax, yMin, yMax });
        }


        public override void Render(Settings settings)
        {
            DrawErrorBar(settings, xPositiveError, true, true);
            DrawErrorBar(settings, xNegativeError, true, false);
            DrawErrorBar(settings, yPositiveError, false, true);
            DrawErrorBar(settings, yNegativeError, false, false);
        }

        public void DrawErrorBar(Settings settings, double[] errorArray, bool xError, bool positiveError)
        {
            if (errorArray is null)
                return;

            float slightPixelOffset = 0.01f; // to fix GDI bug that happens when small straight lines are drawn with anti-aliasing on

            for (int i = 0; i < xs.Length; i++)
            {
                PointF centerPixel = settings.GetPixel(xs[i], ys[i]);
                float errorSize = positiveError ? (float)errorArray[i] : -(float)errorArray[i];
                if (xError)
                {
                    float xWithError = (float)settings.GetPixelX(xs[i] + errorSize);
                    settings.gfxData.DrawLine(penLine, centerPixel.X, centerPixel.Y, xWithError, centerPixel.Y);
                    settings.gfxData.DrawLine(penLine, xWithError, centerPixel.Y - capSize, xWithError + slightPixelOffset, centerPixel.Y + capSize);
                }
                else
                {
                    float yWithError = (float)settings.GetPixelY(ys[i] + errorSize);
                    settings.gfxData.DrawLine(penLine, centerPixel.X, centerPixel.Y, centerPixel.X, yWithError);
                    settings.gfxData.DrawLine(penLine, centerPixel.X - capSize, yWithError, centerPixel.X + capSize, yWithError + slightPixelOffset);
                }
            }
        }
    }
}
