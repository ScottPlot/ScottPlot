using ScottPlot.Config;
using ScottPlot.Diagnostic.Attributes;
using ScottPlot.Drawing;
using ScottPlot.MinMaxSearchStrategies;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace ScottPlot
{
    public class PlottableSignalBase<T> : Plottable, IPlottable, IExportable where T : struct, IComparable
    {
        protected bool MaxRenderIndexLowerYSPromise = false;
        protected bool MaxRenderIndexHigherMinRenderIndexPromise = false;
        protected bool FillColor1MustBeSetPromise = false;
        protected bool FillColor2MustBeSetPromise = false;

        protected IMinMaxSearchStrategy<T> minmaxSearchStrategy = new SegmentedTreeMinMaxSearchStrategy<T>();

        [FiniteNumbers, EqualLength]
        protected T[] _ys;
        public virtual T[] ys
        {
            get => _ys;
            set
            {
                if (value == null)
                    throw new Exception("Y data cannot be null");

                MaxRenderIndexLowerYSPromise = maxRenderIndex > value.Length - 1;

                _ys = value;
                minmaxSearchStrategy.SourceArray = _ys;
            }
        }
        [FiniteNumbers]
        private double _sampleRate = 1;
        public double sampleRate
        {
            get => _sampleRate;
            set
            {
                if (value <= 0)
                    throw new Exception("SampleRate must be greater then zero");
                _sampleRate = value;
                _samplePeriod = 1.0 / value;
            }
        }
        [FiniteNumbers]
        private double _samplePeriod = 1;
        public double samplePeriod
        {
            get => _samplePeriod;
            set
            {
                if (_samplePeriod <= 0)
                    throw new Exception("SamplePeriod must be greater then zero");
                _samplePeriod = value;
            }
        }
        public float markerSize { get; set; } = 5;
        [FiniteNumbers]
        public double xOffset { get; set; } = 0;
        [FiniteNumbers]
        public double yOffset { get; set; } = 0;
        public double lineWidth { get; set; } = 1;

        protected int _minRenderIndex = 0;
        public int minRenderIndex
        {
            get => _minRenderIndex;
            set
            {
                if (value < 0)
                    throw new ArgumentException("MinRenderIndex must be positive");

                MaxRenderIndexHigherMinRenderIndexPromise = value > maxRenderIndex;

                _minRenderIndex = value;
            }
        }
        protected int _maxRenderIndex = 0;
        public int maxRenderIndex
        {
            get => _maxRenderIndex;
            set
            {
                if (value < 0)
                    throw new ArgumentException("MaxRenderIndex must be positive");

                MaxRenderIndexHigherMinRenderIndexPromise = minRenderIndex > value;

                MaxRenderIndexLowerYSPromise = value > _ys.Length - 1;

                _maxRenderIndex = value;
            }
        }
        private int densityLevelCount = 0;

        private Color[] PenColorsByDensity;
        public Color[] colorByDensity
        {
            set
            {
                if (value != null)
                {
                    // turn the ramp into a pen triangle
                    densityLevelCount = value.Length * 2 - 1;
                    PenColorsByDensity = new Color[densityLevelCount];
                    for (int i = 0; i < value.Length; i++)
                    {
                        PenColorsByDensity[i] = value[i];
                        PenColorsByDensity[densityLevelCount - 1 - i] = value[i];
                    }
                }
            }
        }

        public string label { get; set; } = null;
        public Color color { get; set; } = Color.Green;
        public LineStyle lineStyle { get; set; } = LineStyle.Solid;
        public bool useParallel { get; set; } = true;
        private FillType _fillType = FillType.NoFill;
        public FillType fillType
        {
            get => _fillType;
            set
            {
                FillColor1MustBeSetPromise = (_fillColor1 == null && value != FillType.NoFill);

                FillColor2MustBeSetPromise = (_fillColor2 == null && value == FillType.FillAboveAndBelow);

                _fillType = value;
            }
        }
        private Color? _fillColor1 = null;
        public Color? fillColor1
        {
            get => _fillColor1;
            set
            {
                FillColor1MustBeSetPromise = (value == null && fillType != FillType.NoFill);

                _fillColor1 = value;
            }
        }
        public Color? gradientFillColor1 { get; set; } = null;
        private Color? _fillColor2 = null;
        public Color? fillColor2
        {
            get => _fillColor2;
            set
            {
                FillColor2MustBeSetPromise = (value == null && fillType == FillType.FillAboveAndBelow);

                _fillColor2 = value;
            }
        }

        public Color? gradientFillColor2 { get; set; } = null;
        public int baseline { get; set; } = 0;

        public PlottableSignalBase()
        {
        }

        public void updateData(int index, T newValue)
        {
            minmaxSearchStrategy.updateElement(index, newValue);
        }

        public void updateData(int from, int to, T[] newData, int fromData = 0) // RangeUpdate
        {
            minmaxSearchStrategy.updateRange(from, to, newData, fromData);
        }

        public void updateData(int from, T[] newData)
        {
            updateData(from, newData.Length, newData);
        }

        public void updateData(T[] newData)
        {
            updateData(0, newData.Length, newData);
        }

        public override AxisLimits2D GetLimits()
        {
            double xMin = _samplePeriod * minRenderIndex;
            double xMax = _samplePeriod * maxRenderIndex;
            minmaxSearchStrategy.MinMaxRangeQuery(minRenderIndex, maxRenderIndex, out double yMin, out double yMax);
            return new AxisLimits2D(xMin + xOffset, xMax + xOffset, yMin + yOffset, yMax + yOffset);
        }

        private void RenderSingleLine(PlotDimensions dims, Graphics gfx, Pen penHD)
        {
            // this function is for when the graph is zoomed so far out its entire display is a single vertical pixel column
            double yMin, yMax;
            minmaxSearchStrategy.MinMaxRangeQuery(minRenderIndex, maxRenderIndex, out yMin, out yMax);
            PointF point1 = new PointF(dims.GetPixelX(xOffset), dims.GetPixelY(yMin + yOffset));
            PointF point2 = new PointF(dims.GetPixelX(xOffset), dims.GetPixelY(yMax + yOffset));
            gfx.DrawLine(penHD, point1, point2);
        }

        private bool markersAreVisible = false;

        private void RenderLowDensity(PlotDimensions dims, Graphics gfx, int visibleIndex1, int visibleIndex2, Brush brush, Pen penLD, Pen penHD)
        {
            // this function is for when the graph is zoomed in so individual data points can be seen

            List<PointF> linePoints = new List<PointF>(visibleIndex2 - visibleIndex1 + 2);
            if (visibleIndex2 > _ys.Length - 2)
                visibleIndex2 = _ys.Length - 2;
            if (visibleIndex2 > maxRenderIndex - 1)
                visibleIndex2 = maxRenderIndex - 1;
            if (visibleIndex1 < 0)
                visibleIndex1 = 0;
            if (visibleIndex1 < minRenderIndex)
                visibleIndex1 = minRenderIndex;

            for (int i = visibleIndex1; i <= visibleIndex2 + 1; i++)
                linePoints.Add(new PointF(dims.GetPixelX(_samplePeriod * i + xOffset), dims.GetPixelY(minmaxSearchStrategy.SourceElement(i) + yOffset)));

            if (linePoints.Count > 1)
            {
                if (penLD.Width > 0)
                    gfx.DrawLines(penHD, linePoints.ToArray());

                if (fillType == FillType.FillAbove || fillType == FillType.FillBelow)
                {
                    FillAboveOrBelow(dims, gfx, linePoints[0].X, linePoints[linePoints.Count - 1].X, linePoints.ToArray(), fillType);
                }
                else if (fillType == FillType.FillAboveAndBelow)
                {
                    FillAboveAndBelow(dims, gfx, linePoints[0].X, linePoints[linePoints.Count - 1].X, linePoints.ToArray(), this.baseline);
                }

                if (markerSize > 0)
                {
                    // make markers transition away smoothly by making them smaller as the user zooms out
                    float pixelsBetweenPoints = (float)(_samplePeriod * dims.DataWidth / dims.XSpan);
                    float zoomTransitionScale = Math.Min(1, pixelsBetweenPoints / 10);
                    float markerPxDiameter = markerSize * zoomTransitionScale;
                    float markerPxRadius = markerPxDiameter / 2;
                    if (markerPxRadius > .25)
                    {
                        markersAreVisible = true;

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
            if (index1 < minRenderIndex)
                index1 = minRenderIndex;

            if (index2 > _ys.Length - 1)
                index2 = _ys.Length - 1;
            if (index2 > maxRenderIndex)
                index2 = maxRenderIndex;

            // get the min and max value for this column                
            minmaxSearchStrategy.MinMaxRangeQuery(index1, index2, out double lowestValue, out double highestValue);
            float yPxHigh = dims.GetPixelY(lowestValue + yOffset);
            float yPxLow = dims.GetPixelY(highestValue + yOffset);
            return new IntervalMinMax(xPx, yPxLow, yPxHigh);
        }

        private void RenderHighDensity(PlotDimensions dims, Graphics gfx, double offsetPoints, double columnPointCount, Pen penHD)
        {
            int xPxStart = (int)Math.Ceiling((-1 - offsetPoints + minRenderIndex) / columnPointCount - 1);
            int xPxEnd = (int)Math.Ceiling((maxRenderIndex - offsetPoints) / columnPointCount);
            xPxStart = Math.Max(0, xPxStart);
            xPxEnd = Math.Min((int)dims.DataWidth, xPxEnd);
            if (xPxStart >= xPxEnd)
                return;

            var columns = Enumerable.Range(xPxStart, xPxEnd - xPxStart);

            IEnumerable<IntervalMinMax> intervals;
            if (useParallel)
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

            if (linePoints.Length > 0)
                gfx.DrawLines(penHD, linePoints);

            if (fillType == FillType.FillAbove || fillType == FillType.FillBelow)
            {
                FillAboveOrBelow(dims, gfx, xPxStart, xPxEnd, linePoints, fillType);
            }
            else if (fillType == FillType.FillAboveAndBelow)
            {
                FillAboveAndBelow(dims, gfx, xPxStart, xPxEnd, linePoints, this.baseline);
            }
        }

        private void FillAboveOrBelow(PlotDimensions dims, Graphics gfx, float xPxStart, float xPxEnd, PointF[] linePoints, FillType fillType)
        {
            if (fillType == FillType.FillAbove || fillType == FillType.FillBelow)
            {
                float minVal = 0;
                float maxVal = (dims.DataHeight * (fillType == FillType.FillAbove ? -1 : 1));

                PointF first = new PointF(xPxStart, maxVal);
                PointF last = new PointF(xPxEnd, maxVal);

                PointF[] points = new PointF[] { first }
                                .Concat(linePoints)
                                .Concat(new PointF[] { last })
                                .ToArray();

                Rectangle gradientRectangle = new Rectangle(
                        new Point((int)first.X, (int)minVal - (fillType == FillType.FillAbove ? 2 : 0)),
                        new Size(
                            (int)(last.X - first.X),
                            (int)(maxVal - minVal) + 2 * (fillType == FillType.FillAbove ? -1 : 1)));

                using (var brush = new LinearGradientBrush(gradientRectangle, _fillColor1.Value, gradientFillColor1 ?? _fillColor1.Value, LinearGradientMode.Vertical))
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

            PointF first = new PointF(xPxStart, baseline);
            PointF last = new PointF(xPxEnd, baseline);

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
                using (var brush = new LinearGradientBrush(aboveRect, _fillColor1.Value, gradientFillColor1 ?? _fillColor1.Value, LinearGradientMode.Vertical))
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
                using (var brush = new LinearGradientBrush(belowRect, _fillColor2.Value, gradientFillColor2 ?? _fillColor2.Value, LinearGradientMode.Vertical))
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
            int xPxEnd = (int)Math.Ceiling((_ys.Length - offsetPoints) / columnPointCount);
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
                    if (index1 > _ys.Length - 1)
                        index1 = _ys.Length - 1;
                    if (index2 > _ys.Length - 1)
                        index2 = _ys.Length - 1;

                    var indexes = Enumerable.Range(0, densityLevelCount + 1).Select(x => x * (index2 - index1 - 1) / (densityLevelCount));

                    var levelsValues = new ArraySegment<T>(_ys, index1, index2 - index1)
                        .OrderBy(x => x)
                        .Where((y, i) => indexes.Contains(i)).ToArray();
                    return (xPx, levelsValues);
                })
                .ToArray();

            List<PointF[]> linePointsLevels = levelValues
                .Select(x => x.levelsValues
                                .Select(y => new PointF(x.xPx, dims.GetPixelY(Convert.ToDouble(y) + yOffset)))
                                .ToArray())
                .ToList();

            for (int i = 0; i < densityLevelCount; i++)
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
                using (Pen densityPen = GDI.Pen(PenColorsByDensity[i]))
                {
                    gfx.DrawLines(densityPen, linePoints.ToArray());
                }


                if (fillType == FillType.FillAbove || fillType == FillType.FillBelow)
                {
                    FillAboveOrBelow(dims, gfx, xPxStart, xPxEnd, linePoints.ToArray(), fillType);
                }
                else if (fillType == FillType.FillAboveAndBelow)
                {
                    FillAboveAndBelow(dims, gfx, xPxStart, xPxEnd, linePoints.ToArray(), this.baseline);
                }
            }
        }

        public override void Render(Settings settings) => throw new NotImplementedException("Use the other Render method");

        public override string ToString()
        {
            string label = string.IsNullOrWhiteSpace(this.label) ? "" : $" ({this.label})";
            return $"PlottableSignalBase{label} with {GetPointCount()} points ({typeof(T).Name})";
        }

        public void SaveCSV(string filePath, string delimiter = ", ", string separator = "\n")
        {
            System.IO.File.WriteAllText(filePath, GetCSV(delimiter, separator));
        }

        public string GetCSV(string delimiter = ", ", string separator = "\n")
        {
            StringBuilder csv = new StringBuilder();
            for (int i = 0; i < _ys.Length; i++)
                csv.AppendFormat(CultureInfo.InvariantCulture, "{0}{1}{2}{3}", xOffset + i * _samplePeriod, delimiter, minmaxSearchStrategy.SourceElement(i) + yOffset, separator);
            return csv.ToString();
        }

        public override int GetPointCount()
        {
            return _ys.Length;
        }

        public override LegendItem[] GetLegendItems()
        {
            var singleLegendItem = new Config.LegendItem(label, color)
            {
                lineStyle = lineStyle,
                lineWidth = lineWidth,
                markerShape = (markersAreVisible) ? MarkerShape.filledCircle : MarkerShape.none,
                markerSize = (markersAreVisible) ? markerSize : 0
            };
            return new LegendItem[] { singleLegendItem };
        }

        public virtual void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            if (IsValidData() == false)
                throw new InvalidOperationException($"Invalid data: {ValidationErrorMessage}");

            using (Graphics gfx = Graphics.FromImage(bmp))
            using (var brush = new SolidBrush(color))
            using (var penLD = GDI.Pen(color, (float)lineWidth, lineStyle, true))
            using (var penHD = GDI.Pen(color, (float)lineWidth, LineStyle.Solid, true))
            {

                gfx.SmoothingMode = lowQuality ? SmoothingMode.HighSpeed : SmoothingMode.AntiAlias;
                gfx.TextRenderingHint = lowQuality ? TextRenderingHint.SingleBitPerPixelGridFit : TextRenderingHint.AntiAliasGridFit;

                double dataSpanUnits = _ys.Length * _samplePeriod;
                double columnSpanUnits = dims.XSpan / dims.DataWidth;
                double columnPointCount = (columnSpanUnits / dataSpanUnits) * _ys.Length;
                double offsetUnits = dims.XMin - xOffset;
                double offsetPoints = offsetUnits / _samplePeriod;
                int visibleIndex1 = (int)(offsetPoints);
                int visibleIndex2 = (int)(offsetPoints + columnPointCount * (dims.DataWidth + 1));
                int visiblePointCount = visibleIndex2 - visibleIndex1;
                double pointsPerPixelColumn = visiblePointCount / dims.DataWidth;
                double dataWidthPx2 = visibleIndex2 - visibleIndex1 + 2;

                double firstPointX = dims.GetPixelX(xOffset);
                double lastPointX = dims.GetPixelX(_samplePeriod * (_ys.Length - 1) + xOffset);
                double dataWidthPx = lastPointX - firstPointX;

                // set this now, and let the render function change it if it happens
                markersAreVisible = false;

                // use different rendering methods based on how dense the data is on screen
                if ((dataWidthPx <= 1) || (dataWidthPx2 <= 1))
                {
                    RenderSingleLine(dims, gfx, penHD);
                }
                else if (pointsPerPixelColumn > 1)
                {
                    if (densityLevelCount > 0 && pointsPerPixelColumn > densityLevelCount)
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

        public string ValidationErrorMessage { get; protected set; }

        public virtual bool IsValidData(bool deepValidation = false)
        {
            try
            {
                if (ys is null)
                    throw new ArgumentException("ys cannot be null");

                if (minRenderIndex < 0 || minRenderIndex > maxRenderIndex)
                    throw new ArgumentException("minRenderIndex must be between 0 and maxRenderIndex");

                if ((maxRenderIndex > ys.Length - 1) || maxRenderIndex < 0)
                    throw new ArgumentException("maxRenderIndex must be a valid index for ys[]");

                if (deepValidation)
                {
                    for (int i = 0; i < ys.Length; i++)
                    {
                        double y = Convert.ToDouble(ys[i]);
                        if (double.IsNaN(y) || double.IsInfinity(y))
                            throw new ArgumentException($"ys[{i}] is {ys[i]}");
                    }
                }

                if (MaxRenderIndexLowerYSPromise)
                    throw new ArgumentException("maxRenderIndex must be a valid index for ys[]");

                if (MaxRenderIndexHigherMinRenderIndexPromise)
                    throw new ArgumentException("minRenderIndex must be lower maxRenderIndex");

                if (FillColor1MustBeSetPromise)
                    throw new ArgumentException("A fill color needs to be specified if fill is used");

                if (FillColor2MustBeSetPromise)
                    throw new ArgumentException("Two fill colors needs to be specified if fill above and below is used");

                ValidationErrorMessage = null;
                return true;
            }
            catch (ArgumentException e)
            {
                ValidationErrorMessage = e.ToString();
                return false;
            }
        }
    }
}
