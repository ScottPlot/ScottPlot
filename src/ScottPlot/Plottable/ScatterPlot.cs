using ScottPlot.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Text;

namespace ScottPlot.Plottable
{
    public class ScatterPlot : IPlottable, IHasPoints, IExportable
    {
        // data
        public double[] Xs { get; private set; }
        public double[] Ys { get; private set; }
        public double[] XError { get; set; }
        public double[] YError { get; set; }

        // customization
        public bool IsVisible { get; set; } = true;
        public int XAxisIndex { get; set; } = 0;
        public int YAxisIndex { get; set; } = 0;
        public string Label;
        public Color Color = Color.Black;
        public LineStyle LineStyle = LineStyle.Solid;
        public MarkerShape MarkerShape = MarkerShape.filledCircle;
        public double LineWidth = 1;
        public float ErrorLineWidth = 1;
        public float ErrorCapSize = 3;
        public float MarkerSize = 5;
        public bool StepDisplay = false;
        public bool IsArrow { get => ArrowheadWidth > 0 && ArrowheadLength > 0; }
        public float ArrowheadWidth = 0;
        public float ArrowheadLength = 0;

        // TODO: support limited render indexes
        public int? MinRenderIndex { set { throw new NotImplementedException(); } }
        public int? MaxRenderIndex { set { throw new NotImplementedException(); } }

        public ScatterPlot(double[] xs, double[] ys, double[] errorX = null, double[] errorY = null)
        {
            Xs = xs;
            Ys = ys;
            XError = errorX;
            YError = errorY;
        }

        /// <summary>
        /// Replace the Xs array with a new one
        /// </summary>
        public void UpdateX(double[] xs)
        {
            if (xs is null)
                throw new ArgumentException("xs must not be null");
            if (xs.Length != Ys.Length)
                throw new ArgumentException("xs and ys must have the same length");

            Xs = xs;
        }

        /// <summary>
        /// Replace the Ys array with a new one
        /// </summary>
        public void UpdateY(double[] ys)
        {
            if (ys is null)
                throw new ArgumentException("ys must not be null");
            if (Xs.Length != ys.Length)
                throw new ArgumentException("xs and ys must have the same length");

            Ys = ys;
        }

        /// <summary>
        /// Replace Xs and Ys arrays with new ones
        /// </summary>
        public void Update(double[] xs, double[] ys)
        {
            if (xs is null)
                throw new ArgumentException("xs must not be null");
            if (ys is null)
                throw new ArgumentException("ys must not be null");
            if (xs.Length != ys.Length)
                throw new ArgumentException("xs and ys must have the same length");

            Xs = xs;
            Ys = ys;
        }

        public void ValidateData(bool deep = false)
        {
            Validate.AssertHasElements("xs", Xs);
            Validate.AssertHasElements("ys", Ys);
            Validate.AssertEqualLength("xs and ys", Xs, Ys);

            if (XError != null)
            {
                Validate.AssertHasElements("errorX", Xs);
                Validate.AssertEqualLength("xs and errorX", Xs, XError);
            }

            if (YError != null)
            {
                Validate.AssertHasElements("errorY", Ys);
                Validate.AssertEqualLength("ys and errorY", Ys, YError);
            }

            if (deep)
            {
                Validate.AssertAllReal("xs", Xs);
                Validate.AssertAllReal("ys", Ys);

                if (XError != null)
                    Validate.AssertAllReal("errorX", XError);

                if (YError != null)
                    Validate.AssertAllReal("errorY", YError);
            }
        }

        public override string ToString()
        {
            string label = string.IsNullOrWhiteSpace(this.Label) ? "" : $" ({this.Label})";
            return $"PlottableScatter{label} with {PointCount} points";
        }

        public AxisLimits GetAxisLimits()
        {
            ValidateData(deep: false);

            // TODO: don't use an array for this
            double[] limits = new double[4];

            if (XError == null)
            {
                limits[0] = Xs.Min();
                limits[1] = Xs.Max();
            }
            else
            {
                limits[0] = Xs[0] - XError[0];
                limits[1] = Xs[0] + XError[0];
                for (int i = 1; i < Xs.Length; i++)
                {
                    if (Xs[i] - XError[i] < limits[0])
                        limits[0] = Xs[i] - XError[i];
                    if (Xs[i] + XError[i] > limits[0])
                        limits[1] = Xs[i] + XError[i];
                }
            }

            if (YError == null)
            {
                limits[2] = Ys.Min();
                limits[3] = Ys.Max();
            }
            else
            {
                limits[2] = Ys[0] - YError[0];
                limits[3] = Ys[0] + YError[0];
                for (int i = 1; i < Ys.Length; i++)
                {
                    if (Ys[i] - YError[i] < limits[2])
                        limits[2] = Ys[i] - YError[i];
                    if (Ys[i] + YError[i] > limits[3])
                        limits[3] = Ys[i] + YError[i];
                }
            }

            if (double.IsNaN(limits[0]) || double.IsNaN(limits[1]))
                throw new InvalidOperationException("X data must not contain NaN");
            if (double.IsNaN(limits[2]) || double.IsNaN(limits[3]))
                throw new InvalidOperationException("Y data must not contain NaN");

            if (double.IsInfinity(limits[0]) || double.IsInfinity(limits[1]))
                throw new InvalidOperationException("X data must not contain Infinity");
            if (double.IsInfinity(limits[2]) || double.IsInfinity(limits[3]))
                throw new InvalidOperationException("Y data must not contain Infinity");

            return new AxisLimits(limits[0], limits[1], limits[2], limits[3]);
        }

        public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            if (IsVisible == false)
                return;

            using (var gfx = GDI.Graphics(bmp, dims, lowQuality))
            using (var penLine = GDI.Pen(Color, LineWidth, LineStyle, true))
            using (var penLineError = GDI.Pen(Color, ErrorLineWidth, LineStyle.Solid, true))
            {
                PointF[] points = new PointF[Xs.Length];
                for (int i = 0; i < Xs.Length; i++)
                {
                    float x = dims.GetPixelX(Xs[i]);
                    float y = dims.GetPixelY(Ys[i]);
                    if (float.IsNaN(x) || float.IsNaN(y))
                        throw new NotImplementedException("Data must not contain NaN");
                    points[i] = new PointF(x, y);
                }

                if (YError != null)
                {
                    for (int i = 0; i < points.Count(); i++)
                    {
                        float yBot = dims.GetPixelY(Ys[i] - YError[i]);
                        float yTop = dims.GetPixelY(Ys[i] + YError[i]);
                        gfx.DrawLine(penLineError, points[i].X, yBot, points[i].X, yTop);
                        gfx.DrawLine(penLineError, points[i].X - ErrorCapSize, yBot, points[i].X + ErrorCapSize, yBot);
                        gfx.DrawLine(penLineError, points[i].X - ErrorCapSize, yTop, points[i].X + ErrorCapSize, yTop);
                    }
                }

                if (XError != null)
                {
                    for (int i = 0; i < points.Length; i++)
                    {
                        float xLeft = dims.GetPixelX(Xs[i] - XError[i]);
                        float xRight = dims.GetPixelX(Xs[i] + XError[i]);
                        gfx.DrawLine(penLineError, xLeft, points[i].Y, xRight, points[i].Y);
                        gfx.DrawLine(penLineError, xLeft, points[i].Y - ErrorCapSize, xLeft, points[i].Y + ErrorCapSize);
                        gfx.DrawLine(penLineError, xRight, points[i].Y - ErrorCapSize, xRight, points[i].Y + ErrorCapSize);
                    }
                }

                // draw the lines connecting points
                if (LineWidth > 0 && points.Length > 1 && LineStyle != LineStyle.None)
                {
                    if (StepDisplay)
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
                if ((MarkerSize > 0) && (MarkerShape != MarkerShape.none))
                    for (int i = 0; i < points.Length; i++)
                        MarkerTools.DrawMarker(gfx, points[i], MarkerShape, MarkerSize, Color);
            }
        }

        public void SaveCSV(string filePath, string delimiter = ", ", string separator = "\n")
        {
            System.IO.File.WriteAllText(filePath, GetCSV(delimiter, separator));
        }

        public string GetCSV(string delimiter = ", ", string separator = "\n")
        {
            StringBuilder csv = new StringBuilder();
            for (int i = 0; i < Ys.Length; i++)
                csv.AppendFormat(CultureInfo.InvariantCulture, "{0}{1}{2}{3}", Xs[i], delimiter, Ys[i], separator);
            return csv.ToString();
        }

        public int PointCount { get => Ys.Length; }

        public LegendItem[] GetLegendItems()
        {
            var singleLegendItem = new LegendItem()
            {
                label = Label,
                color = Color,
                lineStyle = LineStyle,
                lineWidth = LineWidth,
                markerShape = MarkerShape,
                markerSize = MarkerSize
            };
            return new LegendItem[] { singleLegendItem };
        }


        public (double x, double y, int index) GetPointNearestX(double x)
        {
            double minDistance = Math.Abs(Xs[0] - x);
            int minIndex = 0;
            for (int i = 1; i < Xs.Length; i++)
            {
                double currDistance = Math.Abs(Xs[i] - x);
                if (currDistance < minDistance)
                {
                    minIndex = i;
                    minDistance = currDistance;
                }
            }

            return (Xs[minIndex], Ys[minIndex], minIndex);
        }

        public (double x, double y, int index) GetPointNearestY(double y)
        {
            double minDistance = Math.Abs(Ys[0] - y);
            int minIndex = 0;
            for (int i = 1; i < Ys.Length; i++)
            {
                double currDistance = Math.Abs(Ys[i] - y);
                if (currDistance < minDistance)
                {
                    minIndex = i;
                    minDistance = currDistance;
                }
            }

            return (Xs[minIndex], Ys[minIndex], minIndex);
        }

        public (double x, double y, int index) GetPointNearest(double x, double y)
        {
            List<(double x, double y)> points = Xs.Zip(Ys, (first, second) => (first, second)).ToList();

            double pointDistanceSquared(double x1, double y1) => (x1 - x) * (x1 - x) + (y1 - y) * (y1 - y);

            double minDistance = pointDistanceSquared(points[0].x, points[0].y);
            int minIndex = 0;
            for (int i = 1; i < points.Count; i++)
            {
                double currDistance = pointDistanceSquared(points[i].x, points[i].y);
                if (currDistance < minDistance)
                {
                    minIndex = i;
                    minDistance = currDistance;
                }
            }

            return (Xs[minIndex], Ys[minIndex], minIndex);
        }
    }
}
