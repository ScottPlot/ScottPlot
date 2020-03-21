using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ScottPlot
{
    public class PlottableScatter : Plottable, IExportable
    {
        public double[] xs;
        public double[] ys;
        public double[] errorXPositive;
        public double[] errorXNegative;
        public double[] errorYPositive;
        public double[] errorYNegative;
        public double lineWidth;
        public float errorLineWidth;
        public float errorCapSize;
        public float markerSize;
        public bool stepDisplay;
        public Pen penLine;
        public Pen penLineError;
        public Brush brush;

        public PlottableScatter(double[] xs, double[] ys, Color color, double lineWidth, double markerSize, string label,
            double[] errorXPositive, double[] errorXNegative, double[] errorYPositive, double[] errorYNegative, double errorLineWidth, double errorCapSize, bool stepDisplay, MarkerShape markerShape, LineStyle lineStyle)
        {

            if ((xs == null) || (ys == null))
                throw new Exception("X and Y data cannot be null");

            if (xs.Length != ys.Length)
                throw new Exception("Xs and Ys must have same length");

            if (errorYPositive != null)
                for (int i = 0; i < errorYPositive.Length; i++)
                    if (errorYPositive[i] < 0)
                        errorYPositive[i] = -errorYPositive[i];
            if (errorYNegative != null)
                for (int i = 0; i < errorYNegative.Length; i++)
                    if (errorYNegative[i] < 0)
                        errorYNegative[i] = -errorYNegative[i];

            if (errorXPositive != null)
                for (int i = 0; i < errorXPositive.Length; i++)
                    if (errorXPositive[i] < 0)
                        errorXPositive[i] = -errorXPositive[i];
            if (errorXNegative != null)
                for (int i = 0; i < errorXNegative.Length; i++)
                    if (errorXNegative[i] < 0)
                        errorXNegative[i] = -errorXNegative[i];

            this.xs = xs;
            this.ys = ys;
            this.color = color;
            this.lineWidth = lineWidth;
            this.markerSize = (float)markerSize;
            this.label = label;
            this.errorXPositive = errorXPositive;
            this.errorXNegative = errorXNegative;
            this.errorYPositive = errorYPositive;
            this.errorYNegative = errorYNegative;
            this.errorLineWidth = (float)errorLineWidth;
            this.errorCapSize = (float)errorCapSize;
            this.stepDisplay = stepDisplay;
            this.markerShape = markerShape;
            this.lineStyle = lineStyle;

            pointCount = xs.Length;

            if (xs.Length != ys.Length)
                throw new ArgumentException("X and Y arrays must have the same length");

            if ((errorXPositive != null) && (xs.Length != errorXPositive.Length) ||
                (errorXNegative != null) && (xs.Length != errorXNegative.Length))
                throw new ArgumentException("errorX must be the same length as the original data");

            if ((errorYPositive != null) && (xs.Length != errorYPositive.Length) ||
                (errorYNegative != null) && (xs.Length != errorYNegative.Length))
                throw new ArgumentException("errorY must be the same length as the original data");

            penLine = new Pen(color, (float)lineWidth)
            {
                StartCap = System.Drawing.Drawing2D.LineCap.Round,
                EndCap = System.Drawing.Drawing2D.LineCap.Round,
                LineJoin = System.Drawing.Drawing2D.LineJoin.Round,
                DashStyle = StyleTools.DashStyle(lineStyle),
                DashPattern = StyleTools.DashPattern(lineStyle)
            };

            penLineError = new Pen(color, (float)errorLineWidth)
            {
                StartCap = System.Drawing.Drawing2D.LineCap.Round,
                EndCap = System.Drawing.Drawing2D.LineCap.Round,
                LineJoin = System.Drawing.Drawing2D.LineJoin.Round
            };

            brush = new SolidBrush(color);
        }

        public override string ToString()
        {
            return $"PlottableScatter with {pointCount} points";
        }

        public override Config.AxisLimits2D GetLimits()
        {
            double[] limits = new double[4];

            if (errorXPositive == null)
            {
                limits[1] = xs.Max();
            }
            else
            {
                limits[1] = xs[0] + errorXPositive[0];
                for (int i = 1; i < xs.Length; i++)
                {
                    if (xs[i] + errorXPositive[i] > limits[0])
                        limits[1] = xs[i] + errorXPositive[i];
                }
            }
            if (errorXNegative == null)
            {
                limits[0] = xs.Min();
            }
            else
            {
                limits[0] = xs[0] - errorXNegative[0];
                for (int i = 1; i < xs.Length; i++)
                {
                    if (xs[i] - errorXNegative[i] < limits[0])
                        limits[0] = xs[i] - errorXNegative[i];
                }
            }

            if (errorYPositive == null)
            {
                limits[3] = ys.Max();
            }
            else
            {
                limits[3] = ys[0] + errorYPositive[0];
                for (int i = 1; i < ys.Length; i++)
                {
                    if (ys[i] + errorYPositive[i] > limits[3])
                        limits[3] = ys[i] + errorYPositive[i];
                }
            }
            if (errorYNegative == null)
            {
                limits[2] = ys.Min();
            }
            else
            {
                limits[2] = ys[0] - errorYNegative[0];
                for (int i = 1; i < ys.Length; i++)
                {
                    if (ys[i] - errorYNegative[i] < limits[2])
                        limits[2] = ys[i] - errorYNegative[i];
                }
            }

            // TODO: use features of 2d axis
            return new Config.AxisLimits2D(limits);
        }

        PointF[] points;
        PointF[] pointsStep;
        public override void Render(Settings settings)
        {
            penLine.Color = color;
            penLine.Width = (float)lineWidth;

            if (points is null)
                points = new PointF[xs.Length];

            for (int i = 0; i < xs.Length; i++)
                points[i] = settings.GetPixel(xs[i], ys[i]);

            if (stepDisplay)
            {
                if (pointsStep is null)
                    pointsStep = new PointF[xs.Length * 2 - 1];
                for (int i = 0; i < points.Length; i++)
                    pointsStep[i * 2] = points[i];
                for (int i = 0; i < points.Length - 1; i++)
                    pointsStep[i * 2 + 1] = new PointF(points[i + 1].X, points[i].Y);
            }

            if (errorYPositive != null)
            {
                for (int i = 0; i < points.Length; i++)
                {
                    PointF errorAbove = settings.GetPixel(xs[i], ys[i] + errorYPositive[i]);
                    float xCenter = errorAbove.X;
                    float yTop = errorAbove.Y;
                    settings.gfxData.DrawLine(penLineError, xCenter, (float)ys[i], xCenter, yTop);
                    settings.gfxData.DrawLine(penLineError, xCenter - errorCapSize, yTop, xCenter + errorCapSize, yTop);
                }
            }
            if (errorYNegative != null)
            {
                for (int i = 0; i < points.Length; i++)
                {
                    PointF errorBelow = settings.GetPixel(xs[i], ys[i] - errorYNegative[i]);
                    float xCenter = errorBelow.X;
                    float yBot = errorBelow.Y;
                    settings.gfxData.DrawLine(penLineError, xCenter, yBot, xCenter, (float)ys[i]);
                    settings.gfxData.DrawLine(penLineError, xCenter - errorCapSize, yBot, xCenter + errorCapSize, yBot);
                }
            }

            if (errorXPositive != null)
            {
                for (int i = 0; i < points.Length; i++)
                {
                    PointF errorRight = settings.GetPixel(xs[i] + errorXPositive[i], ys[i]);
                    float yCenter = errorRight.Y;
                    float xRight = errorRight.X;
                    settings.gfxData.DrawLine(penLineError, (float)xs[i], yCenter, xRight, yCenter);
                    settings.gfxData.DrawLine(penLineError, xRight, yCenter - errorCapSize, xRight, yCenter + errorCapSize);
                }
            }
            if (errorXNegative != null)
            {
                for (int i = 0; i < points.Length; i++)
                {
                    PointF errorLeft = settings.GetPixel(xs[i] - errorXNegative[i], ys[i]);
                    float yCenter = errorLeft.Y;
                    float xLeft = errorLeft.X;
                    settings.gfxData.DrawLine(penLineError, xLeft, yCenter, (float)xs[i], yCenter);
                    settings.gfxData.DrawLine(penLineError, xLeft, yCenter - errorCapSize, xLeft, yCenter + errorCapSize);
                }
            }

            if (penLine.Width > 0 && points.Length > 1)
            {
                if (stepDisplay)
                    settings.gfxData.DrawLines(penLine, pointsStep);
                else
                    settings.gfxData.DrawLines(penLine, points);
            }

            if ((markerSize > 0) && (markerShape != MarkerShape.none))
                for (int i = 0; i < points.Length; i++)
                    MarkerTools.DrawMarker(settings.gfxData, points[i], markerShape, markerSize, color);

        }

        public void SaveCSV(string filePath, string delimiter = ", ", string separator = "\n")
        {
            System.IO.File.WriteAllText(filePath, GetCSV(delimiter, separator));
        }

        public string GetCSV(string delimiter = ", ", string separator = "\n")
        {
            StringBuilder csv = new StringBuilder();
            for (int i = 0; i < ys.Length; i++)
                csv.AppendFormat("{0}{1}{2}{3}", xs[i], delimiter, ys[i], separator);
            return csv.ToString();
        }
    }
}