using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using ScottPlot.Config;

namespace ScottPlot
{
    public class PlottableSignalXY : PlottableSignalOld
    {
        public double[] xs;
        public PlottableSignalXY(double[] xs, double[] ys, Color color, double lineWidth, double markerSize, string label, int minRenderIndex, int maxRenderIndex, LineStyle lineStyle, bool useParallel)
            : base(ys, 1, 0, 0, color, lineWidth, markerSize, label, null, minRenderIndex, maxRenderIndex, lineStyle, useParallel)
        {
            if ((xs == null) || (ys == null))
                throw new ArgumentException("X and Y data cannot be null");

            if ((xs.Length == 0) || (ys.Length == 0))
                throw new ArgumentException("xs and ys must have at least one element");

            if (xs.Length != ys.Length)
                throw new ArgumentException("Xs and Ys must have same length");

            for (int i = 1; i < xs.Length; i++)
                if (xs[i] < xs[i - 1])
                    throw new ArgumentException("Xs must only contain ascending values");

            this.xs = xs;
        }

        public override AxisLimits2D GetLimits()
        {
            var limits = base.GetLimits();
            limits.SetX(xs[minRenderIndex], xs[maxRenderIndex]);
            return limits;
        }

        public IEnumerable<PointF> ProcessInterval(int x, int from, int length, Settings settings)
        {
            double start = settings.axes.x.min + settings.axes.x.span / settings.dataSize.Width * x;
            double end = settings.axes.x.min + settings.axes.x.span / settings.dataSize.Width * (x + 1);

            int startIndex = Array.BinarySearch(xs, from, length, start);
            if (startIndex < 0)
            {
                startIndex = ~startIndex;
            }

            int endIndex = Array.BinarySearch(xs, from, length, end);
            if (endIndex < 0)
            {
                endIndex = ~endIndex;
            }

            if (startIndex == endIndex)
            {
                yield break;
            }

            double min = ys[startIndex];
            double max = ys[startIndex];
            for (int j = startIndex; j < endIndex; j++)
            {
                if (ys[j] < min)
                    min = ys[j];
                if (ys[j] > max)
                    max = ys[j];
            }

            var pointsCount = endIndex - startIndex;

            yield return new PointF(x, (float)settings.GetPixelY(ys[startIndex]));
            if (pointsCount > 1)
            {
                yield return new PointF(x, (float)settings.GetPixelY(min));
                yield return new PointF(x, (float)settings.GetPixelY(max));
                yield return new PointF(x, (float)settings.GetPixelY(ys[endIndex - 1]));
            }
        }

        public override void Render(Settings settings)
        {
            brush = new SolidBrush(color);

            PointF[] PointBefore;
            PointF[] PointAfter;
            int searchFrom;
            int searchTo;

            // Calculate point before displayed points
            int pointBeforeIndex = Array.BinarySearch(xs, minRenderIndex, maxRenderIndex - minRenderIndex + 1, settings.axes.x.min); ;
            if (pointBeforeIndex < 0)
            {
                pointBeforeIndex = ~pointBeforeIndex;
            }

            if (pointBeforeIndex > minRenderIndex)
            {
                PointBefore = new PointF[] { settings.GetPixel(xs[pointBeforeIndex - 1], ys[pointBeforeIndex - 1]) };
                searchFrom = pointBeforeIndex;
            }
            else
            {
                PointBefore = new PointF[] { };
                searchFrom = minRenderIndex;
            }

            // Calculate point after displayed points
            int pointAfterIndex = Array.BinarySearch(xs, minRenderIndex, maxRenderIndex - minRenderIndex + 1, settings.axes.x.max);
            if (pointAfterIndex < 0)
            {
                pointAfterIndex = ~pointAfterIndex;
            }

            if (pointAfterIndex <= maxRenderIndex)
            {
                PointAfter = new PointF[] { settings.GetPixel(xs[pointAfterIndex], ys[pointAfterIndex]) };
                searchTo = pointAfterIndex;
            }
            else
            {
                PointAfter = new PointF[] { };
                searchTo = maxRenderIndex;
            }

            IEnumerable<PointF> VisiblePoints;
            if (useParallel)
            {
                VisiblePoints = Enumerable.Range(0, settings.dataSize.Width)
                                          .AsParallel()
                                          .AsOrdered()
                                          .Select(x => ProcessInterval(x, searchFrom, searchTo - searchFrom + 1, settings))
                                          .SelectMany(x => x);

            }
            else
            {
                VisiblePoints = Enumerable.Range(0, settings.dataSize.Width)
                                          .Select(x => ProcessInterval(x, searchFrom, searchTo - searchFrom + 1, settings))
                                          .SelectMany(x => x);
            }

            PointF[] PointsToDraw = PointBefore.Concat(VisiblePoints).Concat(PointAfter).ToArray();

            // Interpolate before displayed point to make it x = -1 (close to visible area)
            // this fix extreme zoom in bug
            if (PointBefore.Length > 0 && PointsToDraw.Length >= 2)
            {
                float x0 = -1;
                float y0 = PointsToDraw[1].Y + (PointsToDraw[0].Y - PointsToDraw[1].Y) * (x0 - PointsToDraw[1].X) / (PointsToDraw[0].X - PointsToDraw[1].X);
                PointsToDraw[0] = new PointF(x0, y0);
            }
            // Interpolate after displayed point to make it x = datasize.Width(close to visible area)
            // this fix extreme zoom in bug
            if (PointAfter.Length > 0 && PointsToDraw.Length >= 2)
            {
                PointF lastPoint = PointsToDraw[PointsToDraw.Length - 2];
                PointF afterPoint = PointsToDraw[PointsToDraw.Length - 1];

                float x1 = settings.dataSize.Width;
                float y1 = lastPoint.Y + (afterPoint.Y - lastPoint.Y) * (x1 - lastPoint.X) / (afterPoint.X - lastPoint.X);
                PointsToDraw[PointsToDraw.Length - 1] = new PointF(x1, y1);
            }

            // Draw lines
            if (PointsToDraw.Length > 1)
                settings.gfxData.DrawLines(penHD, PointsToDraw.ToArray());

            // draw markers
            if (PointsToDraw.Length > 1)
            {
                float dataSpanXPx = PointsToDraw[PointsToDraw.Length - 1].X - PointsToDraw[0].X;
                float markerPxRadius = .3f * dataSpanXPx / PointsToDraw.Length;
                markerPxRadius = Math.Min(markerPxRadius, markerSize / 2);
                if (markerPxRadius > .3)
                {
                    // skip not visible before and after points
                    var PointsWithMarkers = PointsToDraw
                                            .Skip(PointBefore.Length)
                                            .Take(PointsToDraw.Length - PointBefore.Length - PointAfter.Length);
                    foreach (PointF pt in PointsWithMarkers)
                    {
                        // adjust marker offset to improve rendering on Linux and MacOS
                        float markerOffsetX = (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) ? 0 : 1;

                        settings.gfxData.FillEllipse(brush,
                              x: pt.X - markerPxRadius + markerOffsetX,
                              y: pt.Y - markerPxRadius,
                              width: markerPxRadius * 2,
                              height: markerPxRadius * 2);
                    }
                }
            }
        }
    }
}
