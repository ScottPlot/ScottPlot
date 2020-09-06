﻿using ScottPlot.Config;
using ScottPlot.Drawing;
using ScottPlot.MinMaxSearchStrategies;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace ScottPlot
{
    public class PlottableSignalBase<T> : Plottable, IExportable where T : struct, IComparable
    {
        protected IMinMaxSearchStrategy<T> minmaxSearchStrategy;
        // Any changes must be sync with PlottableSignal
        public T[] ys;
        public double sampleRate;
        public double samplePeriod;
        public float markerSize;
        public double xOffset;
        public double yOffset;
        public double lineWidth;
        public Pen penHD;
        public Pen penLD;
        public Brush brush;
        public int minRenderIndex;
        public int maxRenderIndex;
        private Pen[] penByDensity;
        private int densityLevelCount = 0;
        public string label;
        public Color color;
        public LineStyle lineStyle;
        public bool useParallel = true;

        public bool TreesReady => true;
        public PlottableSignalBase(T[] ys, double sampleRate, double xOffset, double yOffset, Color color,
            double lineWidth, double markerSize, string label, Color[] colorByDensity,
            int minRenderIndex, int maxRenderIndex, LineStyle lineStyle, bool useParallel,
            IMinMaxSearchStrategy<T> minMaxSearchStrategy = null)
        {
            if (ys == null)
                throw new Exception("Y data cannot be null");

            this.ys = ys;
            this.sampleRate = sampleRate;
            this.samplePeriod = 1.0 / sampleRate;
            this.markerSize = (float)markerSize;
            this.xOffset = xOffset;
            this.label = label;
            this.color = color;
            this.lineWidth = lineWidth;
            this.yOffset = yOffset;
            if (minRenderIndex < 0 || minRenderIndex > maxRenderIndex)
                throw new ArgumentException("minRenderIndex must be between 0 and maxRenderIndex");
            this.minRenderIndex = minRenderIndex;
            if ((maxRenderIndex > ys.Length - 1) || maxRenderIndex < 0)
                throw new ArgumentException("maxRenderIndex must be a valid index for ys[]");
            this.maxRenderIndex = maxRenderIndex;
            this.lineStyle = lineStyle;
            this.useParallel = useParallel;
            brush = new SolidBrush(color);
            penLD = GDI.Pen(color, (float)lineWidth, lineStyle, true);
            penHD = GDI.Pen(color, (float)lineWidth, LineStyle.Solid, true);

            if (colorByDensity != null)
            {
                // turn the ramp into a pen triangle
                densityLevelCount = colorByDensity.Length * 2 - 1;
                penByDensity = new Pen[densityLevelCount];
                for (int i = 0; i < colorByDensity.Length; i++)
                {
                    penByDensity[i] = new Pen(colorByDensity[i]);
                    penByDensity[densityLevelCount - 1 - i] = new Pen(colorByDensity[i]);
                }
            }
            if (minMaxSearchStrategy == null)
                this.minmaxSearchStrategy = new SegmentedTreeMinMaxSearchStrategy<T>();
            else
                this.minmaxSearchStrategy = minMaxSearchStrategy;
            minmaxSearchStrategy.SourceArray = ys;
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

        public override Config.AxisLimits2D GetLimits()
        {
            double[] limits = new double[4];
            limits[0] = minRenderIndex + xOffset;
            limits[1] = samplePeriod * maxRenderIndex + xOffset;
            minmaxSearchStrategy.MinMaxRangeQuery(minRenderIndex, maxRenderIndex, out limits[2], out limits[3]);
            limits[2] += yOffset;
            limits[3] += yOffset;

            // TODO: use features of 2d axis
            return new Config.AxisLimits2D(limits);
        }

        private void RenderSingleLine(Settings settings)
        {
            // this function is for when the graph is zoomed so far out its entire display is a single vertical pixel column
            double yMin, yMax;
            minmaxSearchStrategy.MinMaxRangeQuery(minRenderIndex, maxRenderIndex, out yMin, out yMax);
            PointF point1 = settings.GetPixel(xOffset, yMin + yOffset);
            PointF point2 = settings.GetPixel(xOffset, yMax + yOffset);
            settings.gfxData.DrawLine(penHD, point1, point2);
        }

        private bool markersAreVisible = false;
        private void RenderLowDensity(Settings settings, int visibleIndex1, int visibleIndex2)
        {
            // this function is for when the graph is zoomed in so individual data points can be seen

            List<PointF> linePoints = new List<PointF>(visibleIndex2 - visibleIndex1 + 2);
            if (visibleIndex2 > ys.Length - 2)
                visibleIndex2 = ys.Length - 2;
            if (visibleIndex2 > maxRenderIndex)
                visibleIndex2 = maxRenderIndex - 1;
            if (visibleIndex1 < 0)
                visibleIndex1 = 0;
            if (visibleIndex1 < minRenderIndex)
                visibleIndex1 = minRenderIndex;

            for (int i = visibleIndex1; i <= visibleIndex2 + 1; i++)
                linePoints.Add(settings.GetPixel(samplePeriod * i + xOffset, minmaxSearchStrategy.SourceElement(i) + yOffset));

            if (linePoints.Count > 1)
            {
                if (penLD.Width > 0)
                    settings.gfxData.DrawLines(penHD, linePoints.ToArray());

                if (markerSize > 0)
                {
                    // make markers transition away smoothly by making them smaller as the user zooms out
                    float pixelsBetweenPoints = (float)(samplePeriod * settings.xAxisScale);
                    float zoomTransitionScale = Math.Min(1, pixelsBetweenPoints / 10);
                    float markerPxDiameter = markerSize * zoomTransitionScale;
                    float markerPxRadius = markerPxDiameter / 2;
                    if (markerPxRadius > .25)
                    {
                        markersAreVisible = true;

                        // adjust marker offset to improve rendering on Linux and MacOS
                        float markerOffsetX = (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) ? 0 : 1;

                        foreach (PointF point in linePoints)
                            settings.gfxData.FillEllipse(brush: brush,
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

        private IntervalMinMax CalcInterval(int xPx, double offsetPoints, double columnPointCount, Settings settings)
        {
            int index1 = (int)(offsetPoints + columnPointCount * xPx);
            int index2 = (int)(offsetPoints + columnPointCount * (xPx + 1));

            if (index1 < 0)
                index1 = 0;
            if (index1 < minRenderIndex)
                index1 = minRenderIndex;

            if (index2 > ys.Length - 1)
                index2 = ys.Length - 1;
            if (index2 > maxRenderIndex)
                index2 = maxRenderIndex;

            // get the min and max value for this column                
            minmaxSearchStrategy.MinMaxRangeQuery(index1, index2, out double lowestValue, out double highestValue);
            float yPxHigh = (float)settings.GetPixelY(lowestValue + yOffset);
            float yPxLow = (float)settings.GetPixelY(highestValue + yOffset);
            return new IntervalMinMax(xPx, yPxLow, yPxHigh);
        }

        private void RenderHighDensity(Settings settings, double offsetPoints, double columnPointCount)
        {
            int xPxStart = (int)Math.Ceiling((-1 - offsetPoints + minRenderIndex) / columnPointCount - 1);
            int xPxEnd = (int)Math.Ceiling((maxRenderIndex - offsetPoints) / columnPointCount);
            xPxStart = Math.Max(0, xPxStart);
            xPxEnd = Math.Min(settings.dataSize.Width, xPxEnd);
            if (xPxStart >= xPxEnd)
                return;

            var columns = Enumerable.Range(xPxStart, xPxEnd - xPxStart);

            IEnumerable<IntervalMinMax> intervals;
            if (useParallel)
            {
                intervals = columns
                    .AsParallel()
                    .AsOrdered()
                    .Select(xPx => CalcInterval(xPx, offsetPoints, columnPointCount, settings))
                    .AsSequential();
            }
            else
            {
                intervals = columns
                    .Select(xPx => CalcInterval(xPx, offsetPoints, columnPointCount, settings));
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
                settings.gfxData.DrawLines(penHD, linePoints);
        }

        private void RenderHighDensityDistributionParallel(Settings settings, double offsetPoints, double columnPointCount)
        {
            int xPxStart = (int)Math.Ceiling((-1 - offsetPoints) / columnPointCount - 1);
            int xPxEnd = (int)Math.Ceiling((ys.Length - offsetPoints) / columnPointCount);
            xPxStart = Math.Max(0, xPxStart);
            xPxEnd = Math.Min(settings.dataSize.Width, xPxEnd);
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
                    if (index1 > ys.Length - 1)
                        index1 = ys.Length - 1;
                    if (index2 > ys.Length - 1)
                        index2 = ys.Length - 1;

                    var indexes = Enumerable.Range(0, densityLevelCount + 1).Select(x => x * (index2 - index1 - 1) / (densityLevelCount));

                    var levelsValues = new ArraySegment<T>(ys, index1, index2 - index1)
                        .OrderBy(x => x)
                        .Where((y, i) => indexes.Contains(i)).ToArray();
                    return (xPx, levelsValues);
                })
                .ToArray();

            List<PointF[]> linePointsLevels = levelValues
                .Select(x => x.levelsValues
                                .Select(y => new PointF(x.xPx, (float)settings.GetPixelY(Convert.ToDouble(y) + yOffset)))
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
                settings.gfxData.DrawLines(penByDensity[i], linePoints.ToArray());
            }
        }

        public override void Render(Settings settings)
        {
            brush = new SolidBrush(color);

            double dataSpanUnits = ys.Length * samplePeriod;
            double columnSpanUnits = settings.axes.x.span / settings.dataSize.Width;
            double columnPointCount = (columnSpanUnits / dataSpanUnits) * ys.Length;
            double offsetUnits = settings.axes.x.min - xOffset;
            double offsetPoints = offsetUnits / samplePeriod;
            int visibleIndex1 = (int)(offsetPoints);
            int visibleIndex2 = (int)(offsetPoints + columnPointCount * (settings.dataSize.Width + 1));
            int visiblePointCount = visibleIndex2 - visibleIndex1;
            double pointsPerPixelColumn = visiblePointCount / settings.dataSize.Width;
            double dataWidthPx2 = visibleIndex2 - visibleIndex1 + 2;

            PointF firstPoint = settings.GetPixel(xOffset, minmaxSearchStrategy.SourceElement(0) + yOffset); ;
            PointF lastPoint = settings.GetPixel(samplePeriod * (ys.Length - 1) + xOffset, minmaxSearchStrategy.SourceElement(ys.Length - 1) + yOffset);
            double dataWidthPx = lastPoint.X - firstPoint.X;

            // set this now, and let the render function change it if it happens
            markersAreVisible = false;

            // use different rendering methods based on how dense the data is on screen
            if ((dataWidthPx <= 1) || (dataWidthPx2 <= 1))
            {
                RenderSingleLine(settings);
            }
            else if (pointsPerPixelColumn > 1)
            {
                if (densityLevelCount > 0 && pointsPerPixelColumn > densityLevelCount)
                    RenderHighDensityDistributionParallel(settings, offsetPoints, columnPointCount);
                else
                    RenderHighDensity(settings, offsetPoints, columnPointCount);
            }
            else
            {
                RenderLowDensity(settings, visibleIndex1, visibleIndex2);
            }
        }

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
            for (int i = 0; i < ys.Length; i++)
                csv.AppendFormat(CultureInfo.InvariantCulture, "{0}{1}{2}{3}", xOffset + i * samplePeriod, delimiter, minmaxSearchStrategy.SourceElement(i) + yOffset, separator);
            return csv.ToString();
        }

        public override int GetPointCount()
        {
            return ys.Length;
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
    }
}
