/* Minimal case signal plot for testing only
 * !! Avoid temptation to use generics or generic math at this early stage of development
 */

using System.IO;

namespace ScottPlot.Plottables;

public class Signal : IPlottable
{
    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = new Axes();

    public readonly DataSources.ISignalSource Data;

    public MarkerStyle Marker { get; set; } = new(MarkerShape.FilledCircle, 5) { Outline = LineStyle.None };
    public string? Label { get; set; }

    public LineStyle LineStyle { get; } = new();

    public Signal(DataSources.ISignalSource data)
    {
        Data = data;
    }

    public AxisLimits GetAxisLimits() => Data.GetLimits();

    public IEnumerable<LegendItem> LegendItems => EnumerableExtensions.One(
        new LegendItem
        {
            Label = Label,
            Marker = Marker,
            Line = LineStyle,
        });

    /// <summary>
    /// Return Y data limits for pixel columns in the data area
    /// Pixel columns not overlapping with the signal are filtered,
    /// column indices in DataArea space are preserved.
    /// </summary>
    private PixelColumn[] GetVerticalBars()
    {
        double xUnitsPerPixel = Axes.XAxis.Width / Axes.DataRect.Width;

        var verticalBars = Enumerable.Range(0, (int)Axes.DataRect.Width)
            .Select(i =>
            {
                // determine how wide this column of pixels is in coordinate units
                float xPixel = i + Axes.DataRect.Left;
                double colX1 = Axes.GetCoordinateX(xPixel);
                double colX2 = colX1 + xUnitsPerPixel;
                CoordinateRange xRange = new(colX1, colX2);
                CoordinateRange yRange = Data.GetYRange(xRange);
                return (yRange, i);
            })
            .Where(vb => vb.yRange.HasBeenSet)
            .Select(vb =>
            {
                // determine how much vertical space the data of this pixel column occupies
                float yMin = Axes.GetPixelY(vb.yRange.Min);
                float yMax = Axes.GetPixelY(vb.yRange.Max);
                return (new PixelRangeY(yMin, yMax), vb.i);
            })
            .ToArray();

        PixelColumn[] cols = verticalBars
            .Select(vb => new PixelColumn(Axes.DataRect.Left + vb.i, vb.Item1.Bottom, vb.Item1.Top))
            .ToArray();

        return cols;
    }

    private CoordinateRange GetVisibleXRange(PixelRect dataRect)
    {
        // TODO: put GetRange in axis translator
        double xViewLeft = Axes.GetCoordinateX(dataRect.Left);
        double xViewRight = Axes.GetCoordinateX(dataRect.Right);
        return new CoordinateRange(xViewLeft, xViewRight);
    }

    private double PointsPerPixel()
    {
        return GetVisibleXRange(Axes.DataRect).Span / Axes.DataRect.Width / Data.Period;
    }

    public void Render(RenderPack rp)
    {
        if (PointsPerPixel() < 1)
        {
            RenderLowDensity(rp);
        }
        else
        {
            RenderHighDensity(rp);
        }
    }

    /// <summary>
    /// Renders each point connected by a single line, like a scatter plot.
    /// Call this when zoomed in enough that no pixel could contain two points.
    /// </summary>
    private void RenderLowDensity(RenderPack rp)
    {
        CoordinateRange visibleXRange = GetVisibleXRange(Axes.DataRect);
        int i1 = Data.GetIndex(visibleXRange.Min, true);
        int i2 = Data.GetIndex(visibleXRange.Max + Data.Period, true);

        IReadOnlyList<double> Ys = Data.GetYs();

        List<Pixel> points = new();
        for (int i = i1; i <= i2; i++)
        {
            float x = Axes.GetPixelX(Data.GetX(i));
            float y = Axes.GetPixelY(Ys[i] + Data.YOffset);
            Pixel px = new(x, y);
            points.Add(px);
        }

        using SKPath path = new();
        path.MoveTo(points[0].ToSKPoint());
        foreach (Pixel point in points)
            path.LineTo(point.ToSKPoint());

        using SKPaint paint = new();
        LineStyle.ApplyToPaint(paint);

        rp.Canvas.DrawPath(path, paint);

        double pointsPerPx = PointsPerPixel();

        if (pointsPerPx < 1)
        {
            paint.IsStroke = false;
            float radius = (float)Math.Min(Math.Sqrt(.2 / pointsPerPx), 4);
            Marker.Size = radius;
            Marker.Render(rp.Canvas, points);
        }
    }

    /// <summary>
    /// Renders the plot by filling-in pixel columns according the extremes of Y data ranges.
    /// Call this when zoomed out enough that one X pixel column may contain two or more points.
    /// </summary>
    private void RenderHighDensity(RenderPack rp)
    {
        using SKPaint paint = new();
        LineStyle.ApplyToPaint(paint);

        PixelColumn[] cols = GetVerticalBars();
        if (cols.Length == 0)
            return;

        using SKPath path = new();
        path.MoveTo(cols[0].X, cols[0].YBottom);

        foreach (PixelColumn col in cols)
        {
            if (col.YBottom == col.YTop)
            {
                // draw a single pixel
                path.LineTo(col.X, col.YBottom);
            }
            else
            {
                // draw a vertical line from bottom to top
                path.MoveTo(col.X, col.YBottom);
                path.LineTo(col.X, col.YTop);
            }
        }

        rp.Canvas.DrawPath(path, paint);
    }
}
