using ScottPlot.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace ScottPlot.Plottable
{
    /// <summary>
    /// A variation of the SignalPlotConst optimized for unevenly-spaced ascending X values.
    /// </summary>
    /// <typeparam name="TX"></typeparam>
    /// <typeparam name="TY"></typeparam>
    public class SignalPlotXYGeneric<TX, TY> : SignalPlotBase<TY>,
        IHasPointsGenericX<TX, TY> where TX : struct,
        IComparable where TY : struct, IComparable
    {
        /// <summary>
        /// Indicates whether Xs have been validated to ensure all values are ascending.
        /// Validation occurs before the first render (not at assignment) to allow the user time to set min/max render indexes.
        /// </summary>
        private bool XsHaveBeenValidated = false;

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

                _Xs = value;
                XsHaveBeenValidated = false;
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

        private static Func<TX, TX, TX> SubstractExp = NumericConversion.CreateSubtractFunction<TX>();

        private static Func<TX, TX, bool> LessThanOrEqualExp = NumericConversion.CreateLessThanOrEqualFunction<TX>();

        public SignalPlotXYGeneric() : base()
        {
            if (Type.GetTypeCode(typeof(TX)) == TypeCode.Byte)
            {
                throw new InvalidOperationException("SignalXY plots cannot use a byte array for their horizontal axis positions");
            }
        }

        public override AxisLimits GetAxisLimits()
        {
            var baseLimits = base.GetAxisLimits();
            var newXMin = NumericConversion.GenericToDouble(Xs, MinRenderIndex) + OffsetX;
            var newXMax = NumericConversion.GenericToDouble(Xs, MaxRenderIndex) + OffsetX;
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
            NumericConversion.DoubleToGeneric(dims.XMin + dims.XSpan / dims.DataWidth * x - OffsetX, out TX start);
            NumericConversion.DoubleToGeneric(dims.XMin + dims.XSpan / dims.DataWidth * (x + 1) - OffsetX, out TX end);

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

            yield return new PointF(x + dims.DataOffsetX, dims.GetPixelY(Strategy.SourceElement(startIndex) + OffsetYAsDouble));
            if (pointsCount > 1)
            {
                yield return new PointF(x + dims.DataOffsetX, dims.GetPixelY(min + OffsetYAsDouble));
                yield return new PointF(x + dims.DataOffsetX, dims.GetPixelY(max + OffsetYAsDouble));
                yield return new PointF(x + dims.DataOffsetX, dims.GetPixelY(Strategy.SourceElement(endIndex - 1) + OffsetYAsDouble));
            }
        }

        public override void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            if (!XsHaveBeenValidated)
            {
                Validate.AssertDoesNotDescend("xs", Xs, MinRenderIndex, MaxRenderIndex);
                XsHaveBeenValidated = true;
            }

            using (Graphics gfx = GDI.Graphics(bmp, dims, lowQuality))
            using (var brush = new SolidBrush(Color))
            using (var penHD = GDI.Pen(Color, (float)LineWidth, LineStyle, true))
            {

                PointF[] PointBefore;
                PointF[] PointAfter;
                int searchFrom;
                int searchTo;

                // Calculate point before displayed points
                NumericConversion.DoubleToGeneric(dims.XMin - OffsetX, out TX x);
                int pointBeforeIndex = Array.BinarySearch(Xs, MinRenderIndex, MaxRenderIndex - MinRenderIndex + 1, x);
                if (pointBeforeIndex < 0)
                {
                    pointBeforeIndex = ~pointBeforeIndex;
                }

                if (pointBeforeIndex > MinRenderIndex)
                {
                    PointBefore = new PointF[]
                    {
                        new PointF(dims.GetPixelX(NumericConversion.GenericToDouble(Xs, pointBeforeIndex - 1) + OffsetX),
                                   dims.GetPixelY(Strategy.SourceElement(pointBeforeIndex - 1) + OffsetYAsDouble))
                    };
                    searchFrom = pointBeforeIndex;
                }
                else
                {
                    PointBefore = new PointF[] { };
                    searchFrom = MinRenderIndex;
                }

                // Calculate point after displayed points
                NumericConversion.DoubleToGeneric(dims.XMax - OffsetX, out x);
                int pointAfterIndex = Array.BinarySearch(Xs, MinRenderIndex, MaxRenderIndex - MinRenderIndex + 1, x);
                if (pointAfterIndex < 0)
                {
                    pointAfterIndex = ~pointAfterIndex;
                }

                if (pointAfterIndex <= MaxRenderIndex)
                {
                    PointAfter = new PointF[]
                    {
                        new PointF(dims.GetPixelX(NumericConversion.GenericToDouble(Xs, pointAfterIndex) + OffsetX),
                                   dims.GetPixelY(Strategy.SourceElement(pointAfterIndex) + OffsetYAsDouble))
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
                if (PointBefore.Length > 0 && PointsToDraw.Length >= 2 && !StepDisplay)
                {
                    // only extrapolate if points are different (otherwise extrapolated point may be infinity)
                    if (PointsToDraw[0].X != PointsToDraw[1].X)
                    {
                        float x0 = -1 + dims.DataOffsetX;
                        float y0 = PointsToDraw[1].Y + (PointsToDraw[0].Y - PointsToDraw[1].Y) * (x0 - PointsToDraw[1].X) / (PointsToDraw[0].X - PointsToDraw[1].X);
                        PointsToDraw[0] = new PointF(x0, y0);
                    }
                }

                // Interpolate after displayed point to make it x = datasize.Width(close to visible area)
                // this fix extreme zoom in bug
                if (PointAfter.Length > 0 && PointsToDraw.Length >= 2 && !StepDisplay)
                {
                    PointF lastPoint = PointsToDraw[PointsToDraw.Length - 2];
                    PointF afterPoint = PointsToDraw[PointsToDraw.Length - 1];

                    // only extrapolate if points are different (otherwise extrapolated point may be infinity)
                    if (afterPoint.X != lastPoint.X)
                    {
                        float x1 = dims.DataWidth + dims.DataOffsetX;
                        float y1 = lastPoint.Y + (afterPoint.Y - lastPoint.Y) * (x1 - lastPoint.X) / (afterPoint.X - lastPoint.X);
                        PointsToDraw[PointsToDraw.Length - 1] = new PointF(x1, y1);
                    }
                }

                PointF[] markersToDraw = PointsToDraw;

                // Simulate a step display by adding extra points at the corners.
                if (StepDisplay)
                    PointsToDraw = ScatterPlot.GetStepDisplayPoints(PointsToDraw, StepDisplayRight);

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
                if (PointsToDraw.Length > 1 && LineStyle != LineStyle.None && LineWidth > 0)
                {
                    ValidatePoints(PointsToDraw);
                    gfx.DrawLines(penHD, PointsToDraw);
                }

                // draw markers
                if (markersToDraw.Length > 1)
                {
                    float dataSpanXPx = markersToDraw[markersToDraw.Length - 1].X - markersToDraw[0].X;
                    float markerPxRadius = .3f * dataSpanXPx / markersToDraw.Length;
                    markerPxRadius = Math.Min(markerPxRadius, MarkerSize / 2);
                    float scaledMarkerSize = markerPxRadius * 2;

                    if (markerPxRadius > .3)
                    {
                        ShowMarkersInLegend = true;

                        // skip not visible before and after points
                        var PointsWithMarkers = markersToDraw
                                                .Skip(PointBefore.Length)
                                                .Take(markersToDraw.Length - PointBefore.Length - PointAfter.Length)
                                                .ToArray();

                        MarkerTools.DrawMarkers(gfx, PointsWithMarkers, MarkerShape, scaledMarkerSize, MarkerColor, MarkerLineWidth);
                    }
                    else
                    {
                        ShowMarkersInLegend = false;
                    }
                }
            }
        }

        public override void ValidateData(bool deep = false)
        {
            // base can only check Ys
            base.ValidateData(deep);

            // X checking must be performed here
            Validate.AssertEqualLength($"{nameof(Xs)} and {nameof(Ys)}", Xs, Ys);
            Validate.AssertHasElements(nameof(Xs), Xs);

            if (deep)
            {
                Validate.AssertAllReal(nameof(Xs), Xs);
                Validate.AssertDoesNotDescend(nameof(Xs), Xs, MinRenderIndex, MaxRenderIndex);
            }
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
            int index = Array.BinarySearch(Xs, MinRenderIndex, MaxRenderIndex - MinRenderIndex, x);
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

        public (TY yMin, TY yMax) GetYDataRange(TX xStart, TX xEnd)
        {
            return base.GetYDataRange(NumericConversion.GenericToDouble(ref xStart), NumericConversion.GenericToDouble(ref xEnd));
        }
    }
}
