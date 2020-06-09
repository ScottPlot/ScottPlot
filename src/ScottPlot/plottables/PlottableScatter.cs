using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScottPlot.Config;
using ScottPlot.Drawing;

namespace ScottPlot
{
    public class PlottableScatter : Plottable, IExportable
    {
        public double[] xs;
        public double[] ys;
        public double[] errorX;
        public double[] errorY;
        public double lineWidth;
        public float errorLineWidth;
        public float errorCapSize;
        public float markerSize;
        public bool stepDisplay;

        public MarkerShape markerShape;
        public Color color;
        public LineStyle lineStyle;
        public string label;

        public Pen penLine;
        private Pen penLineError;

        public PlottableScatter(double[] xs, double[] ys, Color color, double lineWidth, double markerSize, string label,
            double[] errorX, double[] errorY, double errorLineWidth, double errorCapSize, bool stepDisplay, MarkerShape markerShape, LineStyle lineStyle)
        {

            if ((xs == null) || (ys == null))
                throw new ArgumentException("X and Y data cannot be null");

            if ((xs.Length == 0) || (ys.Length == 0))
                throw new ArgumentException("xs and ys must have at least one element");

            if (xs.Length != ys.Length)
                throw new ArgumentException("Xs and Ys must have same length");

            if (errorY != null)
                for (int i = 0; i < errorY.Length; i++)
                    if (errorY[i] < 0)
                        errorY[i] = -errorY[i];

            if (errorX != null)
                for (int i = 0; i < errorX.Length; i++)
                    if (errorX[i] < 0)
                        errorX[i] = -errorX[i];

            this.xs = xs;
            this.ys = ys;
            this.color = color;
            this.lineWidth = lineWidth;
            this.markerSize = (float)markerSize;
            this.label = label;
            this.errorX = errorX;
            this.errorY = errorY;
            this.errorLineWidth = (float)errorLineWidth;
            this.errorCapSize = (float)errorCapSize;
            this.stepDisplay = stepDisplay;
            this.markerShape = markerShape;
            this.lineStyle = lineStyle;

            if (xs.Length != ys.Length)
                throw new ArgumentException("X and Y arrays must have the same length");

            if ((errorX != null) && (xs.Length != errorX.Length))
                throw new ArgumentException("errorX must be the same length as the original data");

            if ((errorY != null) && (xs.Length != errorY.Length))
                throw new ArgumentException("errorY must be the same length as the original data");

            penLine = GDI.Pen(color, lineWidth, lineStyle, true);
            penLineError = GDI.Pen(color, errorLineWidth, LineStyle.Solid, true);
        }

        public override string ToString()
        {
            string label = string.IsNullOrWhiteSpace(this.label) ? "" : $" ({this.label})";
            return $"PlottableScatter{label} with {GetPointCount()} points";
        }

        public override Config.AxisLimits2D GetLimits()
        {
            double[] limits = new double[4];

            if (errorX == null)
            {
                limits[0] = xs.Min();
                limits[1] = xs.Max();
            }
            else
            {
                limits[0] = xs[0] - errorX[0];
                limits[1] = xs[0] + errorX[0];
                for (int i = 1; i < xs.Length; i++)
                {
                    if (xs[i] - errorX[i] < limits[0])
                        limits[0] = xs[i] - errorX[i];
                    if (xs[i] + errorX[i] > limits[0])
                        limits[1] = xs[i] + errorX[i];
                }
            }

            if (errorY == null)
            {
                limits[2] = ys.Min();
                limits[3] = ys.Max();
            }
            else
            {
                limits[2] = ys[0] - errorY[0];
                limits[3] = ys[0] + errorY[0];
                for (int i = 1; i < ys.Length; i++)
                {
                    if (ys[i] - errorY[i] < limits[2])
                        limits[2] = ys[i] - errorY[i];
                    if (ys[i] + errorY[i] > limits[3])
                        limits[3] = ys[i] + errorY[i];
                }
            }

            // TODO: use features of 2d axis
            return new Config.AxisLimits2D(limits);
        }

        protected virtual void DrawPoint(Settings settings, List<PointF> points, int i)
        {
            MarkerTools.DrawMarker(settings.gfxData, points[i], markerShape, markerSize, color);
        }

        public override void Render(Settings settings)
        {
            penLine.Color = color;
            penLine.Width = (float)lineWidth;

            // create a List of only valid points
            List<PointF> points = new List<PointF>(xs.Length);
            for (int i = 0; i < xs.Length; i++)
                if (!double.IsNaN(xs[i]) && !double.IsNaN(ys[i]))
                    points.Add(settings.GetPixel(xs[i], ys[i]));

            // draw Y errorbars
            if (errorY != null)
            {
                for (int i = 0; i < points.Count; i++)
                {
                    PointF errorBelow = settings.GetPixel(xs[i], ys[i] - errorY[i]);
                    PointF errorAbove = settings.GetPixel(xs[i], ys[i] + errorY[i]);
                    float xCenter = errorBelow.X;
                    float yTop = errorAbove.Y;
                    float yBot = errorBelow.Y;
                    settings.gfxData.DrawLine(penLineError, xCenter, yBot, xCenter, yTop);
                    settings.gfxData.DrawLine(penLineError, xCenter - errorCapSize, yBot, xCenter + errorCapSize, yBot);
                    settings.gfxData.DrawLine(penLineError, xCenter - errorCapSize, yTop, xCenter + errorCapSize, yTop);
                }
            }

            // draw X errorbars
            if (errorX != null)
            {
                for (int i = 0; i < points.Count; i++)
                {
                    PointF errorLeft = settings.GetPixel(xs[i] - errorX[i], ys[i]);
                    PointF errorRight = settings.GetPixel(xs[i] + errorX[i], ys[i]);
                    float yCenter = errorLeft.Y;
                    float xLeft = errorLeft.X;
                    float xRight = errorRight.X;
                    settings.gfxData.DrawLine(penLineError, xLeft, yCenter, xRight, yCenter);
                    settings.gfxData.DrawLine(penLineError, xLeft, yCenter - errorCapSize, xLeft, yCenter + errorCapSize);
                    settings.gfxData.DrawLine(penLineError, xRight, yCenter - errorCapSize, xRight, yCenter + errorCapSize);
                }
            }

            // draw the lines connecting points
            if (penLine.Width > 0 && points.Count > 1)
            {
                if (stepDisplay)
                {
                    List<PointF> pointsStep = new List<PointF>(points.Count * 2);
                    for (int i = 0; i < points.Count - 1; i++)
                    {
                        pointsStep.Add(points[i]);
                        pointsStep.Add(new PointF(points[i + 1].X, points[i].Y));
                    }
                    pointsStep.Add(points[points.Count - 1]);
                    settings.gfxData.DrawLines(penLine, pointsStep.ToArray());
                }
                else
                {
                    settings.gfxData.DrawLines(penLine, points.ToArray());
                }
            }

            // draw a marker at each point
            if ((markerSize > 0) && (markerShape != MarkerShape.none))
                for (int i = 0; i < points.Count; i++)
                    DrawPoint(settings, points, i);

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

        public override int GetPointCount()
        {
            return ys.Length;
        }

        public override LegendItem[] GetLegendItems()
        {
            // TODO: determine how to respect line width in legend
            var singleLegendItem = new Config.LegendItem(label, color, lineStyle, lineWidth, markerShape, markerSize);
            return new LegendItem[] { singleLegendItem };
        }
    }
}
