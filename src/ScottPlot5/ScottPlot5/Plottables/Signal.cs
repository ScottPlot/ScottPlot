/* Minimal case signal plot for testing only
 * 
 * !! Avoid temptation to use generics or generic math at this early stage of development
 * 
 */

using ScottPlot.Axis;
using SkiaSharp;
using System.Data;

namespace ScottPlot.Plottables;

public class Signal : IPlottable
{
    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = Axis.Axes.Default;

    public readonly DataSource.ISignalSource Data;

    public Color Color = new(0, 0, 255);
    public float LineWidth = 1;

    public Signal(DataSource.ISignalSource data)
    {
        Data = data;
    }

    public AxisLimits GetAxisLimits() => Data.GetLimits();

    /// <summary>
    /// Return Y data limits for each pixel column in the data area
    /// </summary>
    private PixelRangeY[] GetVerticalBars()
    {
        PixelRangeY[] verticalBars = new PixelRangeY[(int)Axes.DataRect.Width];

        double xUnitsPerPixel = Axes.XAxis.Width / Axes.DataRect.Width;

        // for each vertical column of pixels in the data area
        for (int i = 0; i < verticalBars.Length; i++)
        {
            // determine how wide this column of pixels is in coordinate units
            float xPixel = i + Axes.DataRect.Left;
            double colX1 = Axes.GetCoordinateX(xPixel);
            double colX2 = colX1 + xUnitsPerPixel;
            CoordinateRange xRange = new(colX1, colX2);

            // determine how much vertical space the data of this pixel column occupies
            CoordinateRange yRange = Data.GetYRange(xRange);
            float yMin = Axes.GetPixelY(yRange.Min);
            float yMax = Axes.GetPixelY(yRange.Max);
            verticalBars[i] = new PixelRangeY(yMin, yMax);
        }

        return verticalBars;
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

    public void Render(SKSurface surface)
    {
        if (PointsPerPixel() < 1)
        {
            RenderLowDensity(surface);
        }
        else
        {
            RenderHighDensity(surface);
        }
    }

    /// <summary>
    /// Renders each point connected by a single line, like a scatter plot.
    /// Call this when zoomed in enough that no pixel could contain two points.
    /// </summary>
    private void RenderLowDensity(SKSurface surface)
    {
        CoordinateRange visibleXRange = GetVisibleXRange(Axes.DataRect);
        int i1 = Data.GetIndex(visibleXRange.Min, true);
        int i2 = Data.GetIndex(visibleXRange.Max + Data.Period, true);

        IReadOnlyList<double> Ys = Data.GetYs();

        List<SKPoint> points = new();
        for (int i = i1; i <= i2; i++)
        {
            float x = Axes.GetPixelX(Data.GetX(i));
            float y = Axes.GetPixelY(Ys[i]);
            Pixel px = new(x, y);
            points.Add(px.ToSKPoint());
        }

        using SKPath path = new();
        path.MoveTo(points[0]);
        foreach (SKPoint point in points)
            path.LineTo(point);

        using SKPaint paint = new()
        {
            IsAntialias = true,
            Style = SKPaintStyle.Stroke,
            Color = Color.ToSKColor(),
            StrokeWidth = LineWidth,
        };

        surface.Canvas.DrawPath(path, paint);

        double pointsPerPx = PointsPerPixel();

        if (pointsPerPx < 1)
        {
            paint.IsStroke = false;
            float radius = (float)Math.Min(Math.Sqrt(.2 / pointsPerPx), 4);
            foreach (SKPoint pt in points)
            {
                surface.Canvas.DrawCircle(pt, radius, paint);
            }
        }
    }

    /// <summary>
    /// Renders the plot by filling-in pixel columns according the extremes of Y data ranges.
    /// Call this when zoomed out enough that one X pixel column may contain two or more points.
    /// </summary>
    private void RenderHighDensity(SKSurface surface)
    {
        using SKPaint paint = new()
        {
            IsAntialias = true,
            Style = SKPaintStyle.Stroke,
            Color = Color.ToSKColor(),
            StrokeWidth = LineWidth,
        };

        PixelRangeY[] verticalBars = GetVerticalBars();
        for (int i = 0; i < verticalBars.Length; i++)
        {
            float x = Axes.DataRect.Left + i;
            surface.Canvas.DrawLine(x, verticalBars[i].Bottom, x, verticalBars[i].Top, paint);
        }
    }
}
