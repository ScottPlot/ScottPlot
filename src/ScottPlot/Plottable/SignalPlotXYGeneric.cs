﻿using ScottPlot.Ticks;
using ScottPlot.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;

namespace ScottPlot.Plottable
{

    public class SignalPlotXYGeneric<TX, TY> : SignalPlotBase<TY> where TX : struct, IComparable where TY : struct, IComparable
    {
        private bool XSequalYSPromise = false;

        private TX[] _xs;
        public TX[] xs
        {
            get => _xs;
            set
            {
                if (value == null)
                    throw new ArgumentException("XS cannot be null");
                if (value.Length == 0)
                    throw new ArgumentException("XS must have at least one element");

                for (int i = 1; i < value.Length; i++)
                    if (value[i].CompareTo(value[i - 1]) < 0)
                        throw new ArgumentException("Xs must only contain ascending values");

                XSequalYSPromise = (value.Length != ys?.Length);

                _xs = value;
            }
        }

        public override TY[] ys
        {
            get => _ys;
            set
            {
                if (value.Length == 0)
                    throw new ArgumentException("YS must have at least one element");

                XSequalYSPromise = (value.Length != xs?.Length);

                base.ys = value;
            }
        }

        public SignalPlotXYGeneric() : base()
        {

        }

        public AxisLimits GetLimits()
        {
            var baseLimits = GetAxisLimits();
            var newXMin = Convert.ToDouble(xs[minRenderIndex]);
            var newXMax = Convert.ToDouble(xs[maxRenderIndex]);
            return new AxisLimits(newXMin, newXMax, baseLimits.YMin, baseLimits.YMax);
        }

        public IEnumerable<PointF> ProcessInterval(int x, int from, int length, PlotDimensions dims)
        {
            TX start = (TX)Convert.ChangeType(dims.XMin + dims.XSpan / dims.DataWidth * x, typeof(TX));
            TX end = (TX)Convert.ChangeType(dims.XMin + dims.XSpan / dims.DataWidth * (x + 1), typeof(TX));

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

            yield return new PointF(x, dims.GetPixelY(minmaxSearchStrategy.SourceElement(startIndex)));
            if (pointsCount > 1)
            {
                yield return new PointF(x, dims.GetPixelY(min));
                yield return new PointF(x, dims.GetPixelY(max));
                yield return new PointF(x, dims.GetPixelY(minmaxSearchStrategy.SourceElement(endIndex - 1)));
            }
        }

        public override void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            using (Graphics gfx = Graphics.FromImage(bmp))
            using (var brush = new SolidBrush(color))
            using (var penHD = GDI.Pen(color, (float)lineWidth, LineStyle.Solid, true))
            {

                PointF[] PointBefore;
                PointF[] PointAfter;
                int searchFrom;
                int searchTo;

                // Calculate point before displayed points
                int pointBeforeIndex = Array.BinarySearch(xs, minRenderIndex, maxRenderIndex - minRenderIndex + 1, Convert.ChangeType(dims.XMin, typeof(TX)));
                if (pointBeforeIndex < 0)
                {
                    pointBeforeIndex = ~pointBeforeIndex;
                }

                if (pointBeforeIndex > minRenderIndex)
                {
                    PointBefore = new PointF[]
                    {
                        new PointF(dims.GetPixelX(Convert.ToDouble(xs[pointBeforeIndex - 1])),
                                   dims.GetPixelY(minmaxSearchStrategy.SourceElement(pointBeforeIndex - 1)))
                    };
                    searchFrom = pointBeforeIndex;
                }
                else
                {
                    PointBefore = new PointF[] { };
                    searchFrom = minRenderIndex;
                }

                // Calculate point after displayed points
                int pointAfterIndex = Array.BinarySearch(xs, minRenderIndex, maxRenderIndex - minRenderIndex + 1, Convert.ChangeType(dims.XMax, typeof(TX)));
                if (pointAfterIndex < 0)
                {
                    pointAfterIndex = ~pointAfterIndex;
                }

                if (pointAfterIndex <= maxRenderIndex)
                {
                    PointAfter = new PointF[]
                    {
                        new PointF(dims.GetPixelX(Convert.ToDouble(xs[pointAfterIndex])),
                                   dims.GetPixelY(minmaxSearchStrategy.SourceElement(pointAfterIndex)))
                    };
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
                    VisiblePoints = Enumerable.Range(0, (int)dims.DataWidth)
                                              .AsParallel()
                                              .AsOrdered()
                                              .Select(x => ProcessInterval(x, searchFrom, searchTo - searchFrom + 1, dims))
                                              .SelectMany(x => x);

                }
                else
                {
                    VisiblePoints = Enumerable.Range(0, (int)dims.DataWidth)
                                              .Select(x => ProcessInterval(x, searchFrom, searchTo - searchFrom + 1, dims))
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

                    float x1 = dims.DataWidth;
                    float y1 = lastPoint.Y + (afterPoint.Y - lastPoint.Y) * (x1 - lastPoint.X) / (afterPoint.X - lastPoint.X);
                    PointsToDraw[PointsToDraw.Length - 1] = new PointF(x1, y1);
                }

                // Draw lines
                if (PointsToDraw.Length > 1)
                    gfx.DrawLines(penHD, PointsToDraw.ToArray());

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

                            gfx.FillEllipse(brush,
                                  x: pt.X - markerPxRadius + markerOffsetX,
                                  y: pt.Y - markerPxRadius,
                                  width: markerPxRadius * 2,
                                  height: markerPxRadius * 2);
                        }
                    }
                }
            }
        }

        public override string ErrorMessage(bool deepValidation = false)
        {
            try
            {
                Validate.AssertEqualLength("xs and ys", xs, ys);
                Validate.AssertHasElements("xs", xs);
                Validate.AssertAscending("xs", xs);
                return null;
            }
            catch (ArgumentException e)
            {
                return e.Message;
            }
        }

        public override string ToString()
        {
            string label = string.IsNullOrWhiteSpace(this.label) ? "" : $" ({this.label})";
            return $"PlottableSignalXYGeneric{label} with {PointCount} points ({typeof(TX).Name}, {typeof(TY).Name})";
        }
    }
}
