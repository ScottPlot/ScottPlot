using ScottPlot.Config;
using ScottPlot.Diagnostic.Attributes;
using ScottPlot.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Globalization;
using System.Linq;
using System.Text;

namespace ScottPlot
{
    public class PlottableScatter : Plottable, IExportable, IPlottable
    {
        public double[] xs;
        public double[] ys;
        public double[] errorX;
        public double[] errorY;

        public double lineWidth = 1;
        public float errorLineWidth = 1;
        public float errorCapSize = 3;
        public float markerSize = 5;
        public bool stepDisplay = false;

        public bool IsArrow { get => ArrowheadWidth > 0 && ArrowheadLength > 0; }
        public float ArrowheadWidth = 0;
        public float ArrowheadLength = 0;

        public int? minRenderIndex { set { throw new NotImplementedException(); } }
        public int? maxRenderIndex { set { throw new NotImplementedException(); } }

        public MarkerShape markerShape = MarkerShape.filledCircle;
        public Color color = Color.Black;
        public LineStyle lineStyle = LineStyle.Solid;
        public string label;

        public PlottableScatter(double[] xs, double[] ys, double[] errorX = null, double[] errorY = null)
        {
            this.xs = xs;
            this.ys = ys;
            this.errorX = errorX;
            this.errorY = errorY;
        }

        public string ValidationErrorMessage { get; private set; }
        public bool IsValidData(bool deepValidation = false)
        {
            try
            {
                Validate.AssertHasElements("xs", xs);
                if (deepValidation)
                    Validate.AssertAllReal("xs", xs);

                Validate.AssertHasElements("ys", ys);
                if (deepValidation)
                    Validate.AssertAllReal("ys", ys);

                Validate.AssertEqualLength("xs and ys", xs, ys);

                if (errorX != null)
                {
                    Validate.AssertHasElements("errorX", xs);
                    Validate.AssertEqualLength("xs and errorX", xs, errorX);
                    if (deepValidation)
                        Validate.AssertAllReal("errorX", errorX);
                }

                if (errorY != null)
                {
                    Validate.AssertHasElements("errorY", ys);
                    Validate.AssertEqualLength("ys and errorY", ys, errorY);
                    if (deepValidation)
                        Validate.AssertAllReal("errorY", errorY);
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

        public override string ToString()
        {
            string label = string.IsNullOrWhiteSpace(this.label) ? "" : $" ({this.label})";
            return $"PlottableScatter{label} with {GetPointCount()} points";
        }

        public override AxisLimits2D GetLimits()
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

            return new AxisLimits2D(limits);
        }

        public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            using (var gfx = Graphics.FromImage(bmp))
            using (var penLine = GDI.Pen(color, lineWidth, lineStyle, true))
            using (var penLineError = GDI.Pen(color, errorLineWidth, LineStyle.Solid, true))
            {
                gfx.SmoothingMode = lowQuality ? SmoothingMode.HighSpeed : SmoothingMode.AntiAlias;
                gfx.TextRenderingHint = lowQuality ? TextRenderingHint.SingleBitPerPixelGridFit : TextRenderingHint.AntiAliasGridFit;

                // TODO: use a single array
                List<PointF> points = new List<PointF>(xs.Length);
                for (int i = 0; i < xs.Length; i++)
                    points.Add(new PointF(dims.GetPixelX(xs[i]), dims.GetPixelY(ys[i])));

                // draw Y errorbars
                if (errorY != null)
                {
                    for (int i = 0; i < points.Count; i++)
                    {
                        float xCenter = dims.GetPixelX(xs[i]);
                        float yBot = dims.GetPixelY(ys[i] - errorY[i]);
                        float yTop = dims.GetPixelY(ys[i] + errorY[i]);
                        gfx.DrawLine(penLineError, xCenter, yBot, xCenter, yTop);
                        gfx.DrawLine(penLineError, xCenter - errorCapSize, yBot, xCenter + errorCapSize, yBot);
                        gfx.DrawLine(penLineError, xCenter - errorCapSize, yTop, xCenter + errorCapSize, yTop);
                    }
                }

                // draw X errorbars
                if (errorX != null)
                {
                    for (int i = 0; i < points.Count; i++)
                    {
                        float yCenter = dims.GetPixelY(ys[i]);
                        float xLeft = dims.GetPixelX(xs[i] - errorX[i]);
                        float xRight = dims.GetPixelX(xs[i] + errorX[i]);
                        gfx.DrawLine(penLineError, xLeft, yCenter, xRight, yCenter);
                        gfx.DrawLine(penLineError, xLeft, yCenter - errorCapSize, xLeft, yCenter + errorCapSize);
                        gfx.DrawLine(penLineError, xRight, yCenter - errorCapSize, xRight, yCenter + errorCapSize);
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
                        gfx.DrawLines(penLine, pointsStep.ToArray());
                    }
                    else
                    {
                        if (IsArrow)
                        {
                            penLine.CustomEndCap = new System.Drawing.Drawing2D.AdjustableArrowCap(ArrowheadWidth, ArrowheadLength, isFilled: true);
                            penLine.StartCap = System.Drawing.Drawing2D.LineCap.Flat;
                        }

                        gfx.DrawLines(penLine, points.ToArray());
                    }
                }

                // draw a marker at each point
                if ((markerSize > 0) && (markerShape != MarkerShape.none))
                    for (int i = 0; i < points.Count; i++)
                        MarkerTools.DrawMarker(gfx, points[i], markerShape, markerSize, color);
            }
        }

        public override void Render(Settings settings) =>
            throw new InvalidOperationException("use other Render method");

        public void SaveCSV(string filePath, string delimiter = ", ", string separator = "\n")
        {
            System.IO.File.WriteAllText(filePath, GetCSV(delimiter, separator));
        }

        public string GetCSV(string delimiter = ", ", string separator = "\n")
        {
            StringBuilder csv = new StringBuilder();
            for (int i = 0; i < ys.Length; i++)
                csv.AppendFormat(CultureInfo.InvariantCulture, "{0}{1}{2}{3}", xs[i], delimiter, ys[i], separator);
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
