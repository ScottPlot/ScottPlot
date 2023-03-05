using ScottPlot.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing;
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
        public string[] DataPointLabels { get; set; }
        public Drawing.Font DataPointLabelFont { get; set; } = new();

        public enum NanBehavior
        {
            /// <summary>
            /// Throw a <see cref="NotImplementedException"/> if <see cref="Xs"/> or <see cref="Ys"/> contains <see cref="double.NaN"/>
            /// </summary>
            Throw,

            /// <summary>
            /// Ignore points where X or Y is <see cref="double.NaN"/>, drawing a line between adjacent non-NaN points.
            /// </summary>
            Ignore,

            /// <summary>
            /// Treat points where X or Y is <see cref="double.NaN"/> as missing data and render the scatter plot as a 
            /// broken line with gaps indicating NaN points.
            /// </summary>
            Gap
        }

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
        public string Label { get; set; } = null;
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

        private float _markerSize = 5;
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

        /// <summary>
        /// If enabled, scatter plot points will be connected by square corners rather than straight diagnal lines
        /// </summary>
        public bool StepDisplay { get; set; } = false;

        /// <summary>
        /// Describes orientation of steps if <see cref="StepDisplay"/> is enabled.
        /// If true, lines will extend to the right before ascending or descending to the level of the following point.
        /// </summary>
        public bool StepDisplayRight { get; set; } = true;

        /// <summary>
        /// If enabled, points will be connected by smooth lines instead of straight diagnal lines.
        /// <see cref="SmoothTension"/> adjusts the smoothnes of the lines.
        /// </summary>
        public bool Smooth = false;

        /// <summary>
        /// Tension to use for smoothing when <see cref="Smooth"/> is enabled
        /// </summary>
        public double SmoothTension = 0.5;

        /// <summary>
        /// Defines behavior when <see cref="Xs"/> or <see cref="Ys"/> contains <see cref="double.NaN"/>
        /// </summary>
        public NanBehavior OnNaN = NanBehavior.Throw;

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
            return OnNaN switch
            {
                NanBehavior.Throw => GetAxisLimitsThrowOnNaN(),
                NanBehavior.Ignore => GetAxisLimitsIgnoreNaN(),
                NanBehavior.Gap => GetAxisLimitsIgnoreNaN(),
                _ => throw new NotImplementedException($"{nameof(OnNaN)} behavior not yet supported: {OnNaN}"),
            };
        }

        private AxisLimits GetAxisLimitsIgnoreNaN()
        {
            ValidateData(deep: false);
            int from = MinRenderIndex ?? 0;
            int to = MaxRenderIndex ?? (Xs.Length - 1);

            // TODO: don't use an array for this
            double[] limits = { double.NaN, double.NaN, double.NaN, double.NaN };

            if (XError == null)
            {
                var XsRange = Xs.Skip(from).Take(to - from + 1).Where(x => !double.IsNaN(x));
                if (XsRange.Any())
                {
                    limits[0] = XsRange.Min();
                    limits[1] = XsRange.Max();
                }
            }
            else
            {
                var XsAndError = Xs.Zip(XError, (x, e) => (x, e)).Skip(from).Take(to - from + 1).Where(p => !double.IsNaN(p.x + p.e));
                if (XsAndError.Any())
                {
                    limits[0] = XsAndError.Min(p => p.x - p.e);
                    limits[1] = XsAndError.Max(p => p.x + p.e);
                }
            }

            if (YError == null)
            {
                var YsRange = Ys.Skip(from).Take(to - from + 1).Where(x => !double.IsNaN(x));
                if (YsRange.Any())
                {
                    limits[2] = YsRange.Min();
                    limits[3] = YsRange.Max();
                }
            }
            else
            {
                var YsAndError = Ys.Zip(YError, (y, e) => (y, e)).Skip(from).Take(to - from + 1).Where(p => !double.IsNaN(p.y + p.e));
                if (YsAndError.Any())
                {
                    limits[2] = YsAndError.Min(p => p.y - p.e);
                    limits[3] = YsAndError.Max(p => p.y + p.e);
                }
            }

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

        private AxisLimits GetAxisLimitsThrowOnNaN()
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

                if (OnNaN == NanBehavior.Throw)
                {
                    foreach (PointF point in points)
                    {
                        if (float.IsNaN(point.X) || float.IsNaN(point.Y))
                            throw new NotImplementedException($"Data must not contain NaN if {nameof(OnNaN)} is {OnNaN}");
                    }

                    DrawLines(points, gfx, penLine);
                }
                else if (OnNaN == NanBehavior.Ignore)
                {
                    DrawLinesIngoringNaN(points, gfx, penLine);
                }
                else if (OnNaN == NanBehavior.Gap)
                {
                    DrawLinesWithGaps(points, gfx, penLine);
                }

                if (DataPointLabels is not null)
                {
                    for (var i = 0; i < DataPointLabels.Length; i++)
                    {
                        var label = DataPointLabels[i];
                        if (!string.IsNullOrEmpty(label))
                        {
                            gfx.TranslateTransform(points[i].X, points[i].Y);
                            gfx.RotateTransform(DataPointLabelFont.Rotation);

                            var (dX, dY) = GDI.TranslateString(gfx, label, DataPointLabelFont);
                            gfx.TranslateTransform(-dX, -dY);

                            using var font = GDI.Font(DataPointLabelFont);
                            using var fontBrush = new SolidBrush(DataPointLabelFont.Color);
                            gfx.DrawString(label, font, fontBrush, new PointF(0, 0));

                            GDI.ResetTransformPreservingScale(gfx, dims);
                        }
                    }
                }

                // draw a marker at each point
                if ((MarkerSize > 0) && (MarkerShape != MarkerShape.none))
                {
                    MarkerTools.DrawMarkers(gfx, points, MarkerShape, MarkerSize, MarkerColor, MarkerLineWidth);
                }
            }
        }

        private void DrawLines(PointF[] points, Graphics gfx, Pen penLine)
        {
            bool isLineVisible = LineWidth > 0 && points.Length > 1 && LineStyle != LineStyle.None;
            if (!isLineVisible)
                return;

            if (StepDisplay)
            {
                PointF[] pointsStep = GetStepDisplayPoints(points, StepDisplayRight);
                gfx.DrawLines(penLine, pointsStep);
            }
            else if (Smooth)
            {
                gfx.DrawCurve(penLine, points, (float)SmoothTension);
            }
            else
            {
                gfx.DrawLines(penLine, points);
            }
        }

        private void DrawLinesIngoringNaN(PointF[] points, Graphics gfx, Pen penLine)
        {
            PointF[] pointsWithoutNaNs = points.Where(pt => !double.IsNaN(pt.X) && !double.IsNaN(pt.Y)).ToArray();
            DrawLines(pointsWithoutNaNs, gfx, penLine);
        }

        private void DrawLinesWithGaps(PointF[] points, Graphics gfx, Pen penLine)
        {
            List<PointF> segment = new();
            for (int i = 0; i < points.Length; i++)
            {
                if (double.IsNaN(points[i].X) || double.IsNaN(points[i].Y))
                {
                    if (segment.Any())
                    {
                        DrawLines(segment.ToArray(), gfx, penLine);
                        segment.Clear();
                    }
                }
                else
                {
                    segment.Add(points[i]);
                }
            }

            if (segment.Any())
            {
                DrawLines(segment.ToArray(), gfx, penLine);
            }
        }

        /// <summary>
        /// Convert scatter plot points (connected by diagnal lines) to step plot points (connected by right angles)
        /// by inserting an extra point between each of the original data points to result in L-shaped steps.
        /// </summary>
        /// <param name="points">Array of corner positions</param>
        /// <param name="right">Indicates that a line will extend to the right before rising or falling.</param>
        public static PointF[] GetStepDisplayPoints(PointF[] points, bool right)
        {
            PointF[] pointsStep = new PointF[points.Length * 2 - 1];

            int offsetX = right ? 1 : 0;
            int offsetY = right ? 0 : 1;

            for (int i = 0; i < points.Length - 1; i++)
            {
                pointsStep[i * 2] = points[i];
                pointsStep[i * 2 + 1] = new PointF(points[i + offsetX].X, points[i + offsetY].Y);
            }

            pointsStep[pointsStep.Length - 1] = points[points.Length - 1];

            return pointsStep;
        }

        public LegendItem[] GetLegendItems()
        {
            var singleItem = new LegendItem(this)
            {
                label = Label,
                color = LineColor,
                lineStyle = LineStyle,
                lineWidth = LineWidth,
                markerShape = MarkerShape,
                markerSize = MarkerSize,
            };
            return LegendItem.Single(singleItem);
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
            double minDistance = double.PositiveInfinity;
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
            double minDistance = double.PositiveInfinity;
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

            double minDistance = double.PositiveInfinity;
            int minIndex = 0;
            for (int i = 0; i < points.Count; i++)
            {
                if (double.IsNaN(points[i].x) || double.IsNaN(points[i].y))
                    continue;

                double currDistance = pointDistanceSquared(points[i].x, points[i].y);
                if (currDistance < minDistance)
                {
                    minIndex = i;
                    minDistance = currDistance;
                }
            }

            return (Xs[minIndex], Ys[minIndex], minIndex);
        }

        /// <summary>
        /// Return the vertical limits of the data between horizontal positions (inclusive)
        /// </summary>
        public (double yMin, double yMax) GetYDataRange(double xMin, double xMax)
        {
            var includedYs = Ys.Where((y, i) => Xs[i] >= xMin && Xs[i] <= xMax);
            return (includedYs.Min(), includedYs.Max());
        }
    }
}
