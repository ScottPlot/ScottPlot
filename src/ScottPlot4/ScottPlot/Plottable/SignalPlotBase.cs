using ScottPlot.Drawing;
using ScottPlot.Drawing.Colormaps;
using ScottPlot.MinMaxSearchStrategies;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ScottPlot.Plottable
{
    public abstract class SignalPlotBase<T> : IPlottable, IHasLine, IHasMarker, IHighlightable, IHasColor, IHasPointsGenericX<double, T> where T : struct, IComparable
    {
        protected IMinMaxSearchStrategy<T> Strategy = new SegmentedTreeMinMaxSearchStrategy<T>();
        public int XAxisIndex { get; set; } = 0;
        public int YAxisIndex { get; set; } = 0;
        public bool IsVisible { get; set; } = true;
        public bool StepDisplay { get; set; } = false;
        public bool Smooth { get; set; } = false;
        public double SmoothTension { get; set; } = 0.5f;

        /// <summary>
        /// Describes orientation of steps if <see cref="StepDisplay"/> is enabled.
        /// If true, lines will extend to the right before ascending or descending to the level of the following point.
        /// </summary>
        public bool StepDisplayRight { get; set; } = true;

        private float _markerSize = 5;
        public float MarkerSize
        {
            get => IsHighlighted ? _markerSize * HighlightCoefficient : _markerSize;
            set { _markerSize = value; }
        }

        public MarkerShape MarkerShape { get; set; } = MarkerShape.filledCircle;
        public double OffsetX { get; set; } = 0;
        public T OffsetY { get; set; } = default;
        public double OffsetYAsDouble
        {
            get
            {
                var v = OffsetY;
                return NumericConversion.GenericToDouble(ref v);
            }
        }

        private double _lineWidth = 1;
        public double LineWidth
        {
            get => IsHighlighted ? _lineWidth * HighlightCoefficient : _lineWidth;
            set { _lineWidth = value; }
        }

        private float _markerLineWidth;
        public float MarkerLineWidth
        {
            get => IsHighlighted ? (float)_markerLineWidth * HighlightCoefficient : _markerLineWidth;
            set { _markerLineWidth = value; }
        }

        public string Label { get; set; } = null;
        public Color Color { get => LineColor; set { LineColor = value; MarkerColor = value; } }
        public Color LineColor { get; set; } = Color.Black;
        public Color MarkerColor { get; set; } = Color.Black;
        public LineStyle LineStyle { get; set; } = LineStyle.Solid;

        public bool IsHighlighted { get; set; } = false;
        public float HighlightCoefficient { get; set; } = 2;

        /// <summary>
        /// If enabled, parallel processing will be used to calculate pixel positions for high density datasets.
        /// </summary>
        public bool UseParallel { get; set; } = true;


        /// <summary>
        /// If fill above and/or below is enabled, this defines the baseline level.
        /// </summary>
        public double BaselineY { get; set; } = 0;

        /// <summary>
        /// If fill is enabled, a baseline will be drawn using this color.
        /// </summary>
        public Color BaselineColor { get; set; } = Color.Black;

        /// <summary>
        /// If fill is enabled, a baseline will be drawn using this width.
        /// </summary>
        public float BaselineWidth { get; set; } = 1;

        /// <summary>
        /// If fill is enabled, this color will be used to fill the area below the curve above BaselineY.
        /// </summary>
        [Obsolete("Use the Fill() methods of this object to configure this setting")]
        public Color? GradientFillColor1 { get => _GradientFillColor1; set => _GradientFillColor1 = value; }
        private Color? _GradientFillColor1 = null;

        /// <summary>
        /// If fill is enabled, this color will be used to fill the area above the curve below BaselineY.
        /// </summary>
        [Obsolete("Use the Fill() methods of this object to configure this setting")]
        public Color? GradientFillColor2 { get => _GradientFillColor2; set => _GradientFillColor2 = value; }
        private Color? _GradientFillColor2 = null;

        protected FillType _FillType = FillType.NoFill;
        protected Color? _FillColor1 = null;
        protected Color? _FillColor2 = null;

        /// <summary>
        /// When markers are visible on the line (low density mode) this is True
        /// </summary>
        protected bool ShowMarkersInLegend { get; set; } = false;

        protected T[] _Ys;
        public virtual T[] Ys
        {
            get => _Ys;
            set
            {
                if (value == null)
                    throw new Exception("Y data cannot be null");
                _Ys = value;
                Strategy.SourceArray = _Ys;
            }
        }

        private double _SampleRate = 1;
        public double SampleRate
        {
            get => _SampleRate;
            set
            {
                if (value <= 0)
                    throw new Exception("SampleRate must be greater then zero");
                _SampleRate = value;
                _SamplePeriod = 1.0 / value;
            }
        }

        private double _SamplePeriod = 1;
        public double SamplePeriod
        {
            get => _SamplePeriod;
            set
            {
                if (_SamplePeriod <= 0)
                    throw new Exception("SamplePeriod must be greater then zero");
                _SamplePeriod = value;
                _SampleRate = 1.0 / value;
            }
        }

        protected int _MinRenderIndex = 0;
        public int MinRenderIndex
        {
            get => _MinRenderIndex;
            set
            {
                if (value < 0)
                    throw new ArgumentException("MinRenderIndex must be positive");
                _MinRenderIndex = value;
            }
        }
        protected int _maxRenderIndex = 0;
        public int MaxRenderIndex
        {
            get => _maxRenderIndex;
            set
            {
                if (value < 0)
                    throw new ArgumentException("MaxRenderIndex must be positive");
                _maxRenderIndex = value;
            }
        }

        private int DensityLevelCount = 0;
        private Color[] PenColorsByDensity;
        public Color[] DensityColors
        {
            set
            {
                if (value != null)
                {
                    // turn the ramp into a pen triangle
                    DensityLevelCount = value.Length * 2 - 1;
                    PenColorsByDensity = new Color[DensityLevelCount];
                    for (int i = 0; i < value.Length; i++)
                    {
                        PenColorsByDensity[i] = value[i];
                        PenColorsByDensity[DensityLevelCount - 1 - i] = value[i];
                    }
                }
            }
        }

        [Obsolete("Use the Fill() methods of this object to configure this setting")]
        public FillType FillType
        {
            get => _FillType;
            set
            {
                _FillType = value;
            }
        }

        [Obsolete("Use the Fill() methods of this object to configure this setting")]
        public Color? FillColor1
        {
            get => _FillColor1;
            set
            {
                _FillColor1 = value;
            }
        }

        [Obsolete("Use the Fill() methods of this object to configure this setting")]
        public Color? FillColor2
        {
            get => _FillColor2;
            set
            {
                _FillColor2 = value;
            }
        }

        /// <summary>
        /// This expression adds two parameters of the generic type used by this signal plot.
        /// </summary>
        private readonly Func<T, T, T> AddYsGenericExpression = NumericConversion.CreateAddFunction<T>();

        /// <summary>
        /// Add two Y values (of the generic type used by this signal plot) and return the result as a double
        /// </summary>
        private double AddYs(T y1, T y2)
        {
            var v = AddYsGenericExpression(y1, y2);
            return NumericConversion.GenericToDouble(ref v);
        }

        /// <summary>
        /// Add two Y values (of the generic type used by this signal plot) and return the result as a the same type
        /// </summary>
        private T AddYsGeneric(T y1, T y2) => AddYsGenericExpression(y1, y2);

        public SignalPlotBase()
        {
        }

        /// <summary>
        /// Replace a single Y value
        /// </summary>
        /// <param name="index">array index to replace</param>
        /// <param name="newValue">new value</param>
        public void Update(int index, T newValue) => Strategy.updateElement(index, newValue);

        /// <summary>
        /// Replace a range of Y values
        /// </summary>
        /// <param name="firstIndex">index to begin replacing</param>
        /// <param name="lastIndex">last index to replace</param>
        /// <param name="newData">source for new data</param>
        /// <param name="fromData">source data offset</param>
        public void Update(int firstIndex, int lastIndex, T[] newData, int fromData = 0)
        {
            if (firstIndex < 0 || firstIndex > Ys.Length - 1)
                throw new InvalidOperationException($"{nameof(firstIndex)} cannot exceed the dimensions of the existing {nameof(Ys)} array");
            if (lastIndex > Ys.Length - 1)
                throw new InvalidOperationException($"{nameof(lastIndex)} cannot exceed the dimensions of the existing {nameof(Ys)} array");
            Strategy.updateRange(firstIndex, lastIndex, newData, fromData);
        }

        /// <summary>
        /// Replace all Y values from the given index through the end of the array
        /// </summary>
        /// <param name="firstIndex">first index to begin replacing</param>
        /// <param name="newData">new values</param>
        public void Update(int firstIndex, T[] newData)
        {
            if (firstIndex < 0 || firstIndex > Ys.Length - 1)
                throw new InvalidOperationException($"{nameof(firstIndex)} cannot exceed the dimensions of the existing {nameof(Ys)} array");
            Update(firstIndex, firstIndex + newData.Length, newData);
        }

        /// <summary>
        /// Replace all Y values with new ones
        /// </summary>
        /// <param name="newData">new Y values</param>
        public void Update(T[] newData)
        {
            if (newData.Length > Ys.Length)
                throw new InvalidOperationException($"{nameof(newData)} cannot exceed the dimensions of the existing {nameof(Ys)} array");
            Update(0, newData.Length, newData);
        }

        public virtual AxisLimits GetAxisLimits()
        {
            if (Ys.Length == 0)
                return AxisLimits.NoLimits;

            double xMin = _SamplePeriod * MinRenderIndex;
            double xMax = _SamplePeriod * MaxRenderIndex;
            Strategy.MinMaxRangeQuery(MinRenderIndex, MaxRenderIndex, out double yMin, out double yMax);

            if (double.IsNaN(yMin) || double.IsNaN(yMax))
                throw new InvalidOperationException("Signal data must not contain NaN");
            if (double.IsInfinity(yMin) || double.IsInfinity(yMax))
                throw new InvalidOperationException("Signal data must not contain Infinity");

            double offsetY = OffsetYAsDouble;
            return new AxisLimits(
                xMin: xMin + OffsetX,
                xMax: xMax + OffsetX,
                yMin: yMin + offsetY,
                yMax: yMax + offsetY);
        }

        /// <summary>
        /// Render when the data is zoomed out so much that it just looks like a vertical line.
        /// </summary>
        private void RenderSingleLine(PlotDimensions dims, Graphics gfx, Pen penHD)
        {
            // this function is for when the graph is zoomed so far out its entire display is a single vertical pixel column
            Strategy.MinMaxRangeQuery(MinRenderIndex, MaxRenderIndex, out double yMin, out double yMax);
            double offsetY = OffsetYAsDouble;
            PointF point1 = new(dims.GetPixelX(OffsetX), dims.GetPixelY(yMin + offsetY));
            PointF point2 = new(dims.GetPixelX(OffsetX), dims.GetPixelY(yMax + offsetY));
            gfx.DrawLine(penHD, point1, point2);
        }

        /// <summary>
        /// Render when the data is zoomed in such that there is more than 1 column per data point.
        /// Rendering is accomplished by drawing a straight line from point to point.
        /// </summary>
        private void RenderLowDensity(PlotDimensions dims, Graphics gfx, int visibleIndex1, int visibleIndex2, Pen penLD)
        {
            int capacity = visibleIndex2 - visibleIndex1 + 2;
            List<PointF> linePoints = new(capacity);
            if (visibleIndex2 > _Ys.Length - 2)
                visibleIndex2 = _Ys.Length - 2;
            if (visibleIndex2 > MaxRenderIndex - 1)
                visibleIndex2 = MaxRenderIndex - 1;
            if (visibleIndex1 < 0)
                visibleIndex1 = 0;
            if (visibleIndex1 < MinRenderIndex)
                visibleIndex1 = MinRenderIndex;

            for (int i = visibleIndex1; i <= visibleIndex2 + 1; i++)
            {
                double yCoordinateWithOffset = AddYs(Ys[i], OffsetY);
                float yPixel = dims.GetPixelY(yCoordinateWithOffset);
                float xPixel = dims.GetPixelX(_SamplePeriod * i + OffsetX);
                PointF linePoint = new(xPixel, yPixel);
                linePoints.Add(linePoint);
            }

            if (linePoints.Count > 1)
            {

                PointF[] pointsArray = linePoints.ToArray();
                ValidatePoints(pointsArray);

                if (StepDisplay)
                    pointsArray = ScatterPlot.GetStepDisplayPoints(pointsArray, StepDisplayRight);

                if (LineWidth > 0 && LineStyle != LineStyle.None)
                    if (Smooth)
                    {
                        gfx.DrawCurve(penLD, pointsArray, (float)SmoothTension);
                    }
                    else
                    {
                        gfx.DrawLines(penLD, pointsArray);
                    }


                switch (_FillType)
                {
                    case FillType.NoFill:
                        break;
                    case FillType.FillAbove:
                        FillToInfinity(dims, gfx, linePoints[0].X, linePoints[linePoints.Count - 1].X, pointsArray, true);
                        break;
                    case FillType.FillBelow:
                        FillToInfinity(dims, gfx, linePoints[0].X, linePoints[linePoints.Count - 1].X, pointsArray, false);
                        break;
                    case FillType.FillAboveAndBelow:
                        FillToBaseline(dims, gfx, linePoints[0].X, linePoints[linePoints.Count - 1].X, pointsArray, BaselineY);
                        break;
                    default:
                        throw new InvalidOperationException("unsupported fill type");
                }
            }

            if ((MarkerSize > 0) && (MarkerShape != MarkerShape.none))
            {
                // make markers transition away smoothly by making them smaller as the user zooms out
                float pixelsBetweenPoints = (float)((Ys.Length > 1 ? _SamplePeriod : 1) * dims.DataWidth / dims.XSpan);
                float zoomTransitionScale = Math.Min(1, pixelsBetweenPoints / 10);
                float markerPxDiameter = MarkerSize * zoomTransitionScale;
                float markerPxRadius = markerPxDiameter / 2;
                if (markerPxRadius > .25)
                {
                    ShowMarkersInLegend = true;
                    MarkerTools.DrawMarkers(gfx, linePoints, MarkerShape, markerPxDiameter, MarkerColor, MarkerLineWidth);
                }
                else
                {
                    ShowMarkersInLegend = false;
                }
            }
        }

        private class IntervalMinMax
        {
            public float x;
            public float Min;
            public float Max;
            public IntervalMinMax(float x, float Min, float Max)
            {
                this.x = x;
                this.Min = Min;
                this.Max = Max;
            }
            public IEnumerable<PointF> GetPoints()
            {
                yield return new PointF(x, Min);
                yield return new PointF(x, Max);
            }
        }

        private IntervalMinMax CalcInterval(int xPx, double offsetPoints, double columnPointCount, PlotDimensions dims)
        {
            int index1 = (int)(offsetPoints + columnPointCount * xPx);
            int index2 = (int)(offsetPoints + columnPointCount * (xPx + 1));

            if (index1 < 0)
                index1 = 0;
            if (index1 < MinRenderIndex)
                index1 = MinRenderIndex;

            if (index2 > _Ys.Length - 1)
                index2 = _Ys.Length - 1;
            if (index2 > MaxRenderIndex)
                index2 = MaxRenderIndex;

            // get the min and max value for this column                
            Strategy.MinMaxRangeQuery(index1, index2, out double lowestValue, out double highestValue);
            float yPxHigh = dims.GetPixelY(lowestValue + OffsetYAsDouble);
            float yPxLow = dims.GetPixelY(highestValue + OffsetYAsDouble);
            return new IntervalMinMax(xPx, yPxLow, yPxHigh);
        }

        /// <summary>
        /// Render the data when there is more than one data point per pixel column.
        /// Each pixel column therefore represents multiple data points.
        /// Rendering is optimized by determining the min/max for each pixel column, then a single line is drawn connecting those values.
        /// </summary>
        private void RenderHighDensity(PlotDimensions dims, Graphics gfx, double offsetPoints, double columnPointCount, Pen penHD)
        {
            int dataColumnFirst = (int)Math.Ceiling((-1 - offsetPoints + MinRenderIndex) / columnPointCount - 1);
            int dataColumnLast = (int)Math.Ceiling((MaxRenderIndex - offsetPoints) / columnPointCount);
            dataColumnFirst = Math.Max(0, dataColumnFirst);
            dataColumnLast = Math.Min((int)dims.DataWidth, dataColumnLast);
            if (dataColumnFirst >= dataColumnLast)
                return;

            var columns = Enumerable.Range(dataColumnFirst, dataColumnLast - dataColumnFirst);
            float xPixelStart = dataColumnFirst + dims.DataOffsetX;
            float xPixelEnd = dataColumnLast + dims.DataOffsetX;

            IEnumerable<IntervalMinMax> intervals;
            if (UseParallel)
            {
                intervals = columns
                    .AsParallel()
                    .AsOrdered()
                    .Select(xPx => CalcInterval(xPx, offsetPoints, columnPointCount, dims))
                    .AsSequential();
            }
            else
            {
                intervals = columns
                    .Select(xPx => CalcInterval(xPx, offsetPoints, columnPointCount, dims));
            }

            PointF[] linePoints = intervals
                .SelectMany(c => c.GetPoints())
                .ToArray();

            // adjust order of points to enhance anti-aliasing
            PointF buf;
            for (int i = 1; i < linePoints.Length / 2; i++)
            {
                if (linePoints[i * 2].Y >= linePoints[i * 2 - 1].Y)
                {
                    buf = linePoints[i * 2];
                    linePoints[i * 2] = linePoints[i * 2 + 1];
                    linePoints[i * 2 + 1] = buf;
                }
            }

            for (int i = 0; i < linePoints.Length; i++)
                linePoints[i].X += dims.DataOffsetX;

            if (linePoints.Length > 0)
            {
                ValidatePoints(linePoints);
                gfx.DrawLines(penHD, linePoints);
            }

            switch (_FillType)
            {
                case FillType.NoFill:
                    break;
                case FillType.FillAbove:
                    FillToInfinity(dims, gfx, xPixelStart, xPixelEnd, linePoints, true);
                    break;
                case FillType.FillBelow:
                    FillToInfinity(dims, gfx, xPixelStart, xPixelEnd, linePoints, false);
                    break;
                case FillType.FillAboveAndBelow:
                    FillToBaseline(dims, gfx, xPixelStart, xPixelEnd, linePoints, BaselineY);
                    break;
                default:
                    throw new InvalidOperationException("unsupported fill type");
            }
        }

        /// <summary>
        /// Shade the region abvove or below the curve (to infinity) by drawing a polygon to the edge of the visible plot area.
        /// </summary>
        internal void FillToInfinity(PlotDimensions dims, Graphics gfx, float xPxStart, float xPxEnd, PointF[] linePoints, bool fillToPositiveInfinity)
        {
            if ((int)(xPxEnd - xPxStart) == 0 || (int)dims.Height == 0)
                return;
            float minVal = 0;
            float maxVal = (dims.DataHeight * (fillToPositiveInfinity ? -1 : 1)) + dims.DataOffsetY;

            PointF first = new(xPxStart, maxVal);
            PointF last = new(xPxEnd, maxVal);

            PointF[] points = new PointF[] { first }
                            .Concat(linePoints)
                            .Concat(new PointF[] { last })
                            .ToArray();

            Rectangle gradientRectangle = new(
                    x: (int)first.X,
                    y: (int)minVal - (fillToPositiveInfinity ? 2 : 0),
                    width: (int)(last.X - first.X),
                    height: (int)dims.Height);

            using var brush = new LinearGradientBrush(gradientRectangle, _FillColor1.Value, _GradientFillColor1 ?? _FillColor1.Value, LinearGradientMode.Vertical);
            gfx.FillPolygon(brush, points);
        }

        private PointF? GetIntersection(PointF point1, PointF point2, PointF baselineStart, PointF baselineEnd)
        {
            double a1 = point2.Y - point1.Y;
            double b1 = point1.X - point2.X;
            double c1 = a1 * (point1.X) + b1 * (point1.Y);

            double a2 = baselineEnd.Y - baselineStart.Y;
            double b2 = baselineStart.X - baselineEnd.X;
            double c2 = a2 * (baselineStart.X) + b2 * (baselineStart.Y);

            double d = a1 * b2 - a2 * b1;

            if (d == 0)
            {
                // Lines do not intersect. This could also be the case if the plot is zoomed out too much.
                return null;
            }
            else
            {
                double x = (b2 * c1 - b1 * c2) / d;
                double y = (a1 * c2 - a2 * c1) / d;
                return new PointF((float)x, (float)y);
            }
        }

        /// <summary>
        /// Shade the region abvove and below the curve (to the baseline level) by drawing two polygons
        /// </summary>
        internal void FillToBaseline(PlotDimensions dims, Graphics gfx, float xPxStart, float xPxEnd, PointF[] linePoints, double baselineY)
        {
            int baseline = (int)dims.GetPixelY(baselineY);

            PointF first = new(xPxStart, baseline);
            PointF last = new(xPxEnd, baseline);

            PointF[] points = new PointF[] { first }
                            .Concat(linePoints)
                            .Concat(new PointF[] { last })
                            .ToArray();

            PointF baselinePointStart = new(linePoints[0].X, baseline);
            PointF baselinePointEnd = new(linePoints[linePoints.Length - 1].X, baseline);

            var pointList = points.ToList();
            int newlyAddedItems = 0;
            for (int i = 1; i < points.Length + newlyAddedItems; ++i)
            {
                if ((pointList[i - 1].Y > baseline && pointList[i].Y < baseline) ||
                    (pointList[i - 1].Y < baseline && pointList[i].Y > baseline))
                {
                    var intersection = GetIntersection(pointList[i], pointList[i - 1], baselinePointStart, baselinePointEnd);
                    if (intersection != null)
                    {
                        pointList.Insert(i, intersection.Value);
                        newlyAddedItems++;
                        i++;
                    }
                }
            }

            var dataAreaRect = new Rectangle(0, 0, (int)dims.Width, (int)dims.Height);

            // Above graph
            if (dataAreaRect.Height > 0 && dataAreaRect.Width > 0)
            {
                var color = _GradientFillColor1 ?? _FillColor1.Value;
                var edgeColor = _FillColor1.Value;
                using var brush = new LinearGradientBrush(dataAreaRect, color, edgeColor, LinearGradientMode.Vertical);
                gfx.FillPolygon(brush,
                    new PointF[] { first }
                    .Concat(pointList.Where(p => p.Y <= baseline).ToArray())
                    .Concat(new PointF[] { last })
                    .ToArray());
            }

            // Below graph
            if (dataAreaRect.Height > 0 && dataAreaRect.Width > 0)
            {
                var color = _FillColor2.Value;
                var edgeColor = _GradientFillColor2 ?? _FillColor2.Value;
                using var brush = new LinearGradientBrush(dataAreaRect, color, edgeColor, LinearGradientMode.Vertical);
                gfx.FillPolygon(brush,
                    new PointF[] { first }
                    .Concat(pointList.Where(p => p.Y >= baseline).ToArray())
                    .Concat(new PointF[] { last })
                    .ToArray());
            }

            // Draw baseline
            using var baselinePen = GDI.Pen(BaselineColor, BaselineWidth);
            gfx.DrawLine(baselinePen, baselinePointStart, baselinePointEnd);
        }

        /// <summary>
        /// Render similar to high density mode except use multiple colors to represent density distributions.
        /// </summary>
        private void RenderHighDensityDistributionParallel(PlotDimensions dims, Graphics gfx, double offsetPoints, double columnPointCount)
        {
            int xPxStart = (int)Math.Ceiling((-1 - offsetPoints) / columnPointCount - 1);
            int xPxEnd = (int)Math.Ceiling((_Ys.Length - offsetPoints) / columnPointCount);
            xPxStart = Math.Max(0, xPxStart);
            xPxEnd = Math.Min((int)dims.DataWidth, xPxEnd);
            if (xPxStart >= xPxEnd)
                return;

            int capacity = (xPxEnd - xPxStart) * 2 + 1;
            List<PointF> linePoints = new(capacity);

            var levelValues = Enumerable.Range(xPxStart, xPxEnd - xPxStart)
                .AsParallel()
                .AsOrdered()
                .Select(xPx =>
                {
                    // determine data indexes for this pixel column
                    int index1 = (int)(offsetPoints + columnPointCount * xPx);
                    int index2 = (int)(offsetPoints + columnPointCount * (xPx + 1));

                    if (index1 < 0)
                        index1 = 0;
                    if (index1 > _Ys.Length - 1)
                        index1 = _Ys.Length - 1;
                    if (index2 > _Ys.Length - 1)
                        index2 = _Ys.Length - 1;

                    var indexes = Enumerable.Range(0, DensityLevelCount + 1).Select(x => x * (index2 - index1 - 1) / (DensityLevelCount));

                    var levelsValues = new ArraySegment<T>(_Ys, index1, index2 - index1)
                        .OrderBy(x => x)
                        .Where((y, i) => indexes.Contains(i)).ToArray();
                    return (xPx, levelsValues);
                })
                .ToArray();

            List<PointF[]> linePointsLevels = levelValues
                .Select(x => x.levelsValues
                              .Select(y => new PointF(
                                    x: x.xPx + dims.DataOffsetX,
                                    y: dims.GetPixelY(AddYs(y, OffsetY))))
                              .ToArray())
                .ToList();

            for (int i = 0; i < DensityLevelCount; i++)
            {
                linePoints.Clear();
                for (int j = 0; j < linePointsLevels.Count; j++)
                {
                    if (i + 1 < linePointsLevels[j].Length)
                    {
                        linePoints.Add(linePointsLevels[j][i]);
                        linePoints.Add(linePointsLevels[j][i + 1]);
                    }
                }

                PointF[] pointsArray = linePoints.ToArray();
                ValidatePoints(pointsArray);

                using (Pen densityPen = GDI.Pen(PenColorsByDensity[i]))
                {
                    gfx.DrawLines(densityPen, pointsArray);
                }

                switch (_FillType)
                {
                    case FillType.NoFill:
                        break;
                    case FillType.FillAbove:
                        FillToInfinity(dims, gfx, xPxStart, xPxEnd, pointsArray, true);
                        break;
                    case FillType.FillBelow:
                        FillToInfinity(dims, gfx, xPxStart, xPxEnd, pointsArray, false);
                        break;
                    case FillType.FillAboveAndBelow:
                        FillToBaseline(dims, gfx, xPxStart, xPxEnd, pointsArray, BaselineY);
                        break;
                    default:
                        throw new InvalidOperationException("unsupported fill type");
                }
            }
        }

        public override string ToString()
        {
            string label = string.IsNullOrWhiteSpace(this.Label) ? "" : $" ({this.Label})";
            return $"PlottableSignalBase{label} with {PointCount} points ({typeof(T).Name})";
        }

        public int PointCount { get => _Ys.Length; }

        public LegendItem[] GetLegendItems()
        {
            var singleItem = new LegendItem(this)
            {
                label = Label,
                color = Color,
                lineStyle = LineStyle,
                lineWidth = LineWidth,
                markerShape = ShowMarkersInLegend ? MarkerShape : MarkerShape.none,
                markerSize = ShowMarkersInLegend ? MarkerSize : 0
            };
            return LegendItem.Single(singleItem);
        }

        public virtual void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            if (Ys.Length == 0)
                return;

            using var gfx = GDI.Graphics(bmp, dims, lowQuality);
            using var penLD = GDI.Pen(LineColor, (float)LineWidth, LineStyle, true);
            using var penHD = GDI.Pen(LineColor, (float)LineWidth, LineStyle.Solid, true);

            double dataSpanUnits = _Ys.Length * _SamplePeriod;
            double columnSpanUnits = dims.XSpan / dims.DataWidth;
            double columnPointCount = (columnSpanUnits / dataSpanUnits) * _Ys.Length;
            double offsetUnits = dims.XMin - OffsetX;
            double offsetPoints = offsetUnits / _SamplePeriod;
            int visibleIndex1 = (int)(offsetPoints);
            int visibleIndex2 = (int)(offsetPoints + columnPointCount * (dims.DataWidth + 1));
            int visiblePointCount = visibleIndex2 - visibleIndex1;
            double pointsPerPixelColumn = visiblePointCount / dims.DataWidth;
            double dataWidthPx2 = visibleIndex2 - visibleIndex1 + 2;
            bool densityLevelsAvailable = DensityLevelCount > 0 && pointsPerPixelColumn > DensityLevelCount;
            double firstPointX = dims.GetPixelX(OffsetX);
            double lastPointX = dims.GetPixelX(_SamplePeriod * (_Ys.Length - 1) + OffsetX);
            double dataWidthPx = lastPointX - firstPointX;
            double columnsWithData = Math.Min(dataWidthPx, dataWidthPx2);

            if (columnsWithData < 1 && Ys.Length > 1)
            {
                RenderSingleLine(dims, gfx, penHD);
            }
            else if (pointsPerPixelColumn > 1 && Ys.Length > 1)
            {
                if (densityLevelsAvailable)
                    RenderHighDensityDistributionParallel(dims, gfx, offsetPoints, columnPointCount);
                else
                    RenderHighDensity(dims, gfx, offsetPoints, columnPointCount, penHD);
            }
            else
            {
                RenderLowDensity(dims, gfx, visibleIndex1, visibleIndex2, penLD);
            }
        }

        protected void ValidatePoints(PointF[] points)
        {
            foreach (PointF pt in points)
                if (float.IsNaN(pt.Y))
                    throw new InvalidOperationException("Data must not contain NaN");
        }

        public virtual void ValidateData(bool deep = false)
        {
            // check Y values
            if (Ys is null)
                throw new InvalidOperationException($"{nameof(Ys)} cannot be null");
            if (Ys.Length == 0)
                return; // no additional validation required since nothing will be rendered
            if (deep)
                Validate.AssertAllReal(nameof(Ys), Ys);

            // check render indexes
            if (MinRenderIndex < 0 || MinRenderIndex > MaxRenderIndex)
                throw new IndexOutOfRangeException("minRenderIndex must be between 0 and maxRenderIndex");
            if ((MaxRenderIndex > Ys.Length - 1) || MaxRenderIndex < 0)
                throw new IndexOutOfRangeException("maxRenderIndex must be a valid index for ys[]");
            if (MaxRenderIndex > Ys.Length - 1)
                throw new IndexOutOfRangeException("maxRenderIndex must be a valid index for ys[]");
            if (MinRenderIndex > MaxRenderIndex)
                throw new IndexOutOfRangeException("minRenderIndex must be lower maxRenderIndex");

            // check misc styling options
            if (_FillColor1 is null && _FillType != FillType.NoFill)
                throw new InvalidOperationException($"A Color must be assigned to FillColor1 to use fill type '{_FillType}'");
            if (_FillColor1 is null && _FillType == FillType.FillAboveAndBelow)
                throw new InvalidOperationException($"A Color must be assigned to FillColor2 to use fill type '{_FillType}'");
        }

        /// <summary>
        /// Return the index for the data point corresponding to the given X coordinate
        /// </summary>
        private int GetIndexForX(double x)
        {
            int index = (int)((x - OffsetX + SamplePeriod / 2) * SampleRate);
            index = Math.Max(index, MinRenderIndex);
            index = Math.Min(index, MaxRenderIndex);
            return index;
        }

        /// <summary>
        /// Return the X coordinate of the data point at the given index
        /// </summary>
        private double IndexToX(int index)
        {
            return index * SampleRate + OffsetX;
        }

        /// <summary>
        /// Return the X/Y coordinates of the point nearest the X position
        /// </summary>
        /// <param name="x">X position in plot space</param>
        /// <returns></returns>
        public (double x, T y, int index) GetPointNearestX(double x)
        {
            int index = GetIndexForX(x);
            double pointX = OffsetX + index * SamplePeriod;
            T pointY = AddYsGeneric(Ys[index], OffsetY);
            return (pointX, pointY, index);
        }

        /// <summary>
        /// Configure the signal plot to only show the curve with no filled area above or below it
        /// </summary>
        public void FillDisable()
        {
            _FillType = FillType.NoFill;
            _GradientFillColor1 = null;
            _GradientFillColor2 = null;
        }

        /// <summary>
        /// Show a solid color beneath the curve
        /// </summary>
        public void FillBelow(System.Drawing.Color? color = null, double alpha = .2)
        {
            _FillType = FillType.FillBelow;
            _FillColor1 = GDI.Semitransparent(color ?? Color, alpha);
        }

        /// <summary>
        /// Show a two-color gradient beneath the curve
        /// </summary>
        public void FillBelow(System.Drawing.Color upperColor, System.Drawing.Color lowerColor, double alpha = .2)
        {
            _FillType = FillType.FillBelow;
            _FillColor1 = GDI.Semitransparent(upperColor, alpha);
            _GradientFillColor1 = GDI.Semitransparent(lowerColor, alpha);
        }

        /// <summary>
        /// Show a solid color above the curve
        /// </summary>
        public void FillAbove(System.Drawing.Color? color = null, double alpha = .2)
        {
            _FillType = FillType.FillAbove;
            _FillColor1 = GDI.Semitransparent(color ?? Color, alpha);
        }

        /// <summary>
        /// Show a two-color gradient above the curve
        /// </summary>
        public void FillAbove(System.Drawing.Color lowerColor, System.Drawing.Color upperColor, double alpha = .2)
        {
            _FillType = FillType.FillAbove;
            _FillColor1 = GDI.Semitransparent(upperColor, alpha);
            _GradientFillColor1 = GDI.Semitransparent(lowerColor, alpha);
        }

        /// <summary>
        /// Fill the area between the curve and the <see cref="BaselineY"/> value
        /// </summary>
        public void FillAboveAndBelow(System.Drawing.Color colorAbove, System.Drawing.Color colorBelow, double alpha = .2)
        {
            _FillType = FillType.FillAboveAndBelow;
            _FillColor1 = GDI.Semitransparent(colorAbove, alpha);
            _FillColor2 = GDI.Semitransparent(colorBelow, alpha);
        }

        /// <summary>
        /// Fill the area between the curve and the edge of the display area using two gradients
        /// </summary>
        /// <param name="above1">Color above the line next to the curve</param>
        /// <param name="above2">Color above the line next to the upper edge of the plot area</param>
        /// <param name="below1">Color below the line next to the curve</param>
        /// <param name="below2">Color below the line next to the lower edge of the plot area</param>
        /// <param name="alpha">Apply this opacity to all colors</param>
        public void FillAboveAndBelow(System.Drawing.Color above1, System.Drawing.Color above2,
            System.Drawing.Color below1, System.Drawing.Color below2, double alpha = .2)
        {
            _FillType = FillType.FillAboveAndBelow;

            _FillColor1 = GDI.Semitransparent(above1, alpha);
            _GradientFillColor1 = GDI.Semitransparent(above2, alpha);

            _FillColor2 = GDI.Semitransparent(below2, alpha);
            _GradientFillColor2 = GDI.Semitransparent(below1, alpha);
        }

        /// <summary>
        /// Return the vertical limits of the data between horizontal positions (inclusive)
        /// </summary>
        public (T yMin, T yMax) GetYDataRange(double xMin, double xMax)
        {
            int startIndex = GetIndexForX(xMin);
            int endIndex = GetIndexForX(xMax);

            if (IndexToX(endIndex) < xMax)
            {
                endIndex = Math.Min(endIndex + 1, MaxRenderIndex);
            }

            Strategy.MinMaxRangeQuery(startIndex, endIndex, out double yMin, out double yMax);

            NumericConversion.DoubleToGeneric<T>(yMin, out T genericYMin);
            NumericConversion.DoubleToGeneric<T>(yMax, out T genericYMax);

            return (genericYMin, genericYMax);
        }
    }
}
