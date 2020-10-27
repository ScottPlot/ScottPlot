using ScottPlot.Config;
using ScottPlot.Drawing;
using ScottPlot.Renderable;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Text;

namespace ScottPlot.Plottable
{
    public class ScatterPlot : IRenderable, IExportable, IHasLegendItems, IHasAxisLimits, IValidatable
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
        public bool IsVisible { get; set; } = true;

        public ScatterPlot(double[] xs, double[] ys, double[] errorX = null, double[] errorY = null)
        {
            this.xs = xs;
            this.ys = ys;
            this.errorX = errorX;
            this.errorY = errorY;
        }

        public string ErrorMessage(bool deepValidation = false)
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

                return null;
            }
            catch (ArgumentException e)
            {
                return e.Message;
            }
        }

        public override string ToString()
        {
            string label = string.IsNullOrWhiteSpace(this.label) ? "" : $" ({this.label})";
            return $"PlottableScatter{label} with {PointCount} points";
        }

        public AxisLimits2D GetLimits()
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
            if (IsVisible == false)
                return;

            using (var gfx = GDI.Graphics(bmp, dims, lowQuality))
            using (var penLine = GDI.Pen(color, lineWidth, lineStyle, true))
            using (var penLineError = GDI.Pen(color, errorLineWidth, LineStyle.Solid, true))
            {
                PointF[] points = new PointF[xs.Length];
                for (int i = 0; i < xs.Length; i++)
                    points[i] = new PointF(dims.GetPixelX(xs[i]), dims.GetPixelY(ys[i]));

                if (errorY != null)
                {
                    for (int i = 0; i < points.Length; i++)
                    {
                        float yBot = dims.GetPixelY(ys[i] - errorY[i]);
                        float yTop = dims.GetPixelY(ys[i] + errorY[i]);
                        gfx.DrawLine(penLineError, points[i].X, yBot, points[i].X, yTop);
                        gfx.DrawLine(penLineError, points[i].X - errorCapSize, yBot, points[i].X + errorCapSize, yBot);
                        gfx.DrawLine(penLineError, points[i].X - errorCapSize, yTop, points[i].X + errorCapSize, yTop);
                    }
                }

                if (errorX != null)
                {
                    for (int i = 0; i < points.Length; i++)
                    {
                        float xLeft = dims.GetPixelX(xs[i] - errorX[i]);
                        float xRight = dims.GetPixelX(xs[i] + errorX[i]);
                        gfx.DrawLine(penLineError, xLeft, points[i].Y, xRight, points[i].Y);
                        gfx.DrawLine(penLineError, xLeft, points[i].Y - errorCapSize, xLeft, points[i].Y + errorCapSize);
                        gfx.DrawLine(penLineError, xRight, points[i].Y - errorCapSize, xRight, points[i].Y + errorCapSize);
                    }
                }

                // draw the lines connecting points
                if (lineWidth > 0 && lineStyle != LineStyle.None)
                {
                    if (stepDisplay)
                    {
                        PointF[] pointsStep = new PointF[points.Length * 2 - 1];
                        for (int i = 0; i < points.Length - 1; i++)
                        {
                            pointsStep[i * 2] = points[i];
                            pointsStep[i * 2 + 1] = new PointF(points[i + 1].X, points[i].Y);
                        }
                        pointsStep[pointsStep.Length - 1] = points[points.Length - 1];
                        gfx.DrawLines(penLine, pointsStep);
                    }
                    else
                    {
                        if (IsArrow)
                        {
                            penLine.CustomEndCap = new AdjustableArrowCap(ArrowheadWidth, ArrowheadLength, true);
                            penLine.StartCap = LineCap.Flat;
                        }

                        gfx.DrawLines(penLine, points);
                    }
                }

                // draw a marker at each point
                if ((markerSize > 0) && (markerShape != MarkerShape.none))
                    for (int i = 0; i < points.Length; i++)
                        MarkerTools.DrawMarker(gfx, points[i], markerShape, markerSize, color);
            }
        }

        public void Render(Settings settings) =>
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

        public int PointCount { get => ys.Length; }

        public LegendItem[] LegendItems
        {
            get
            {
                var legendItem = new LegendItem()
                {
                    label = label,
                    color = color,
                    lineStyle = lineStyle,
                    lineWidth = lineWidth,
                    markerShape = markerShape,
                    markerSize = markerSize
                };
                return new LegendItem[] { legendItem };
            }
        }
    }
}
