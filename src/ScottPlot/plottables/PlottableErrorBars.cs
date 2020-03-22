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
        private readonly float capLength;
        private readonly double xOffSet; //used for multi-bar plot error bars
        private readonly Pen penLine;

        public PlottableErrorBars(double[] xs, double[] ys, double[] xPositiveError, double[] xNegativeError,
            double[] yPositiveError, double[] yNegativeError, Color color, double lineWidth = 1, double capLength = 3, double xOffSet = 0)
        {
            //check input
            if (xs.Length != ys.Length)
                throw new ArgumentException("X and Y arrays must have the same length");
            SanitizeErrors(xPositiveError, xs.Length);
            SanitizeErrors(xNegativeError, xs.Length);
            SanitizeErrors(yPositiveError, xs.Length);
            SanitizeErrors(yNegativeError, xs.Length);

            //save properties
            this.xs = xs;
            this.ys = ys;
            this.xPositiveError = xPositiveError;
            this.xNegativeError = xNegativeError;
            this.yPositiveError = yPositiveError;
            this.yNegativeError = yNegativeError;
            this.capLength = (float)capLength;
            this.xOffSet = xOffSet;
            this.color = color;
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

        /// <summary>
        /// Make all error values positive and 
        /// check the number of errors is equal
        /// to the number of points
        /// </summary>
        /// <param name="errorArray"></param>
        private void SanitizeErrors(double[] errorArray, int expectedLength)
        {
            if (errorArray != null)
            {
                if (errorArray.Length != expectedLength)
                {
                    throw new ArgumentException("Point arrays and error arrays must have the same length");
                }
                else
                {
                    for (int i = 0; i < errorArray.Length; i++)
                        if (errorArray[i] < 0)
                            errorArray[i] = -errorArray[i];
                }
            }
        }

        public override string ToString()
        {
            return $"PlottableErrorBars with {pointCount} points";
        }

        public override Config.AxisLimits2D GetLimits()
        {
            // TODO: use features of 2d axis
            return new Config.AxisLimits2D(new double[] { xs.Min(), xs.Max(), ys.Min(), ys.Max() });
        }


        public override void Render(Settings settings)
        {
            DrawErrorBar(settings, xPositiveError, true, true);
            DrawErrorBar(settings, xNegativeError, true, false);
            DrawErrorBar(settings, yPositiveError, false, true);
            DrawErrorBar(settings, yNegativeError, false, false);
        }

        /// <summary>
        /// Draws the error bar for a given error type
        /// (e.g. x or y, positive or negative)
        /// Makes a happy little "T"
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="errorArray"></param>
        /// <param name="xError"></param>
        /// <param name="positiveError"></param>
        public void DrawErrorBar(Settings settings, double[] errorArray, bool xError, bool positiveError)
        {
            if (errorArray != null)
            {
                for (int i = 0; i < xs.Length; i++)
                {

                    PointF mainPoint = settings.GetPixel(xs[i], ys[i]);
                    float x = mainPoint.X + (float)(xOffSet * settings.xAxisScale);
                    float y = mainPoint.Y;
                    float error = positiveError ? (float)errorArray[i] : (float)errorArray[i] * -1;
                    if (xError)
                    {
                        PointF errorPoint = settings.GetPixel(xs[i] + error, ys[i]);
                        float xWithError = errorPoint.X;
                        settings.gfxData.DrawLine(penLine, x, y, xWithError, y); //draw the error
                        settings.gfxData.DrawLine(penLine, xWithError, y - capLength, xWithError, y + capLength); //draw the cap
                    }
                    else
                    {
                        PointF errorPoint = settings.GetPixel(xs[i], ys[i] + error);
                        float yWithError = errorPoint.Y;
                        settings.gfxData.DrawLine(penLine, x, y, x, yWithError); //draw the error
                        settings.gfxData.DrawLine(penLine, x - capLength, yWithError, x + capLength, yWithError); //draw the cap
                    }
                }
            }
        }
    }
}
