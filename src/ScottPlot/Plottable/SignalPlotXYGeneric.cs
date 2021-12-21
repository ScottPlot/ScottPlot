using ScottPlot.Drawing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace ScottPlot.Plottable
{
    /// <summary>
    /// A variation of the SignalPlotConst optimized for unevenly-spaced ascending X values.
    /// </summary>
    /// <typeparam name="TX"></typeparam>
    /// <typeparam name="TY"></typeparam>
    public class SignalPlotXYGeneric<TX, TY> : SignalPlotBase<TY>, IHasPointsGenericX<TX, TY> where TX : struct, IComparable where TY : struct, IComparable
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
            InitExp();
        }

        public override AxisLimits GetAxisLimits()
        {
            var baseLimits = base.GetAxisLimits();
            var newXMin = Convert.ToDouble(Xs[MinRenderIndex]) + OffsetX;
            var newXMax = Convert.ToDouble(Xs[MaxRenderIndex]) + OffsetX;
            return new AxisLimits(newXMin, newXMax, baseLimits.YMin, baseLimits.YMax);
        }

        /// <summary>
        /// TODO: document this
        /// </summary>
        /// <param name="x"></param>
        /// <param name="from"></param>
        /// <param name="length"></param>
        /// <param name="dims"></param>
        /// <returns></returns>
        public IEnumerable<PointF> ProcessInterval(int x, int from, int length, PlotDimensions dims)
        {
            TX start = (TX)Convert.ChangeType(dims.XMin + dims.XSpan / dims.DataWidth * x - OffsetX, typeof(TX));
            TX end = (TX)Convert.ChangeType(dims.XMin + dims.XSpan / dims.DataWidth * (x + 1) - OffsetX, typeof(TX));

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

            yield return new PointF(x + dims.DataOffsetX, dims.GetPixelY(Strategy.SourceElement(startIndex) + Convert.ToDouble(OffsetY)));
            if (pointsCount > 1)
            {
                yield return new PointF(x + dims.DataOffsetX, dims.GetPixelY(min + Convert.ToDouble(OffsetY)));
                yield return new PointF(x + dims.DataOffsetX, dims.GetPixelY(max + Convert.ToDouble(OffsetY)));
                yield return new PointF(x + dims.DataOffsetX, dims.GetPixelY(Strategy.SourceElement(endIndex - 1) + Convert.ToDouble(OffsetY)));
            }
        }

        public override void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            using (Graphics gfx = GDI.Graphics(bmp, dims, lowQuality))
            using (var brush = new SolidBrush(Color))
            using (var penHD = GDI.Pen(Color, (float)LineWidth, LineStyle, true))
            {

                PointF[] PointBefore;
                PointF[] PointAfter;
                int searchFrom;
                int searchTo;

                // Calculate point before displayed points
                int pointBeforeIndex = Array.BinarySearch(Xs, MinRenderIndex, MaxRenderIndex - MinRenderIndex + 1, Convert.ChangeType(dims.XMin - OffsetX, typeof(TX)));
                if (pointBeforeIndex < 0)
                {
                    pointBeforeIndex = ~pointBeforeIndex;
                }

                if (pointBeforeIndex > MinRenderIndex)
                {
                    PointBefore = new PointF[]
                    {
                        new PointF(dims.GetPixelX(Convert.ToDouble(Xs[pointBeforeIndex - 1]) + OffsetX),
                                   dims.GetPixelY(Strategy.SourceElement(pointBeforeIndex - 1) + Convert.ToDouble(OffsetY)))
                    };
                    searchFrom = pointBeforeIndex;
                }
                else
                {
                    PointBefore = new PointF[] { };
                    searchFrom = MinRenderIndex;
                }

                // Calculate point after displayed points
                int pointAfterIndex = Array.BinarySearch(Xs, MinRenderIndex, MaxRenderIndex - MinRenderIndex + 1, Convert.ChangeType(dims.XMax - OffsetX, typeof(TX)));
                if (pointAfterIndex < 0)
                {
                    pointAfterIndex = ~pointAfterIndex;
                }

                if (pointAfterIndex <= MaxRenderIndex)
                {
                    PointAfter = new PointF[]
                    {
                        new PointF(dims.GetPixelX(Convert.ToDouble(Xs[pointAfterIndex]) + OffsetX),
                                   dims.GetPixelY(Strategy.SourceElement(pointAfterIndex) + Convert.ToDouble(OffsetY)))
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
                    VisiblePoints = Enumerable.Range(0, (int)Math.Round(dims.DataWidth))
                                              .AsParallel()
                                              .AsOrdered()
                                              .Select(x => ProcessInterval(x, searchFrom, searchTo - searchFrom + 1, dims))
                                              .SelectMany(x => x);

                }
                else
                {
                    VisiblePoints = Enumerable.Range(0, (int)Math.Round(dims.DataWidth))
                                              .Select(x => ProcessInterval(x, searchFrom, searchTo - searchFrom + 1, dims))
                                              .SelectMany(x => x);
                }

                PointF[] PointsToDraw = PointBefore.Concat(VisiblePoints).Concat(PointAfter).ToArray();

                // Interpolate before displayed point to make it x = -1 (close to visible area)
                // this fix extreme zoom in bug
                if (PointBefore.Length > 0 && PointsToDraw.Length >= 2)
                {
                    float x0 = -1 + dims.DataOffsetX;
                    float y0 = PointsToDraw[1].Y + (PointsToDraw[0].Y - PointsToDraw[1].Y) * (x0 - PointsToDraw[1].X) / (PointsToDraw[0].X - PointsToDraw[1].X);
                    PointsToDraw[0] = new PointF(x0, y0);
                }
                // Interpolate after displayed point to make it x = datasize.Width(close to visible area)
                // this fix extreme zoom in bug
                if (PointAfter.Length > 0 && PointsToDraw.Length >= 2)
                {
                    PointF lastPoint = PointsToDraw[PointsToDraw.Length - 2];
                    PointF afterPoint = PointsToDraw[PointsToDraw.Length - 1];

                    float x1 = dims.DataWidth + dims.DataOffsetX;
                    float y1 = lastPoint.Y + (afterPoint.Y - lastPoint.Y) * (x1 - lastPoint.X) / (afterPoint.X - lastPoint.X);
                    PointsToDraw[PointsToDraw.Length - 1] = new PointF(x1, y1);
                }

                // Fill below the line
                switch (_FillType)
                {
                    case FillType.NoFill:
                        break;
                    case FillType.FillAbove:
                        FillToInfinity(dims, gfx, PointsToDraw[0].X, PointsToDraw[PointsToDraw.Length - 1].X, PointsToDraw, true);
                        break;
                    case FillType.FillBelow:
                        FillToInfinity(dims, gfx, PointsToDraw[0].X, PointsToDraw[PointsToDraw.Length - 1].X, PointsToDraw, false);
                        break;
                    case FillType.FillAboveAndBelow:
                        FillToBaseline(dims, gfx, PointsToDraw[0].X, PointsToDraw[PointsToDraw.Length - 1].X, PointsToDraw, BaselineY);
                        break;
                    default:
                        throw new InvalidOperationException("unsupported fill type");
                }

                // Draw lines
                if (PointsToDraw.Length > 1)
                {
                    PointF[] pointsArray = PointsToDraw.ToArray();
                    ValidatePoints(pointsArray);

                    if (StepDisplay)
                        pointsArray = GetStepPoints(pointsArray);

                    gfx.DrawLines(penHD, pointsArray);
                }

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

        private (TX x, TY y, int index) GetPointByIndex(int index)
        {
            return (Xs[index], Ys[index], index);
        }

        /// <summary>
        /// Return the X/Y coordinates of the point nearest the X position
        /// </summary>
        /// <param name="x">X position in plot space</param>
        /// <returns></returns>
        public (TX x, TY y, int index) GetPointNearestX(TX x)
        {
            int index = Array.BinarySearch(Xs, MinRenderIndex, MaxRenderIndex - 1, x);
            if (index < 0)
            {
                index = ~index;
            }
            else // x equal to XS element
            {
                return GetPointByIndex(index);
            }
            if (index <= MinRenderIndex) // x lower then any MinRenderIndex
                return GetPointByIndex(MinRenderIndex);
            if (index > MaxRenderIndex) // x higher then MaxRenderIndex
                return GetPointByIndex(MaxRenderIndex);

            TX distLeft = SubstractExp(x, Xs[index - 1]);
            TX distRight = SubstractExp(Xs[index], x);
            if (LessThanOrEqualExp(distLeft, distRight)) // x closer to XS[index -1]
                return GetPointByIndex(index - 1);
            else // x closer to XS[index]
                return GetPointByIndex(index);
        }

        /// <summary>
        /// This method to pass test only
        /// </summary>
        /// <param name="x">X position in plot space</param>
        /// <returns></returns>
        private new(double x, TY y, int index) GetPointNearestX(double x)
        {
            throw new NotImplementedException();
        }

        private static Func<TX, TX, TX> SubstractExp;
        private static Func<TX, TX, bool> LessThanOrEqualExp;

        private void InitExp()
        {
            ParameterExpression paramA = Expression.Parameter(typeof(TX), "a");
            ParameterExpression paramB = Expression.Parameter(typeof(TX), "b");
            BinaryExpression bodySubstract = Expression.Subtract(paramA, paramB);
            BinaryExpression bodyLessOrEqual = Expression.LessThanOrEqual(paramA, paramB);
            SubstractExp = Expression.Lambda<Func<TX, TX, TX>>(bodySubstract, paramA, paramB).Compile();
            LessThanOrEqualExp = Expression.Lambda<Func<TX, TX, bool>>(bodyLessOrEqual, paramA, paramB).Compile();
        }
    }
}
