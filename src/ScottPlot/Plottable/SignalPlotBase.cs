using ScottPlot.Drawing;
using ScottPlot.MinMaxSearchStrategies;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace ScottPlot.Plottable
{
    public class SignalPlotBase<T> : IPlottable, IHasPoints, IExportable where T : struct, IComparable
    {
        protected IMinMaxSearchStrategy<T> Strategy = new SegmentedTreeMinMaxSearchStrategy<T>();
        protected bool MaxRenderIndexLowerYSPromise = false;
        protected bool MaxRenderIndexHigherMinRenderIndexPromise = false;
        protected bool FillColor1MustBeSetPromise = false;
        protected bool FillColor2MustBeSetPromise = false;
        public int XAxisIndex { get; set; } = 0;
        public int YAxisIndex { get; set; } = 0;
        public bool IsVisible { get; set; } = true;
        public float MarkerSize { get; set; } = 5;
        public double OffsetX { get; set; } = 0;
        public double OffsetY { get; set; } = 0;
        public double LineWidth { get; set; } = 1;
        public string Label { get; set; } = null;
        public Color Color { get; set; } = Color.Green;
        public LineStyle LineStyle { get; set; } = LineStyle.Solid;
        public bool UseParallel { get; set; } = true;
        public int BaselineY { get; set; } = 0;
        public Color? GradientFillColor1 { get; set; } = null;
        public Color? GradientFillColor2 { get; set; } = null;
        private bool ShowMarkers { get; set; } = false; // this gets set in the render loop

        protected T[] _Ys;
        public virtual T[] Ys
        {
            get => _Ys;
            set
            {
                if (value == null)
                    throw new Exception("Y data cannot be null");

                MaxRenderIndexLowerYSPromise = MaxRenderIndex > value.Length - 1;

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

                MaxRenderIndexHigherMinRenderIndexPromise = value > MaxRenderIndex;

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

                MaxRenderIndexHigherMinRenderIndexPromise = MinRenderIndex > value;

                MaxRenderIndexLowerYSPromise = value > _Ys.Length - 1;

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

        private FillType _FillType = FillType.NoFill;
        public FillType FillType
        {
            get => _FillType;
            set
            {
                FillColor1MustBeSetPromise = (_FillColor1 == null && value != FillType.NoFill);

                FillColor2MustBeSetPromise = (_FillColor2 == null && value == FillType.FillAboveAndBelow);

                _FillType = value;
            }
        }

        private Color? _FillColor1 = null;
        public Color? FillColor1
        {
            get => _FillColor1;
            set
            {
                FillColor1MustBeSetPromise = (value == null && FillType != FillType.NoFill);

                _FillColor1 = value;
            }
        }

        private Color? _FillColor2 = null;
        public Color? FillColor2
        {
            get => _FillColor2;
            set
            {
                FillColor2MustBeSetPromise = (value == null && FillType == FillType.FillAboveAndBelow);

                _FillColor2 = value;
            }
        }

        /// <summary>
        /// Replace a single Y value
        /// </summary>
        public void Update(int index, T newValue) => Strategy.updateElement(index, newValue);

        /// <summary>
        /// Replace a range of Y values
        /// </summary>
        public void Update(int firstIndex, int lastIndex, T[] newData, int fromData = 0) =>
            Strategy.updateRange(firstIndex, lastIndex, newData, fromData);

        /// <summary>
        /// Replace all Y values from the given index through the end of the array
        /// </summary>
        public void Update(int firstIndex, T[] newData) => Update(firstIndex, newData.Length, newData);

        /// <summary>
        /// Replace all Y values with new ones
        /// </summary>
        public void Update(T[] newData) => Update(0, newData.Length, newData);

        public virtual AxisLimits GetAxisLimits()
        {
            double xMin = _SamplePeriod * MinRenderIndex;
            double xMax = _SamplePeriod * MaxRenderIndex;
            Strategy.MinMaxRangeQuery(MinRenderIndex, MaxRenderIndex, out double yMin, out double yMax);

            if (double.IsNaN(yMin) || double.IsNaN(yMax))
                throw new InvalidOperationException("Signal data must not contain NaN");
            if (double.IsInfinity(yMin) || double.IsInfinity(yMax))
                throw new InvalidOperationException("Signal data must not contain Infinity");

            return new AxisLimits(xMin + OffsetX, xMax + OffsetX, yMin + OffsetY, yMax + OffsetY);
        }

        private void RenderSingleLine(PlotDimensions dims, Graphics gfx, Pen penHD)
        {
            // this function is for when the graph is zoomed so far out its entire display is a single vertical pixel column
            double yMin, yMax;
            Strategy.MinMaxRangeQuery(MinRenderIndex, MaxRenderIndex, out yMin, out yMax);
            PointF point1 = new PointF(dims.GetPixelX(OffsetX), dims.GetPixelY(yMin + OffsetY));
            PointF point2 = new PointF(dims.GetPixelX(OffsetX), dims.GetPixelY(yMax + OffsetY));
            gfx.DrawLine(penHD, point1, point2);
        }

        private void RenderLowDensity(PlotDimensions dims, Graphics gfx, int visibleIndex1, int visibleIndex2, Brush brush, Pen penLD, Pen penHD)
        {
            // this function is for when the graph is zoomed in so individual data points can be seen

            List<PointF> linePoints = new List<PointF>(visibleIndex2 - visibleIndex1 + 2);
            if (visibleIndex2 > _Ys.Length - 2)
                visibleIndex2 = _Ys.Length - 2;
            if (visibleIndex2 > MaxRenderIndex)
                visibleIndex2 = MaxRenderIndex - 1;
            if (visibleIndex1 < 0)
                visibleIndex1 = 0;
            if (visibleIndex1 < MinRenderIndex)
                visibleIndex1 = MinRenderIndex;

            for (int i = visibleIndex1; i <= visibleIndex2 + 1; i++)
                linePoints.Add(new PointF(dims.GetPixelX(_SamplePeriod * i + OffsetX), dims.GetPixelY(Strategy.SourceElement(i) + OffsetY)));

            if (linePoints.Count > 1)
            {

                PointF[] pointsArray = linePoints.ToArray();
                ValidatePoints(pointsArray);

                if (penLD.Width > 0)
                    gfx.DrawLines(penHD, pointsArray);

                if (FillType == FillType.FillAbove || FillType == FillType.FillBelow)
                {
                    FillAboveOrBelow(dims, gfx, linePoints[0].X, linePoints[linePoints.Count - 1].X, pointsArray, FillType);
                }
                else if (FillType == FillType.FillAboveAndBelow)
                {
                    FillAboveAndBelow(dims, gfx, linePoints[0].X, linePoints[linePoints.Count - 1].X, pointsArray, BaselineY);
                }

                if (MarkerSize > 0)
                {
                    // make markers transition away smoothly by making them smaller as the user zooms out
                    float pixelsBetweenPoints = (float)(_SamplePeriod * dims.DataWidth / dims.XSpan);
                    float zoomTransitionScale = Math.Min(1, pixelsBetweenPoints / 10);
                    float markerPxDiameter = MarkerSize * zoomTransitionScale;
                    float markerPxRadius = markerPxDiameter / 2;
                    if (markerPxRadius > .25)
                    {
                        ShowMarkers = true;

                        // adjust marker offset to improve rendering on Linux and MacOS
                        float markerOffsetX = (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) ? 0 : 1;
                        foreach (PointF point in linePoints)
                            gfx.FillEllipse(brush: brush,
                                x: point.X - markerPxRadius + markerOffsetX, y: point.Y - markerPxRadius,
                                width: markerPxDiameter, height: markerPxDiameter);
                    }
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
            float yPxHigh = dims.GetPixelY(lowestValue + OffsetY);
            float yPxLow = dims.GetPixelY(highestValue + OffsetY);
            return new IntervalMinMax(xPx, yPxLow, yPxHigh);
        }

        private void RenderHighDensity(PlotDimensions dims, Graphics gfx, double offsetPoints, double columnPointCount, Pen penHD)
        {
            int xPxStart = (int)Math.Ceiling((-1 - offsetPoints + MinRenderIndex) / columnPointCount - 1);
            int xPxEnd = (int)Math.Ceiling((MaxRenderIndex - offsetPoints) / columnPointCount);
            xPxStart = Math.Max(0, xPxStart);
            xPxEnd = Math.Min((int)dims.DataWidth, xPxEnd);
            if (xPxStart >= xPxEnd)
                return;

            var columns = Enumerable.Range(xPxStart, xPxEnd - xPxStart);

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

            if (FillType == FillType.FillAbove || FillType == FillType.FillBelow)
            {
                FillAboveOrBelow(dims, gfx, xPxStart, xPxEnd, linePoints, FillType);
            }
            else if (FillType == FillType.FillAboveAndBelow)
            {
                FillAboveAndBelow(dims, gfx, xPxStart, xPxEnd, linePoints, this.BaselineY);
            }
        }

        private void FillAboveOrBelow(PlotDimensions dims, Graphics gfx, float xPxStart, float xPxEnd, PointF[] linePoints, FillType fillType)
        {
            if (fillType == FillType.FillAbove || fillType == FillType.FillBelow)
            {
                float minVal = 0;
                float maxVal = (dims.DataHeight * (fillType == FillType.FillAbove ? -1 : 1));

                PointF first = new PointF(xPxStart + dims.DataOffsetX, maxVal);
                PointF last = new PointF(xPxEnd + dims.DataOffsetX, maxVal);

                PointF[] points = new PointF[] { first }
                                .Concat(linePoints)
                                .Concat(new PointF[] { last })
                                .ToArray();

                Rectangle gradientRectangle = new Rectangle(
                        new Point((int)first.X, (int)minVal - (fillType == FillType.FillAbove ? 2 : 0)),
                        new Size(
                            (int)(last.X - first.X),
                            (int)(maxVal - minVal) + 2 * (fillType == FillType.FillAbove ? -1 : 1)));

                using (var brush = new LinearGradientBrush(gradientRectangle, _FillColor1.Value, GradientFillColor1 ?? _FillColor1.Value, LinearGradientMode.Vertical))
                {
                    gfx.FillPolygon(brush, points);
                }
            }
            else
            {
                throw new Exception("Invalid fill type");
            }
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

        private void FillAboveAndBelow(PlotDimensions dims, Graphics gfx, float xPxStart, float xPxEnd, PointF[] linePoints, int baseline)
        {
            baseline = (int)dims.GetPixelY(baseline);

            PointF first = new PointF(xPxStart + dims.DataOffsetX, baseline);
            PointF last = new PointF(xPxEnd + dims.DataOffsetX, baseline);

            PointF[] points = new PointF[] { first }
                            .Concat(linePoints)
                            .Concat(new PointF[] { last })
                            .ToArray();

            PointF baselinePointStart = new PointF(linePoints[0].X, baseline);
            PointF baselinePointEnd = new PointF(linePoints[linePoints.Length - 1].X, baseline);

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

            // Above graph
            var aboveRect = GetFillRectangle(dims, xPxStart, xPxEnd, FillType.FillAbove);
            if (aboveRect.Height != 0 && aboveRect.Width != 0)
            {
                using (var brush = new LinearGradientBrush(aboveRect, _FillColor1.Value, GradientFillColor1 ?? _FillColor1.Value, LinearGradientMode.Vertical))
                {
                    gfx.FillPolygon(brush,
                        new PointF[] { first }
                        .Concat(pointList.Where(p => p.Y <= baseline).ToArray())
                        .Concat(new PointF[] { last })
                        .ToArray());
                }
            }

            // Below graph
            var belowRect = GetFillRectangle(dims, xPxStart, xPxEnd, FillType.FillBelow);
            if (belowRect.Height != 0 && belowRect.Width != 0)
            {
                using (var brush = new LinearGradientBrush(belowRect, _FillColor2.Value, GradientFillColor2 ?? _FillColor2.Value, LinearGradientMode.Vertical))
                {
                    gfx.FillPolygon(brush,
                        new PointF[] { first }
                        .Concat(pointList.Where(p => p.Y >= baseline).ToArray())
                        .Concat(new PointF[] { last })
                        .ToArray());
                }
            }

            // Draw baseline
            using (var baselinePen = GDI.Pen(Color.Black))
            {
                gfx.DrawLine(baselinePen, baselinePointStart, baselinePointEnd);
            }
        }

        private Rectangle GetFillRectangle(PlotDimensions dims, float startX, float xPxEnd, FillType fillType)
        {
            float maxVal = (dims.DataHeight * (fillType == FillType.FillAbove ? -1 : 1));

            Rectangle rectangle = new Rectangle((int)startX, 0, (int)(xPxEnd - startX), (int)maxVal);

            return rectangle;
        }

        private void RenderHighDensityDistributionParallel(PlotDimensions dims, Graphics gfx, double offsetPoints, double columnPointCount)
        {
            int xPxStart = (int)Math.Ceiling((-1 - offsetPoints) / columnPointCount - 1);
            int xPxEnd = (int)Math.Ceiling((_Ys.Length - offsetPoints) / columnPointCount);
            xPxStart = Math.Max(0, xPxStart);
            xPxEnd = Math.Min((int)dims.DataWidth, xPxEnd);
            if (xPxStart >= xPxEnd)
                return;
            List<PointF> linePoints = new List<PointF>((xPxEnd - xPxStart) * 2 + 1);

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
                                .Select(y => new PointF(x.xPx + dims.DataOffsetX, dims.GetPixelY(Convert.ToDouble(y) + OffsetY)))
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

                if (FillType == FillType.FillAbove || FillType == FillType.FillBelow)
                {
                    FillAboveOrBelow(dims, gfx, xPxStart, xPxEnd, pointsArray, FillType);
                }
                else if (FillType == FillType.FillAboveAndBelow)
                {
                    FillAboveAndBelow(dims, gfx, xPxStart, xPxEnd, pointsArray, BaselineY);
                }
            }
        }

        public override string ToString()
        {
            string label = string.IsNullOrWhiteSpace(this.Label) ? "" : $" ({this.Label})";
            return $"PlottableSignalBase{label} with {PointCount} points ({typeof(T).Name})";
        }

        public void SaveCSV(string filePath, string delimiter = ", ", string separator = "\n")
        {
            System.IO.File.WriteAllText(filePath, GetCSV(delimiter, separator));
        }

        public string GetCSV(string delimiter = ", ", string separator = "\n")
        {
            StringBuilder csv = new StringBuilder();
            for (int i = 0; i < _Ys.Length; i++)
                csv.AppendFormat(CultureInfo.InvariantCulture, "{0}{1}{2}{3}", OffsetX + i * _SamplePeriod, delimiter, Strategy.SourceElement(i) + OffsetY, separator);
            return csv.ToString();
        }

        public int PointCount { get => _Ys.Length; }

        public LegendItem[] GetLegendItems()
        {
            var singleLegendItem = new LegendItem()
            {
                label = Label,
                color = Color,
                lineStyle = LineStyle,
                lineWidth = LineWidth,
                markerShape = ShowMarkers ? MarkerShape.filledCircle : MarkerShape.none,
                markerSize = ShowMarkers ? MarkerSize : 0
            };
            return new LegendItem[] { singleLegendItem };
        }

        public virtual void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            using (var gfx = GDI.Graphics(bmp, dims, lowQuality))
            using (var brush = GDI.Brush(Color))
            using (var penLD = GDI.Pen(Color, (float)LineWidth, LineStyle, true))
            using (var penHD = GDI.Pen(Color, (float)LineWidth, LineStyle.Solid, true))
            {
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

                double firstPointX = dims.GetPixelX(OffsetX);
                double lastPointX = dims.GetPixelX(_SamplePeriod * (_Ys.Length - 1) + OffsetX);
                double dataWidthPx = lastPointX - firstPointX;

                // set this now, and let the render function change it if it happens
                ShowMarkers = false;

                // use different rendering methods based on how dense the data is on screen
                if ((dataWidthPx <= 1) || (dataWidthPx2 <= 1))
                {
                    RenderSingleLine(dims, gfx, penHD);
                }
                else if (pointsPerPixelColumn > 1)
                {
                    if (DensityLevelCount > 0 && pointsPerPixelColumn > DensityLevelCount)
                        RenderHighDensityDistributionParallel(dims, gfx, offsetPoints, columnPointCount);
                    else
                        RenderHighDensity(dims, gfx, offsetPoints, columnPointCount, penHD);
                }
                else
                {
                    RenderLowDensity(dims, gfx, visibleIndex1, visibleIndex2, brush, penLD, penHD);
                }
            }
        }

        private void ValidatePoints(PointF[] points)
        {
            foreach (PointF pt in points)
                if (float.IsNaN(pt.Y))
                    throw new InvalidOperationException("Data must not contain NaN");
        }

        public void ValidateData(bool deep = false)
        {
            // check Y values
            if (Ys is null)
                throw new InvalidOperationException("ys cannot be null");
            if (deep)
                Validate.AssertAllReal("ys", Ys);

            // check render indexes
            if (MinRenderIndex < 0 || MinRenderIndex > MaxRenderIndex)
                throw new IndexOutOfRangeException("minRenderIndex must be between 0 and maxRenderIndex");
            if ((MaxRenderIndex > Ys.Length - 1) || MaxRenderIndex < 0)
                throw new IndexOutOfRangeException("maxRenderIndex must be a valid index for ys[]");
            if (MaxRenderIndexLowerYSPromise)
                throw new IndexOutOfRangeException("maxRenderIndex must be a valid index for ys[]");
            if (MaxRenderIndexHigherMinRenderIndexPromise)
                throw new IndexOutOfRangeException("minRenderIndex must be lower maxRenderIndex");

            // check misc styling options
            if (FillColor1MustBeSetPromise)
                throw new InvalidOperationException("A fill color needs to be specified if fill is used");
            if (FillColor2MustBeSetPromise)
                throw new InvalidOperationException("Two fill colors needs to be specified if fill above and below is used");
        }

        public (double x, double y, int index) GetPointNearestX(double x)
        {
            int index = (int)((x - OffsetX) / SamplePeriod);
            index = Math.Max(index, 0);
            index = Math.Min(index, Ys.Length - 1);
            return (OffsetX + index * SamplePeriod, Convert.ToDouble(Ys[index]) + OffsetY, index);
        }

        [Obsolete("Only GetPointNearestX() is appropraite for signal plots.", true)]
        public (double x, double y, int index) GetPointNearestY(double y) => throw new NotImplementedException();

        [Obsolete("Only GetPointNearestX() is appropraite for signal plots.", true)]
        public (double x, double y, int index) GetPointNearest(double x, double y) => throw new NotImplementedException();
    }
}
