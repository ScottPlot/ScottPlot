using ScottPlot;
using SkiaSharp;
using SkiaSharp.Views.Desktop;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlotSkia
{
    public class PlottableSignalConstSkia<T> : PlottableSignalConst<T> where T : struct, IComparable
    {
        SKPaint paint;
        public PlottableSignalConstSkia(T[] ys, double sampleRate, double xOffset, double yOffset, Color color, double lineWidth, double markerSize, string label, bool useParallel)
            : base(ys, sampleRate, xOffset, yOffset, color, lineWidth, markerSize, label, useParallel)
        {
            paint = new SKPaint()
            {
                Color = color.ToSKColor(),
                IsAntialias = true, // Always AA
                StrokeJoin = SKStrokeJoin.Round,
                StrokeCap = SKStrokeCap.Round,
                IsStroke = true,
                StrokeWidth = (float)lineWidth,
            };
        }

        public override void Render(Settings settings)
        {
            double dataSpanUnits = ys.Length * samplePeriod;
            double columnSpanUnits = settings.xAxisSpan / settings.dataSize.Width;
            double columnPointCount = (columnSpanUnits / dataSpanUnits) * ys.Length;
            double offsetUnits = settings.axis[0] - xOffset;
            double offsetPoints = offsetUnits / samplePeriod;
            int visibleIndex1 = (int)(offsetPoints);
            int visibleIndex2 = (int)(offsetPoints + columnPointCount * (settings.dataSize.Width + 1));
            int visiblePointCount = visibleIndex2 - visibleIndex1;
            double pointsPerPixelColumn = visiblePointCount / settings.dataSize.Width;

            PointF firstPoint = settings.GetPixel(xOffset, Convert.ToDouble(ys[0]) + yOffset);
            PointF lastPoint = settings.GetPixel(samplePeriod * (ys.Length - 1) + xOffset, Convert.ToDouble(ys[ys.Length - 1]) + yOffset);
            double dataWidthPx = lastPoint.X - firstPoint.X;

            paint.IsAntialias = settings.antiAliasData;

            // use different rendering methods based on how dense the data is on screen
            if (dataWidthPx <= 1)
            {
                RenderSingleLine(settings);
            }
            else if (pointsPerPixelColumn > 1)
            {
                if (useParallel)
                    RenderHighDensityParallel(settings, offsetPoints, columnPointCount);
                else
                    RenderHighDensity(settings, offsetPoints, columnPointCount);
            }
            else
            {
                RenderLowDensity(settings, visibleIndex1, visibleIndex2);
            }
        }

        private void RenderSingleLine(Settings settings)
        {
            // this function is for when the graph is zoomed so far out its entire display is a single vertical pixel column
            double yMin, yMax;
            MinMaxRangeQuery(0, ys.Length - 1, out yMin, out yMax);
            PointF point1 = settings.GetPixel(xOffset, yMin + yOffset);
            PointF point2 = settings.GetPixel(xOffset, yMax + yOffset);
            //settings.gfxData.DrawLine(pen, point1, point2);
        }

        private void RenderLowDensity(Settings settings, int visibleIndex1, int visibleIndex2)
        {
            // this function is for when the graph is zoomed in so individual data points can be seen

            List<PointF> linePoints = new List<PointF>(visibleIndex2 - visibleIndex1 + 2);
            if (visibleIndex2 > ys.Length - 2)
                visibleIndex2 = ys.Length - 2;
            if (visibleIndex1 < 0)
                visibleIndex1 = 0;
            for (int i = visibleIndex1; i <= visibleIndex2 + 1; i++)
                linePoints.Add(settings.GetPixel(samplePeriod * i + xOffset, Convert.ToDouble(ys[i]) + yOffset));

            if (linePoints.Count > 1)
            {
                var settingsSkia = settings as SettingsSkia;
                settingsSkia.canvas.DrawPoints(SKPointMode.Polygon, linePoints.Select(x => x.ToSKPoint()).ToArray(), paint);

                paint.Style = SKPaintStyle.Fill;
                foreach (PointF point in linePoints)
                    settingsSkia.canvas.DrawOval(SKRect.Create(point.X - markerSize / 2, point.Y - markerSize / 2, markerSize, markerSize), paint);
            }
        }

        private void RenderHighDensityParallel(Settings settings, double offsetPoints, double columnPointCount)
        {
            int xPxStart = (int)Math.Ceiling((-1 - offsetPoints) / columnPointCount - 1);
            int xPxEnd = (int)Math.Ceiling((ys.Length - offsetPoints) / columnPointCount);
            xPxStart = Math.Max(0, xPxStart);
            xPxEnd = Math.Min(settings.dataSize.Width, xPxEnd);
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
                double lowestValue, highestValue;
                MinMaxRangeQuery(index1, index2, out lowestValue, out highestValue);
                float yPxHigh = settings.GetPixel(0, lowestValue + yOffset).Y;
                float yPxLow = settings.GetPixel(0, highestValue + yOffset).Y;

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
            (settings as SettingsSkia).canvas.DrawPoints(SkiaSharp.SKPointMode.Polygon, linePoints.Select(x => x.ToSKPoint()).ToArray(), paint);
        }

        private void RenderHighDensity(Settings settings, double offsetPoints, double columnPointCount)
        {
            // this function is for when the graph is zoomed out so each pixel column represents the vertical span of multiple data points
            int xPxStart = (int)Math.Ceiling((-1 - offsetPoints) / columnPointCount - 1);
            int xPxEnd = (int)Math.Ceiling((ys.Length - offsetPoints) / columnPointCount);
            xPxStart = Math.Max(0, xPxStart);
            xPxEnd = Math.Min(settings.dataSize.Width, xPxEnd);
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
                if (index2 > ys.Length - 1)
                    index2 = ys.Length - 1;

                // get the min and max value for this column                
                double lowestValue, highestValue;
                MinMaxRangeQuery(index1, index2, out lowestValue, out highestValue);
                float yPxHigh = settings.GetPixel(0, lowestValue + yOffset).Y;
                float yPxLow = settings.GetPixel(0, highestValue + yOffset).Y;

                // adjust order of points to enhance anti-aliasing
                if ((linePoints.Count < 2) || (yPxLow < linePoints[linePoints.Count - 1].Y))
                {
                    linePoints.Add(new PointF(0.5f + xPx, yPxLow));
                    linePoints.Add(new PointF(0.5f + xPx, yPxHigh));
                }
                else
                {
                    linePoints.Add(new PointF(0.5f + xPx, yPxHigh));
                    linePoints.Add(new PointF(0.5f + xPx, yPxLow));
                }
            }
            if (linePoints.Count > 0)
                (settings as SettingsSkia).canvas.DrawPoints(SkiaSharp.SKPointMode.Polygon, linePoints.Select(x => x.ToSKPoint()).ToArray(), paint);
        }
    }
}
