using ScottPlot.Drawing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;

namespace ScottPlot.Plottable
{
    public class SignalPlotXYGeneric<TX, TY> : SignalPlotBase<TY> where TX : struct, IComparable where TY : struct, IComparable
    {
        private TX[] _Xs;
        public TX[] Xs
        {
            get => _Xs;
            set
            {
                if (value == null)
                    throw new ArgumentException("XS cannot be null");
                if (value.Length == 0)
                    throw new ArgumentException("XS must have at least one element");

                for (int i = 1; i < value.Length; i++)
                    if (value[i].CompareTo(value[i - 1]) < 0)
                        throw new ArgumentException("Xs must only contain ascending values");

                _Xs = value;
            }
        }

        public override TY[] Ys
        {
            get => _Ys;
            set
            {
                if (value.Length == 0)
                    throw new ArgumentException("YS must have at least one element");

                base.Ys = value;
            }
        }

        public SignalPlotXYGeneric() : base()
        {

        }

        public override AxisLimits GetAxisLimits()
        {
            var baseLimits = base.GetAxisLimits();
            var newXMin = Convert.ToDouble(Xs[MinRenderIndex]);
            var newXMax = Convert.ToDouble(Xs[MaxRenderIndex]);
            Debug.WriteLine($"Limits: {newXMin} {newXMax}");
            return new AxisLimits(newXMin, newXMax, baseLimits.YMin, baseLimits.YMax);
        }

        public IEnumerable<PointF> ProcessInterval(int x, int from, int length, PlotDimensions dims)
        {
            TX start = (TX)Convert.ChangeType(dims.XMin + dims.XSpan / dims.DataWidth * x, typeof(TX));
            TX end = (TX)Convert.ChangeType(dims.XMin + dims.XSpan / dims.DataWidth * (x + 1), typeof(TX));

            int startIndex = Array.BinarySearch(Xs, from, length, start);
            if (startIndex < 0)
            {
                startIndex = ~startIndex;
            }

            int endIndex = Array.BinarySearch(Xs, from, length, end);
            if (endIndex < 0)
            {
                endIndex = ~endIndex;
            }

            if (startIndex == endIndex)
            {
                yield break;
            }

            Strategy.MinMaxRangeQuery(startIndex, endIndex - 1, out double min, out double max);

            var pointsCount = endIndex - startIndex;

            yield return new PointF(x, dims.GetPixelY(Strategy.SourceElement(startIndex)));
            if (pointsCount > 1)
            {
                yield return new PointF(x, dims.GetPixelY(min));
                yield return new PointF(x, dims.GetPixelY(max));
                yield return new PointF(x, dims.GetPixelY(Strategy.SourceElement(endIndex - 1)));
            }
        }

        public override void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            using (Graphics gfx = GDI.Graphics(bmp, dims, lowQuality))
            using (var brush = new SolidBrush(Color))
            using (var penHD = GDI.Pen(Color, (float)LineWidth, LineStyle.Solid, true))
            {

                PointF[] PointBefore;
                PointF[] PointAfter;
                int searchFrom;
                int searchTo;

                // Calculate point before displayed points
                int pointBeforeIndex = Array.BinarySearch(Xs, MinRenderIndex, MaxRenderIndex - MinRenderIndex + 1, Convert.ChangeType(dims.XMin, typeof(TX)));
                if (pointBeforeIndex < 0)
                {
                    pointBeforeIndex = ~pointBeforeIndex;
                }

                if (pointBeforeIndex > MinRenderIndex)
                {
                    PointBefore = new PointF[]
                    {
                        new PointF(dims.GetPixelX(Convert.ToDouble(Xs[pointBeforeIndex - 1])),
                                   dims.GetPixelY(Strategy.SourceElement(pointBeforeIndex - 1)))
                    };
                    searchFrom = pointBeforeIndex;
                }
                else
                {
                    PointBefore = new PointF[] { };
                    searchFrom = MinRenderIndex;
                }

                // Calculate point after displayed points
                int pointAfterIndex = Array.BinarySearch(Xs, MinRenderIndex, MaxRenderIndex - MinRenderIndex + 1, Convert.ChangeType(dims.XMax, typeof(TX)));
                if (pointAfterIndex < 0)
                {
                    pointAfterIndex = ~pointAfterIndex;
                }

                if (pointAfterIndex <= MaxRenderIndex)
                {
                    PointAfter = new PointF[]
                    {
                        new PointF(dims.GetPixelX(Convert.ToDouble(Xs[pointAfterIndex])),
                                   dims.GetPixelY(Strategy.SourceElement(pointAfterIndex)))
                    };
                    searchTo = pointAfterIndex;
                }
                else
                {
                    PointAfter = new PointF[] { };
                    searchTo = MaxRenderIndex;
                }

                IEnumerable<PointF> VisiblePoints;
                if (UseParallel)
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

                // correct points for data offset
                for (int i = 0; i < PointsToDraw.Length; i++)
                    PointsToDraw[i].X += dims.DataOffsetX;

                // Draw lines
                if (PointsToDraw.Length > 1)
                    gfx.DrawLines(penHD, PointsToDraw.ToArray());

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

        public new void ValidateData(bool deep = false)
        {
            base.ValidateData(deep);
            Validate.AssertEqualLength("xs and ys", Xs, Ys);
            Validate.AssertHasElements("xs", Xs);
            Validate.AssertAscending("xs", Xs);
        }

        public override string ToString()
        {
            string label = string.IsNullOrWhiteSpace(this.Label) ? "" : $" ({this.Label})";
            return $"PlottableSignalXYGeneric{label} with {PointCount} points ({typeof(TX).Name}, {typeof(TY).Name})";
        }
    }
}
