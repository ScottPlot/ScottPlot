using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Linq.Expressions;

namespace ScottPlot.Plottables
{
    public class Signal : IPlottable, IExportable
    {
        // interface stuff
        public bool visible { get; set; } = true;
        public int pointCount { get { return ys.Length; } }
        public string label { get; set; }
        public Color color { get; set; }
        public MarkerShape markerShape { get; set; }
        public LineStyle lineStyle { get; set; }

        // properties
        bool useParallel;

        // Any changes must be sync with PlottableSignalConst
        public double[] ys;
        public double sampleRate;
        public double samplePeriod;
        public float markerSize;
        public double xOffset;
        public double yOffset;
        public double lineWidth;
        public Pen pen;
        public Brush brush;
        public int maxRenderIndex;
        private Pen[] penByDensity;
        private int densityLevelCount = 0;

        public Signal(double[] ys, double sampleRate, double xOffset, double yOffset, Color color, double lineWidth, double markerSize, string label, bool useParallel, Color[] colorByDensity, int maxRenderIndex)
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
            this.useParallel = useParallel;
            if ((maxRenderIndex > ys.Length - 1) || maxRenderIndex < 0)
                throw new ArgumentException("maxRenderIndex must be a valid index for ys[]");
            this.maxRenderIndex = maxRenderIndex;
            brush = new SolidBrush(color);
            pen = new Pen(color, (float)lineWidth)
            {
                // this prevents sharp corners
                StartCap = System.Drawing.Drawing2D.LineCap.Round,
                EndCap = System.Drawing.Drawing2D.LineCap.Round,
                LineJoin = System.Drawing.Drawing2D.LineJoin.Round
            };

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
            return $"PlottableSignal with {pointCount} points";
        }

        public Config.AxisLimits2D GetLimits()
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

        private void RenderSingleLine(DataArea dataArea)
        {
            // this function is for when the graph is zoomed so far out its entire display is a single vertical pixel column                     
            PointF point1 = dataArea.GetPixel(xOffset, ys.Min() + yOffset);
            PointF point2 = dataArea.GetPixel(xOffset, ys.Max() + yOffset);
            dataArea.gfxData.DrawLine(pen, point1, point2);
        }

        private void RenderLowDensity(DataArea dataArea, int visibleIndex1, int visibleIndex2)
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
                linePoints.Add(dataArea.GetPixel(samplePeriod * i + xOffset, ys[i] + yOffset));

            if (linePoints.Count > 1)
            {
                dataArea.gfxData.DrawLines(pen, linePoints.ToArray());
                foreach (PointF point in linePoints)
                    dataArea.gfxData.FillEllipse(brush, point.X - markerSize / 2, point.Y - markerSize / 2, markerSize, markerSize);
            }
        }

        private void RenderHighDensityParallel(DataArea dataArea, double offsetPoints, double columnPointCount)
        {
            int xPxStart = (int)Math.Ceiling((-1 - offsetPoints) / columnPointCount - 1);
            int xPxEnd = (int)Math.Ceiling((ys.Length - offsetPoints) / columnPointCount);
            xPxStart = Math.Max(0, xPxStart);
            xPxEnd = (int)Math.Min(dataArea.widthPx, xPxEnd);
            if (xPxStart >= xPxEnd)
                return;
            PointF[] linePoints = new PointF[(xPxEnd - xPxStart) * 2];
            Parallel.For(xPxStart, xPxEnd, xPx =>
            {
                // determine data indexes for this pixel column
                int index1 = (int)(offsetPoints + columnPointCount * xPx);
                int index2 = (int)(offsetPoints + columnPointCount * (xPx + 1));

                if (index1 < 0)
                    index1 = 0;
                if (index2 > ys.Length - 1)
                    index2 = ys.Length - 1;

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
                float yPxHigh = dataArea.GetPixel(0, lowestValue + yOffset).Y;
                float yPxLow = dataArea.GetPixel(0, highestValue + yOffset).Y;

                linePoints[(xPx - xPxStart) * 2] = new PointF(xPx, yPxLow);
                linePoints[(xPx - xPxStart) * 2 + 1] = new PointF(xPx, yPxHigh);
            });

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

            dataArea.gfxData.DrawLines(pen, linePoints);
        }

        private void RenderHighDensityDistribution(DataArea dataArea, double offsetPoints, double columnPointCount)
        {
            int xPxStart = (int)Math.Ceiling((-1 - offsetPoints) / columnPointCount - 1);
            int xPxEnd = (int)Math.Ceiling((ys.Length - offsetPoints) / columnPointCount);
            xPxStart = Math.Max(0, xPxStart);
            xPxEnd = (int)Math.Min(dataArea.widthPx, xPxEnd);
            if (xPxStart >= xPxEnd)
                return;
            List<PointF> linePoints = new List<PointF>((xPxEnd - xPxStart) * 2 + 1);
            List<PointF[]> linePointsLevels = new List<PointF[]>((xPxEnd - xPxStart) + 1);
            for (int xPx = xPxStart; xPx < xPxEnd; xPx++)
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
                    .Where((y, i) => indexes.Contains(i));

                var Points = levelsValues
                    .Select(x => dataArea.GetPixel(0, x + yOffset).Y)
                    .Select(y => new PointF(xPx, y))
                    .ToArray();

                linePointsLevels.Add(Points);
            }
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
                dataArea.gfxData.DrawLines(penByDensity[i], linePoints.ToArray());
            }
        }

        private void RenderHighDensityDistributionParallel(DataArea dataArea, double offsetPoints, double columnPointCount)
        {
            int xPxStart = (int)Math.Ceiling((-1 - offsetPoints) / columnPointCount - 1);
            int xPxEnd = (int)Math.Ceiling((ys.Length - offsetPoints) / columnPointCount);
            xPxStart = Math.Max(0, xPxStart);
            xPxEnd = (int)Math.Min(dataArea.widthPx, xPxEnd);
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
                                .Select(y => new PointF(x.xPx, dataArea.GetPixel(0, y + yOffset).Y))
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
                dataArea.gfxData.DrawLines(penByDensity[i], linePoints.ToArray());
            }
        }

        private void RenderHighDensity(DataArea dataArea, double offsetPoints, double columnPointCount)
        {
            // this function is for when the graph is zoomed out so each pixel column represents the vertical span of multiple data points

            int xPxStart = (int)Math.Ceiling((-1 - offsetPoints) / columnPointCount - 1);
            int xPxEnd = (int)Math.Ceiling((ys.Length - offsetPoints) / columnPointCount);
            xPxStart = Math.Max(0, xPxStart);
            xPxEnd = (int)Math.Min(dataArea.widthPx, xPxEnd);
            if (xPxStart >= xPxEnd)
                return;
            List<PointF> linePoints = new List<PointF>((xPxEnd - xPxStart) * 2 + 1);
            for (int xPx = xPxStart; xPx < xPxEnd; xPx++)
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

                // skip data above maxRenderIndex 
                if (index1 > maxRenderIndex)
                    continue;
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
                float yPxHigh = dataArea.GetPixel(0, lowestValue + yOffset).Y;
                float yPxLow = dataArea.GetPixel(0, highestValue + yOffset).Y;

                // adjust order of points to enhance anti-aliasing
                if ((linePoints.Count < 2) || (yPxLow < linePoints[linePoints.Count - 1].Y))
                {
                    linePoints.Add(new PointF(xPx, yPxLow));
                    linePoints.Add(new PointF(xPx, yPxHigh));
                }
                else
                {
                    linePoints.Add(new PointF(xPx, yPxHigh));
                    linePoints.Add(new PointF(xPx, yPxLow));
                }
            }

            if (linePoints.Count > 0)
                dataArea.gfxData.DrawLines(pen, linePoints.ToArray());
        }

        public void Render(DataArea dataArea)
        {
            //var settings = renderContext.settings;

            pen.Color = color;
            pen.Width = (float)lineWidth;
            brush = new SolidBrush(color);

            double dataSpanUnits = ys.Length * samplePeriod;
            double columnSpanUnits = dataArea.axisLimits.xSpan / dataArea.widthPx;
            double columnPointCount = (columnSpanUnits / dataSpanUnits) * ys.Length;
            double offsetUnits = dataArea.axisLimits.x1 - xOffset;
            double offsetPoints = offsetUnits / samplePeriod;
            int visibleIndex1 = (int)(offsetPoints);
            int visibleIndex2 = (int)(offsetPoints + columnPointCount * (dataArea.widthPx + 1));
            int visiblePointCount = visibleIndex2 - visibleIndex1;
            double pointsPerPixelColumn = visiblePointCount / dataArea.widthPx;
            double dataWidthPx2 = visibleIndex2 - visibleIndex1 + 2;

            PointF firstPoint = dataArea.GetPixel(xOffset, ys[0] + yOffset);
            PointF lastPoint = dataArea.GetPixel(samplePeriod * (ys.Length - 1) + xOffset, ys[ys.Length - 1] + yOffset);
            double dataWidthPx = lastPoint.X - firstPoint.X;

            // use different rendering methods based on how dense the data is on screen
            if ((dataWidthPx <= 1) || (dataWidthPx2 <= 1))
            {
                RenderSingleLine(dataArea);
            }
            else if (pointsPerPixelColumn > 1)
            {
                if (densityLevelCount > 0 && pointsPerPixelColumn > densityLevelCount)
                {
                    if (useParallel)
                        RenderHighDensityDistributionParallel(dataArea, offsetPoints, columnPointCount);
                    else
                        RenderHighDensityDistribution(dataArea, offsetPoints, columnPointCount);
                }
                else
                {
                    if (useParallel)
                        RenderHighDensityParallel(dataArea, offsetPoints, columnPointCount);
                    else
                        RenderHighDensity(dataArea, offsetPoints, columnPointCount);
                }
            }
            else
            {
                RenderLowDensity(dataArea, visibleIndex1, visibleIndex2);
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
    }
}
