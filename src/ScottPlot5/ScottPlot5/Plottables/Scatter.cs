using System.Data;
using System.Linq;

namespace ScottPlot.Plottables;

public class Scatter(IScatterSource data) : IPlottable, IHasLine, IHasMarker, IHasLegendText, IDataSource, IGetNearest
{
    [Obsolete("use LegendText")]
    public string Label { get => LegendText; set => LegendText = value; }
    public string LegendText { get; set; } = string.Empty;

    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = new Axes();

    public LineStyle LineStyle { get; set; } = new() { Width = 1 };
    public float LineWidth { get => LineStyle.Width; set => LineStyle.Width = value; }
    public LinePattern LinePattern { get => LineStyle.Pattern; set => LineStyle.Pattern = value; }
    public Color LineColor { get => LineStyle.Color; set => LineStyle.Color = value; }

    public int MinRenderIndex { get => Data.MinRenderIndex; set => Data.MinRenderIndex = value; }
    public int MaxRenderIndex { get => Data.MaxRenderIndex; set => Data.MaxRenderIndex = value; }

    public MarkerStyle MarkerStyle { get; set; } = new()
    {
        LineWidth = 1,
        Size = 5,
        Shape = MarkerShape.FilledCircle,
    };
    public MarkerShape MarkerShape { get => MarkerStyle.Shape; set => MarkerStyle.Shape = value; }
    public float MarkerSize { get => MarkerStyle.Size; set => MarkerStyle.Size = value; }
    public Color MarkerFillColor { get => MarkerStyle.FillColor; set => MarkerStyle.FillColor = value; }
    public Color MarkerLineColor { get => MarkerStyle.LineColor; set => MarkerStyle.LineColor = value; }
    public Color MarkerColor { get => MarkerStyle.MarkerColor; set => MarkerStyle.MarkerColor = value; }
    public float MarkerLineWidth { get => MarkerStyle.LineWidth; set => MarkerStyle.LineWidth = value; }

    public IScatterSource Data { get; } = data;

    public bool FillY { get; set; } = false;
    public bool FillYBelow { get; set; } = true;
    public bool FillYAbove { get; set; } = true;
    public double FillYValue { get; set; } = 0;
    public Color FillYAboveColor { get; set; } = Colors.Blue.WithAlpha(.2);
    public Color FillYBelowColor { get; set; } = Colors.Blue.WithAlpha(.2);
    public Color FillYColor { get => FillYAboveColor; set { FillYAboveColor = value; FillYBelowColor = value; } }

    public List<ColorPosition> ColorPositions { get; set; } = [];

    public record struct ColorPosition(Color Color, double Position);

    public double OffsetX { get; set; } = 0;
    public double OffsetY { get; set; } = 0;
    public double ScaleX { get; set; } = 1;
    public double ScaleY { get; set; } = 1;

    /// <summary>
    /// The style of lines to use when connecting points.
    /// </summary>
    public ConnectStyle ConnectStyle = ConnectStyle.Straight;

    /// <summary>
    /// Controls whether points are connected by smooth or straight lines
    /// </summary>
    public bool Smooth
    {
        set
        {
            PathStrategy = value
                ? new PathStrategies.CubicSpline()
                : new PathStrategies.Straight();
        }
    }

    /// <summary>
    /// Setting this value enables <see cref="Smooth"/> and sets the curve tension.
    /// Low tensions tend to "overshoot" data points.
    /// High tensions begin to approach connecting points with straight lines.
    /// </summary>
    public double SmoothTension
    {
        get
        {
            if (PathStrategy is PathStrategies.CubicSpline cs)
            {
                return cs.Tension;
            }
            else
            {
                return 0;
            }
        }
        set
        {
            PathStrategy = new PathStrategies.CubicSpline() { Tension = value };
        }
    }

    /// <summary>
    /// Strategy to use for generating the path used to connect points
    /// </summary>
    public IPathStrategy PathStrategy { get; set; } = new PathStrategies.Straight();

    public Color Color
    {
        get => LineStyle.Color;
        set
        {
            LineStyle.Color = value;
            MarkerStyle.FillColor = value;
            MarkerStyle.LineColor = value;
        }
    }

    public AxisLimits GetAxisLimits()
    {
        ExpandingAxisLimits limits = new(Data.GetLimits());

        if (FillY)
            limits.ExpandY(FillYValue);

        return new AxisLimits(
            left: limits.Left * ScaleX + OffsetX,
            right: limits.Right * ScaleX + OffsetX,
            bottom: limits.Bottom * ScaleY + OffsetY,
            top: limits.Top * ScaleY + OffsetY);
    }

    public IEnumerable<LegendItem> LegendItems => LegendItem.Single(this, LegendText, MarkerStyle, LineStyle);

    private Gradient CreateXAxisGradient(RenderPack rp)
    {
        float xMin = (float)Axes.XAxis.GetCoordinate(rp.DataRect.Left, rp.DataRect);
        float xMax = (float)Axes.XAxis.GetCoordinate(rp.DataRect.Right, rp.DataRect);

        // TODO: move this logic to a positioned color colormap class
        Color interpolate(double val)
        {
            if (ColorPositions.Select(x => x.Position).All(i => val < i))
            {
                return ColorPositions.First().Color;
            }

            if (ColorPositions.Select(x => x.Position).All(i => val > i))
            {
                return ColorPositions.Last().Color;
            }

            int lIdx = -1;
            int rIdx = -1;
            for (int i = 0; i < ColorPositions.Count; i++)
            {
                if (ColorPositions[i].Position <= val &&
                    (lIdx < 0 || ColorPositions[i].Position > ColorPositions[lIdx].Position))
                {
                    lIdx = i;
                }

                if (ColorPositions[i].Position >= val &&
                    (rIdx < 0 || ColorPositions[i].Position < ColorPositions[rIdx].Position))
                {
                    rIdx = i;
                }
            }

            if (lIdx == rIdx)
            {
                return ColorPositions[lIdx].Color;
            }

            double factor = (val - ColorPositions[lIdx].Position) / (ColorPositions[rIdx].Position - ColorPositions[lIdx].Position);
            return ColorPositions[lIdx].Color.InterpolateRgb(ColorPositions[rIdx].Color, factor);
        }

        float GetAxisFractionX(double x)
        {
            double distanceFromLeft = Axes.GetPixelX(x) - Axes.DataRect.Left;
            double width = Axes.DataRect.Right - Axes.DataRect.Left;
            return (float)(distanceFromLeft / width);
        }

        IOrderedEnumerable<double> colorPositions = ColorPositions
            .Select(x => x.Position)
            .Concat([xMin, xMax])
            .Where(i => i >= xMin && i <= xMax)
            .OrderBy(i => i);

        IEnumerable<Color> colors = colorPositions.Select(interpolate);
        float[] colorPositionsFrac = colorPositions.Select(GetAxisFractionX).ToArray();

        return new Gradient()
        {
            GradientType = GradientType.Linear,
            AlignmentStart = Alignment.MiddleLeft,
            AlignmentEnd = Alignment.MiddleRight,
            ColorPositions = colorPositionsFrac,
            Colors = colors.ToArray(),
        };
    }

    public virtual void Render(RenderPack rp)
    {
        // TODO: can this be done with an iterator to avoid copying?
        var coordinates = Data.GetScatterPoints();

        Pixel[] markerPixels = new Pixel[coordinates.Count];
        for (int i = 0; i < coordinates.Count; i++)
        {
            double x = coordinates[i].X * ScaleX + OffsetX;
            double y = coordinates[i].Y * ScaleY + OffsetY;
            markerPixels[i] = Axes.GetPixel(new(x, y));
        }

        if (markerPixels.Length == 0)
            return;

        Pixel[] linePixels = ConnectStyle switch
        {
            ConnectStyle.Straight => markerPixels,
            ConnectStyle.StepHorizontal => GetStepDisplayPixels(markerPixels, true),
            ConnectStyle.StepVertical => GetStepDisplayPixels(markerPixels, false),
            _ => throw new NotImplementedException($"unsupported {nameof(ConnectStyle)}: {ConnectStyle}"),
        };

        using SKPaint paint = new();
        using SKPath path = PathStrategy.GetPath(linePixels);

        if (FillY)
        {
            FillStyle fs = new()
            {
                IsVisible = true,
            };

            if (ColorPositions.Count > 0)
            {
                fs.Hatch = CreateXAxisGradient(rp);
            }

            PixelRect dataPxRect = new(markerPixels);

            PixelRect rect = new(linePixels);
            float yValuePixel = Axes.YAxis.GetPixel(FillYValue + OffsetY, rp.DataRect);

            using SKPath fillPath = new(path);
            fillPath.LineTo(rect.Right, yValuePixel);
            fillPath.LineTo(rect.Left, yValuePixel);

            bool midWay = yValuePixel < dataPxRect.Bottom && yValuePixel > dataPxRect.Top;
            bool belowOnly = yValuePixel <= dataPxRect.Top;
            bool aboveOnly = yValuePixel >= dataPxRect.Bottom;

            if (midWay || aboveOnly)
            {
                PixelRect rectAbove = new(rp.DataRect.Left, rp.DataRect.Right, yValuePixel, rect.Top);
                rp.CanvasState.Save();
                rp.CanvasState.Clip(rectAbove);
                fs.Color = ColorPositions.Count > 0 ? Colors.Black : FillYAboveColor;
                Drawing.DrawPath(rp.Canvas, paint, fillPath, fs, rectAbove);
                rp.CanvasState.Restore();
            }

            if (midWay || belowOnly)
            {
                PixelRect rectBelow = new(rp.DataRect.Left, rp.DataRect.Right, rect.Bottom, yValuePixel);
                rp.CanvasState.Save();
                rp.CanvasState.Clip(rectBelow);
                fs.Color = ColorPositions.Count > 0 ? Colors.Black : FillYBelowColor;
                Drawing.DrawPath(rp.Canvas, paint, fillPath, fs, rectBelow);
                rp.CanvasState.Restore();
            }
        }

        Drawing.DrawLines(rp.Canvas, paint, path, LineStyle);
        Drawing.DrawMarkers(rp.Canvas, paint, markerPixels, MarkerStyle);
    }

    /// <summary>
    /// Convert scatter plot points (connected by diagonal lines) to step plot points (connected by right angles)
    /// by inserting an extra point between each of the original data points to result in L-shaped steps.
    /// </summary>
    /// <param name="points">Array of corner positions</param>
    /// <param name="right">Indicates that a line will extend to the right before rising or falling.</param>
    public static Pixel[] GetStepDisplayPixels(Pixel[] pixels, bool right)
    {
        Pixel[] pixelsStep = new Pixel[pixels.Count() * 2 - 1];

        int offsetX = right ? 1 : 0;
        int offsetY = right ? 0 : 1;

        for (int i = 0; i < pixels.Count() - 1; i++)
        {
            pixelsStep[i * 2] = pixels[i];
            pixelsStep[i * 2 + 1] = new Pixel(pixels[i + offsetX].X, pixels[i + offsetY].Y);
        }

        pixelsStep[pixelsStep.Length - 1] = pixels[pixels.Length - 1];

        return pixelsStep;
    }


    public DataPoint GetNearest(Coordinates mouseLocation, RenderDetails renderInfo, float maxDistance = 15)
    {
        if (Data is IDataSource ds && ds.IsSorted())
            return DataSourceUtilities.GetNearest(this, mouseLocation, renderInfo, maxDistance, Axes.XAxis, Axes.YAxis);
        else
            return DataSourceUtilities.GetNearest(GetIDataSource(), mouseLocation, renderInfo, maxDistance, Axes.XAxis, Axes.YAxis);
    }

    public DataPoint GetNearestX(Coordinates mouseLocation, RenderDetails renderInfo, float maxDistance = 15)
    {
        if (Data is IDataSource ds && ds.IsSorted())
            return DataSourceUtilities.GetNearestXFast(this, mouseLocation, renderInfo, maxDistance, Axes.XAxis);
        else
            return DataSourceUtilities.GetNearestX(GetIDataSource(), mouseLocation, renderInfo, maxDistance, Axes.XAxis);
    }

    /// <summary>
    /// Returns an optimized <see cref="IDataSource"/> to work with when having to iterate across the collection of points
    /// </summary>
    public IDataSource GetIDataSource()
    {
        if (Data is IDataSource) return this; // should be already optimized, as this just passed calls to the data directly

        // benefits : wrapping the collection will handle the offset, scaling and axes whereas IScatterSource.GetNearest will not.
        // cons : copies values to array. BUT, this should still be much better than having to get the value from the GetScatterPoints() every interface call.
        return new DataSources.CoordinateDataSource(Data.GetScatterPoints())
        {
            XOffset = OffsetX,
            XScale = ScaleX,
            YOffset = OffsetY,
            YScale = ScaleY,
        };
    }

    #region IDataSource

    // Data object is assumed to implement IDataSource and will provide best execution

    readonly IDataSource? _dataSource = data as IDataSource;
    bool IDataSource.IsSorted() => _dataSource?.IsSorted() ?? false;
    bool IDataSource.PreferCoordinates => _dataSource?.PreferCoordinates ?? true;
    int IDataSource.Length => _dataSource?.Length ?? Data.GetScatterPoints().Count;
    int IDataSource.MinRenderIndex => Data.MinRenderIndex;
    int IDataSource.MaxRenderIndex => Data.MaxRenderIndex;

    int IDataSource.GetXClosestIndex(Coordinates mouseLocation)
    {
        if (OffsetX == 0 && ScaleX == 1 && Data is IDataSource ds)
            return ds.GetXClosestIndex(mouseLocation);

        var points = Data.GetScatterPoints();
        if (points.IsAscending(BinarySearchComparer.Instance))
        {
            return DataSourceUtilities.GetClosestIndex(
                points,
                DataSourceUtilities.UnScaleCoordinate(mouseLocation, ScaleX, OffsetX, ScaleY, OffsetY),
                new IndexRange(0, points.Count - 1),
                BinarySearchComparer.Instance
                );
        }
        else
        {
            int closestIndex = -1;
            double closestDistance = double.PositiveInfinity;
            mouseLocation = DataSourceUtilities.UnScaleCoordinate(mouseLocation, ScaleX, OffsetX, ScaleY, OffsetY);

            for (int i = 0; i < points.Count; i++)
            {
                double dx = mouseLocation.X - points[i].X;

                if (dx < closestDistance)
                {
                    if (dx == 0) return i;
                    closestDistance = dx;
                    closestIndex = i;
                }
            }
            return closestIndex;
        }
    }

    Coordinates IDataSource.GetCoordinate(int index)
    {
        return _dataSource?.GetCoordinate(index) ?? Data.GetScatterPoints()[index];
    }

    Coordinates IDataSource.GetCoordinateScaled(int index)
    {
        IDataSource ds = this;
        if (ds.PreferCoordinates)
        {
            return DataSourceUtilities.ScaleCoordinate(ds.GetCoordinate(index), ScaleX, OffsetX, ScaleY, OffsetY);
        }
        return new Coordinates()
        {
            X = ds.GetXScaled(index),
            Y = ds.GetYScaled(index)
        };
    }

    double IDataSource.GetX(int index)
    {
        return _dataSource?.GetX(index) ?? Data.GetScatterPoints()[index].X;
    }

    double IDataSource.GetXScaled(int index)
    {
        return DataSourceUtilities.ScaleXY(
            value: _dataSource?.GetX(index) ?? Data.GetScatterPoints()[index].X,
            scalingFactor: ScaleX,
            offset: OffsetX
            );
    }

    double IDataSource.GetY(int index)
    {
        return _dataSource?.GetY(index) ?? Data.GetScatterPoints()[index].Y;
    }

    double IDataSource.GetYScaled(int index)
    {
        return DataSourceUtilities.ScaleXY(
            value: _dataSource?.GetY(index) ?? Data.GetScatterPoints()[index].Y,
            scalingFactor: ScaleY,
            offset: OffsetY
            );
    }
    #endregion
}
