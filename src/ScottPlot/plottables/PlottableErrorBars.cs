using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace ScottPlot
{
    public class PlottableErrorBars : Plottable
    {
        private double[] xs;
        private double[] ys;
        private double[] xPositiveError;
        private double[] xNegativeError;
        private double[] yPositiveError;
        private double[] yNegativeError;
        private float capLength;
        private Pen penLine;

        public PlottableErrorBars(double[] xs, double[] ys, double[] xPositiveError = null, double[] xNegativeError = null,
            double[] yPositiveError = null, double[] yNegativeError = null, Color? color = null, double lineWidth = 1, double capLength = 3)
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
            this.color = color ?? Color.Black; //default is black, no null allowed
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

        public void DrawErrorBar(Settings settings, double[] errorArray, bool xError, bool positiveError)
        {
            if (errorArray != null)
            {
                for (int i = 0; i < xs.Length; i++)
                {
                    float x = (float)xs[i];
                    float y = (float)xs[i];
                    float error = positiveError ? (float)errorArray[i] : (float)errorArray[i] * -1;
                    if (xError)
                    {
                        float xWithError = x + error;
                        settings.gfxData.DrawLine(penLine, x, y, xWithError, y); //draw the error
                        settings.gfxData.DrawLine(penLine, xWithError, y - capLength, xWithError, y + capLength); //draw the cap
                    }
                    else
                    {
                        float yWithError = y + error;
                        settings.gfxData.DrawLine(penLine, x, y, x, yWithError); //draw the error
                        settings.gfxData.DrawLine(penLine, x - capLength, yWithError, x + capLength, yWithError); //draw the cap
                    }
                }
            }
        }
    }
}
