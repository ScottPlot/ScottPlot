using ScottPlot.Drawing;
using System.Drawing;

namespace ScottPlot.Plottable;

/// <summary>
/// Experimental plottabled refactored for migration to ScottPlot 5
/// </summary>
public class SignalXY2 : SignalPlotBase<double>
{
    private bool XsHaveBeenValidated = false;

    private double[] _Xs;
    public double[] Xs
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

    public override double[] Ys
    {
        get => _Ys;
        set
        {
            if (value.Length == 0)
                throw new ArgumentException("YS must have at least one element");

            base.Ys = value;
        }
    }

    private static Func<double, double, double> SubstractExp = NumericConversion.CreateSubtractFunction<double>();

    private static Func<double, double, bool> LessThanOrEqualExp = NumericConversion.CreateLessThanOrEqualFunction<double>();

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
        NumericConversion.DoubleToGeneric(dims.XMin + dims.XSpan / dims.DataWidth * x - OffsetX, out double start);
        NumericConversion.DoubleToGeneric(dims.XMin + dims.XSpan / dims.DataWidth * (x + 1) - OffsetX, out double end);

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

        yield return new PointF(x + dims.DataOffsetX, dims.GetPixelY((Strategy.SourceElement(startIndex) * ScaleYAsDouble) + OffsetYAsDouble));
        if (pointsCount > 1)
        {
            yield return new PointF(x + dims.DataOffsetX, dims.GetPixelY(min * ScaleYAsDouble + OffsetYAsDouble));
            yield return new PointF(x + dims.DataOffsetX, dims.GetPixelY(max * ScaleYAsDouble + OffsetYAsDouble));
            yield return new PointF(x + dims.DataOffsetX, dims.GetPixelY((Strategy.SourceElement(endIndex - 1) * ScaleYAsDouble) + OffsetYAsDouble));
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
            NumericConversion.DoubleToGeneric(dims.XMin - OffsetX, out double x);
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
                               dims.GetPixelY((Strategy.SourceElement(pointBeforeIndex - 1) * ScaleYAsDouble) + OffsetYAsDouble))
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
                               dims.GetPixelY((Strategy.SourceElement(pointAfterIndex) * ScaleYAsDouble)+ OffsetYAsDouble))
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
            if (markersToDraw.Length >= 1)
            {
                float dataSpanXPx = markersToDraw[markersToDraw.Length - 1].X - markersToDraw[0].X;
                float markerPxRadius = .3f * dataSpanXPx / markersToDraw.Length;
                markerPxRadius = markersToDraw.Length > 1 ? Math.Min(markerPxRadius, MarkerSize / 2) : MarkerSize / 2;
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
        return $"PlottableSignalXYGeneric{label} with {PointCount} points ({typeof(double).Name}, {typeof(double).Name})";
    }

    private (double x, double y, int index) GetPointByIndex(int index)
    {
        return (Xs[index], Ys[index], index);
    }
}
