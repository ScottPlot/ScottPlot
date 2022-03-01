using ScottPlot.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

namespace ScottPlot.Plottable
{
    /// <summary>
    /// The scatter plot renders X/Y pairs as points and/or connected lines.
    /// Scatter plots can be extremely slow for large datasets, so use Signal plots in these situations.
    /// </summary>
    public class ScatterPlot : IPlottable, IHasPoints, IHasLine, IHasMarker, IHighlightable, IHasColor
    {
        // data
        public double[] Xs { get; private set; }
        public double[] Ys { get; private set; }
        public double[] XError { get; set; }
        public double[] YError { get; set; }

        /// <summary>
        /// Add this value to each X value before plotting (axis units)
        /// </summary>
        public double OffsetX { get; set; } = 0;

        /// <summary>
        /// Add this value to each Y value before plotting (axis units)
        /// </summary>
        public double OffsetY { get; set; } = 0;

        public int PointCount => Ys.Length;

        // customization
        public bool IsVisible { get; set; } = true;
        public int XAxisIndex { get; set; } = 0;
        public int YAxisIndex { get; set; } = 0;
        public string Label;
        public Color Color { get => LineColor; set { LineColor = value; MarkerColor = value; } }
        public Color LineColor { get; set; } = Color.Black;
        public Color MarkerColor { get; set; } = Color.Black;
        public LineStyle LineStyle { get; set; } = LineStyle.Solid;
        public MarkerShape MarkerShape { get; set; } = MarkerShape.filledCircle;

        private double _lineWidth = 1;
        public double LineWidth
        {
            get => IsHighlighted ? _lineWidth * HighlightCoefficient : _lineWidth;
            set { _lineWidth = value; }
        }

        private double _errorLineWidth = 1;
        public double ErrorLineWidth
        {
            get => IsHighlighted ? _errorLineWidth * HighlightCoefficient : _errorLineWidth;
            set { _errorLineWidth = value; }
        }

        public float ErrorCapSize = 3;

        public float _markerSize = 5;
        public float MarkerSize
        {
            get => IsHighlighted ? _markerSize * HighlightCoefficient : _markerSize;
            set { _markerSize = value; }
        }
        private float _markerLineWidth = 1;
        public float MarkerLineWidth
        {
            get => IsHighlighted ? (float)_lineWidth * HighlightCoefficient : _markerLineWidth;
            set { _markerLineWidth = value; }
        }

        public bool StepDisplay = false;

        public bool IsHighlighted { get; set; } = false;
        public float HighlightCoefficient { get; set; } = 2;

        [Obsolete("Scatter plot arrowheads have been deprecated. Use the Arrow plot type instead.", true)]
        public bool IsArrow { get => ArrowheadWidth > 0 && ArrowheadLength > 0; }

        [Obsolete("Scatter plot arrowheads have been deprecated. Use the Arrow plot type instead.", true)]
        public float ArrowheadWidth = 0;

        [Obsolete("Scatter plot arrowheads have been deprecated. Use the Arrow plot type instead.", true)]
        public float ArrowheadLength = 0;

        // TODO: think about better/additional API ?
        public int? MinRenderIndex { get; set; }
        public int? MaxRenderIndex { get; set; }

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

            if (MaxRenderIndex != null)
            {
                if ((MaxRenderIndex > Ys.Length - 1) || MaxRenderIndex < 0)
                    throw new IndexOutOfRangeException("maxRenderIndex must be a valid index for ys[]");
            }

            if (MinRenderIndex != null)
            {
                if (MinRenderIndex < 0)
                    throw new IndexOutOfRangeException("minRenderIndex must be between 0 and maxRenderIndex");
                if (MaxRenderIndex != null && MinRenderIndex > MaxRenderIndex)
                    throw new IndexOutOfRangeException("minRenderIndex must be between 0 and maxRenderIndex");
            }


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
            int from = MinRenderIndex ?? 0;
            int to = MaxRenderIndex ?? (Xs.Length - 1);

            // TODO: don't use an array for this
            double[] limits = new double[4];

            if (XError == null)
            {
                var XsRange = Xs.Skip(from).Take(to - from + 1);
                limits[0] = XsRange.Min();
                limits[1] = XsRange.Max();
            }
            else
            {
                var XsAndError = Xs.Zip(XError, (x, e) => (x, e)).Skip(from).Take(to - from + 1);
                limits[0] = XsAndError.Min(p => p.x - p.e);
                limits[1] = XsAndError.Max(p => p.x + p.e);
            }

            if (YError == null)
            {
                var YsRange = Ys.Skip(from).Take(to - from + 1);
                limits[2] = YsRange.Min();
                limits[3] = YsRange.Max();
            }
            else
            {
                var YsAndError = Ys.Zip(YError, (y, e) => (y, e)).Skip(from).Take(to - from + 1);
                limits[2] = YsAndError.Min(p => p.y - p.e);
                limits[3] = YsAndError.Max(p => p.y + p.e);
            }

            if (double.IsNaN(limits[0]) || double.IsNaN(limits[1]))
                throw new InvalidOperationException("X data must not contain NaN");
            if (double.IsNaN(limits[2]) || double.IsNaN(limits[3]))
                throw new InvalidOperationException("Y data must not contain NaN");

            if (double.IsInfinity(limits[0]) || double.IsInfinity(limits[1]))
                throw new InvalidOperationException("X data must not contain Infinity");
            if (double.IsInfinity(limits[2]) || double.IsInfinity(limits[3]))
                throw new InvalidOperationException("Y data must not contain Infinity");

            return new AxisLimits(
                xMin: limits[0] + OffsetX,
                xMax: limits[1] + OffsetX,
                yMin: limits[2] + OffsetY,
                yMax: limits[3] + OffsetY);
        }

        public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            if (IsVisible == false)
                return;

            using (var gfx = GDI.Graphics(bmp, dims, lowQuality))
            using (var penLine = GDI.Pen(LineColor, LineWidth, LineStyle, true))
            using (var penLineError = GDI.Pen(LineColor, ErrorLineWidth, LineStyle.Solid, true))
            {
                int from = MinRenderIndex ?? 0;
                int to = MaxRenderIndex ?? (Xs.Length - 1);
                PointF[] points = new PointF[to - from + 1];
                for (int i = from; i <= to; i++)
                {
                    float x = dims.GetPixelX(Xs[i] + OffsetX);
                    float y = dims.GetPixelY(Ys[i] + OffsetY);
                    if (float.IsNaN(x) || float.IsNaN(y))
                        throw new NotImplementedException("Data must not contain NaN");
                    points[i - from] = new PointF(x, y);
                }

                if (YError != null)
                {
                    for (int i = 0; i < points.Count(); i++)
                    {
                        double yWithOffset = Ys[i] + OffsetY;
                        float yBot = dims.GetPixelY(yWithOffset - YError[i + from]);
                        float yTop = dims.GetPixelY(yWithOffset + YError[i + from]);
                        gfx.DrawLine(penLineError, points[i].X, yBot, points[i].X, yTop);
                        gfx.DrawLine(penLineError, points[i].X - ErrorCapSize, yBot, points[i].X + ErrorCapSize, yBot);
                        gfx.DrawLine(penLineError, points[i].X - ErrorCapSize, yTop, points[i].X + ErrorCapSize, yTop);
                    }
                }

                if (XError != null)
                {
                    for (int i = 0; i < points.Length; i++)
                    {
                        double xWithOffset = Xs[i] + OffsetX;
                        float xLeft = dims.GetPixelX(xWithOffset - XError[i + from]);
                        float xRight = dims.GetPixelX(xWithOffset + XError[i + from]);
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
                        gfx.DrawLines(penLine, points);
                    }
                }

                // draw a marker at each point
                if ((MarkerSize > 0) && (MarkerShape != MarkerShape.none))
                {
                    MarkerTools.DrawMarkers(gfx, points, MarkerShape, MarkerSize, MarkerColor, MarkerLineWidth);
                }
            }
        }

        public LegendItem[] GetLegendItems()
        {
            var singleLegendItem = new LegendItem(this)
            {
                label = Label,
                color = LineColor,
                lineStyle = LineStyle,
                lineWidth = LineWidth,
                markerShape = MarkerShape,
                markerSize = MarkerSize,
            };
            return new LegendItem[] { singleLegendItem };
        }

        /// <summary>
        /// Return the X/Y coordinates of the point nearest the X position
        /// </summary>
        /// <param name="x">X position in plot space</param>
        /// <returns></returns>
        public (double x, double y, int index) GetPointNearestX(double x)
        {
            int from = MinRenderIndex ?? 0;
            int to = MaxRenderIndex ?? (Xs.Length - 1);
            double minDistance = Math.Abs(Xs[from] - x);
            int minIndex = 0;
            for (int i = from; i <= to; i++)
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

        /// <summary>
        /// Return the X/Y coordinates of the point nearest the Y position
        /// </summary>
        /// <param name="y">Y position in plot space</param>
        /// <returns></returns>
        public (double x, double y, int index) GetPointNearestY(double y)
        {
            int from = MinRenderIndex ?? 0;
            int to = MaxRenderIndex ?? (Ys.Length - 1);
            double minDistance = Math.Abs(Ys[from] - y);
            int minIndex = 0;
            for (int i = from; i <= to; i++)
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

        /// <summary>
        /// Return the position and index of the data point nearest the given coordinate
        /// </summary>
        /// <param name="x">location in coordinate space</param>
        /// <param name="y">location in coordinate space</param>
        /// <param name="xyRatio">Ratio of pixels per unit (X/Y) when rendered</param>
        public (double x, double y, int index) GetPointNearest(double x, double y, double xyRatio = 1)
        {
            int from = MinRenderIndex ?? 0;
            int to = MaxRenderIndex ?? (Ys.Length - 1);

            List<(double x, double y)> points = Xs.Zip(Ys, (first, second) => (first, second)).Skip(from).Take(to - from + 1).ToList();

            double xyRatioSquared = xyRatio * xyRatio;
            double pointDistanceSquared(double x1, double y1) =>
                (x1 - x) * (x1 - x) * xyRatioSquared + (y1 - y) * (y1 - y);

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
