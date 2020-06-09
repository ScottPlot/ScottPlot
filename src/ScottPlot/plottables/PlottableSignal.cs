using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using ScottPlot.Config;
using ScottPlot.Drawing;

namespace ScottPlot
{
    public class PlottableSignal : Plottable, IExportable
    {
        // Any changes must be sync with PlottableSignalConst
        public double[] ys;
        public double sampleRate;
        public double samplePeriod;
        public float markerSize;
        public double xOffset;
        public double yOffset;
        public double lineWidth;
        public Pen penHD;
        public Pen penLD;
        public Brush brush;
        public int maxRenderIndex;
        private Pen[] penByDensity;
        private int densityLevelCount = 0;
        public Color color;
        public string label;
        public LineStyle lineStyle;
        public bool useParallel = true;

        public PlottableSignal(double[] ys, double sampleRate, double xOffset, double yOffset, Color color, double lineWidth, double markerSize, string label, Color[] colorByDensity, int maxRenderIndex, LineStyle lineStyle, bool useParallel)
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
        }

        public override string ToString()
        {
            string label = string.IsNullOrWhiteSpace(this.label) ? "" : $" ({this.label})";
            return $"PlottableSignal{label} with {GetPointCount()} points";
        }

        public override Config.AxisLimits2D GetLimits()
        {
            double yMin = ys[0];
            double yMax = ys[0];
            for (int i = 0; i < maxRenderIndex; i++)
            {
                // TODO: ignore NaN
                if (ys[i] < yMin) yMin = ys[i];
                else if (ys[i] > yMax) yMax = ys[i];
            }

            double[] limits = new double[4];
            limits[0] = 0 + xOffset;
            limits[1] = samplePeriod * maxRenderIndex + xOffset;
            limits[2] = yMin + yOffset;
            limits[3] = yMax + yOffset;

            // TODO: use features of 2d axis
            return new Config.AxisLimits2D(limits);
        }

        private void RenderSingleLine(Settings settings)
        {
            // this function is for when the graph is zoomed so far out its entire display is a single vertical pixel column                     
            PointF point1 = settings.GetPixel(xOffset, ys.Min() + yOffset);
            PointF point2 = settings.GetPixel(xOffset, ys.Max() + yOffset);
            settings.gfxData.DrawLine(penHD, point1, point2);
        }

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
            for (int i = visibleIndex1; i <= visibleIndex2 + 1; i++)
                linePoints.Add(settings.GetPixel(samplePeriod * i + xOffset, ys[i] + yOffset));

            if (linePoints.Count > 1)
            {
                if (penLD.Width > 0)
                    settings.gfxData.DrawLines(penLD, linePoints.ToArray());

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
            if (index2 > ys.Length - 1)
                index2 = ys.Length - 1;

            if (index2 > maxRenderIndex)
                index2 = maxRenderIndex;

            // get the min and max value for this column                
            double lowestValue = ys[index1];
            double highestValue = ys[index1];
            for (int i = index1; i < index2; i++)
            {
                if (ys[i] < lowestValue)
                    lowestValue = ys[i];
                if (ys[i] > highestValue)
                    highestValue = ys[i];
            }
            float yPxHigh = (float)settings.GetPixelY(lowestValue + yOffset);
            float yPxLow = (float)settings.GetPixelY(highestValue + yOffset);
            return new IntervalMinMax(xPx, yPxLow, yPxHigh);
        }

        private void RenderHighDensity(Settings settings, double offsetPoints, double columnPointCount)
        {
            int xPxStart = (int)Math.Ceiling((-1 - offsetPoints) / columnPointCount - 1);
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

                    var levelsValues = new ArraySegment<double>(ys, index1, index2 - index1)
                        .OrderBy(x => x)
                        .Where((y, i) => indexes.Contains(i)).ToArray();
                    return (xPx, levelsValues);
                })
                .ToArray();

            List<PointF[]> linePointsLevels = levelValues
                .Select(x => x.levelsValues
                                .Select(y => new PointF(x.xPx, (float)settings.GetPixelY(y + yOffset)))
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

        private bool markersAreVisible = false;
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

            PointF firstPoint = settings.GetPixel(xOffset, ys[0] + yOffset);
            PointF lastPoint = settings.GetPixel(samplePeriod * (ys.Length - 1) + xOffset, ys[ys.Length - 1] + yOffset);
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

        public void SaveCSV(string filePath, string delimiter = ", ", string separator = "\n")
        {
            System.IO.File.WriteAllText(filePath, GetCSV(delimiter, separator));
        }

        public string GetCSV(string delimiter = ", ", string separator = "\n")
        {
            StringBuilder csv = new StringBuilder();
            for (int i = 0; i < ys.Length; i++)
                csv.AppendFormat("{0}{1}{2}{3}", xOffset + i * samplePeriod, delimiter, ys[i] + yOffset, separator);
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
                lineStyle = LineStyle.Solid,
                lineWidth = lineWidth,
                markerShape = (markersAreVisible) ? MarkerShape.filledCircle : MarkerShape.none,
                markerSize = (markersAreVisible) ? markerSize : 0
            };
            return new LegendItem[] { singleLegendItem };
        }
    }
}
