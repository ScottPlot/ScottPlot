using ScottPlot.Config;
using ScottPlot.Diagnostic.Attributes;
using ScottPlot.Drawing;
using ScottPlot.MinMaxSearchStrategies;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;

namespace ScottPlot
{

    public class PlottableSignalXYGeneric<TX, TY> : PlottableSignalBase<TY> where TX : struct, IComparable where TY : struct, IComparable
    {
        [FiniteNumbers, EqualLength, Accending]
        public TX[] xs;
        public PlottableSignalXYGeneric(TX[] xs, TY[] ys, Color color, double lineWidth, double markerSize, string label, int minRenderIndex, int maxRenderIndex, LineStyle lineStyle, bool useParallel, IMinMaxSearchStrategy<TY> minMaxSearchStrategy = null)
            : base(ys, 1, 0, 0, color, lineWidth, markerSize, label, null, minRenderIndex, maxRenderIndex, lineStyle, useParallel, minMaxSearchStrategy)
        {
            if ((xs == null) || (ys == null))
                throw new ArgumentException("X and Y data cannot be null");

            if ((xs.Length == 0) || (ys.Length == 0))
                throw new ArgumentException("xs and ys must have at least one element");

            if (xs.Length != ys.Length)
                throw new ArgumentException("Xs and Ys must have same length");

            for (int i = 1; i < xs.Length; i++)
                if (xs[i].CompareTo(xs[i - 1]) < 0)
                    throw new ArgumentException("Xs must only contain ascending values");

            if (minMaxSearchStrategy == null)
                this.minmaxSearchStrategy = new SegmentedTreeMinMaxSearchStrategy<TY>();
            else
                this.minmaxSearchStrategy = minMaxSearchStrategy;
            minmaxSearchStrategy.SourceArray = ys;

            this.xs = xs;
        }

        public override AxisLimits2D GetLimits()
        {
            var limits = base.GetLimits();
            limits.SetX(Convert.ToDouble(xs[MinRenderIndex]), Convert.ToDouble(xs[maxRenderIndex]));
            return limits;
        }

        public IEnumerable<PointF> ProcessInterval(int x, int from, int length, Settings settings)
        {
            TX start = (TX)Convert.ChangeType(settings.axes.x.min + settings.axes.x.span / settings.dataSize.Width * x, typeof(TX));
            TX end = (TX)Convert.ChangeType(settings.axes.x.min + settings.axes.x.span / settings.dataSize.Width * (x + 1), typeof(TX));

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

            minmaxSearchStrategy.MinMaxRangeQuery(startIndex, endIndex - 1, out double min, out double max);

            var pointsCount = endIndex - startIndex;

            yield return new PointF(x, (float)settings.GetPixelY(minmaxSearchStrategy.SourceElement(startIndex)));
            if (pointsCount > 1)
            {
                yield return new PointF(x, (float)settings.GetPixelY(min));
                yield return new PointF(x, (float)settings.GetPixelY(max));
                yield return new PointF(x, (float)settings.GetPixelY(minmaxSearchStrategy.SourceElement(endIndex - 1)));
            }
        }

        public override void Render(Settings settings)
        {
            using (var brush = new SolidBrush(Color))
            using (var penHD = GDI.Pen(Color, (float)LineWidth, LineStyle.Solid, true))
            {

                PointF[] PointBefore;
                PointF[] PointAfter;
                int searchFrom;
                int searchTo;

                // Calculate point before displayed points
                int pointBeforeIndex = Array.BinarySearch(xs, MinRenderIndex, maxRenderIndex - MinRenderIndex + 1, Convert.ChangeType(settings.axes.x.min, typeof(TX)));
                if (pointBeforeIndex < 0)
                {
                    pointBeforeIndex = ~pointBeforeIndex;
                }

                if (pointBeforeIndex > MinRenderIndex)
                {
                    PointBefore = new PointF[] { settings.GetPixel(Convert.ToDouble(xs[pointBeforeIndex - 1]), minmaxSearchStrategy.SourceElement(pointBeforeIndex - 1)) };
                    searchFrom = pointBeforeIndex;
                }
                else
                {
                    PointBefore = new PointF[] { };
                    searchFrom = MinRenderIndex;
                }

                // Calculate point after displayed points
                int pointAfterIndex = Array.BinarySearch(xs, MinRenderIndex, maxRenderIndex - MinRenderIndex + 1, Convert.ChangeType(settings.axes.x.max, typeof(TX)));
                if (pointAfterIndex < 0)
                {
                    pointAfterIndex = ~pointAfterIndex;
                }

                if (pointAfterIndex <= maxRenderIndex)
                {
                    PointAfter = new PointF[] { settings.GetPixel(Convert.ToDouble(xs[pointAfterIndex]), minmaxSearchStrategy.SourceElement(pointAfterIndex)) };
                    searchTo = pointAfterIndex;
                }
                else
                {
                    PointAfter = new PointF[] { };
                    searchTo = maxRenderIndex;
                }

                IEnumerable<PointF> VisiblePoints;
                if (UseParallel)
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
                    markerPxRadius = Math.Min(markerPxRadius, MarkerSize / 2);
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

        public override string ToString()
        {
            string label = string.IsNullOrWhiteSpace(this.Label) ? "" : $" ({this.Label})";
            return $"PlottableSignalXYGeneric{label} with {GetPointCount()} points ({typeof(TX).Name}, {typeof(TY).Name})";
        }
    }
}
