using ScottPlot.Axis;
using System.Data;

namespace ScottPlot.Rendering;

/// <summary>
/// Common rendering tasks
/// </summary>
public static class Common
{
    public static void ReplaceNullAxesWithDefaults(RenderPack rp)
    {
        rp.Plot.ReplaceNullAxesWithDefaults();
    }

    public static void AutoAxisAnyUnsetAxes(RenderPack rp)
    {
        foreach (IPlottable plottable in rp.Plot.Plottables)
        {
            if (!plottable.Axes.XAxis.Range.HasBeenSet || !plottable.Axes.YAxis.Range.HasBeenSet)
            {
                rp.Plot.AutoScale(plottable.Axes.XAxis, plottable.Axes.YAxis);
            }
        }

        if (!rp.Plot.XAxis.Range.HasBeenSet) // may occur when there are no plottables with data
        {
            rp.Plot.SetAxisLimits(AxisLimits.Default);
        }
    }

    public static void RecalculateDataRect(RenderPack rp)
    {
        rp.CalculateLayout();
    }

    public static void RegnerateTicks(RenderPack rp)
    {
        rp.Plot.XAxis.TickGenerator.Regenerate(rp.Plot.XAxis.Range, rp.Plot.XAxis.Edge, rp.DataRect.Width);
        rp.Plot.YAxis.TickGenerator.Regenerate(rp.Plot.YAxis.Range, rp.Plot.YAxis.Edge, rp.DataRect.Height);
    }

    public static void EnsureAxesHaveArea(RenderPack rp)
    {
        foreach (CoordinateRange range in rp.Plot.GetAllAxes().Where(x => x.Range.Span == 0).Select(x => x.Range))
        {
            range.Min -= 1;
            range.Max += 1;
        }
    }

    public static void RenderBackground(RenderPack rp)
    {
        rp.Canvas.Clear(rp.Plot.FigureBackground.ToSKColor());

        using SKPaint paint = new() { Color = rp.Plot.DataBackground.ToSKColor() };
        rp.Canvas.DrawRect(rp.DataRect.ToSKRect(), paint);
    }

    public static void RenderGridsBelowPlottables(RenderPack rp)
    {
        foreach (IGrid grid in rp.Plot.Grids.Where(x => x.IsBeneathPlottables))
        {
            grid.Render(rp);
        }
    }

    public static void RenderGridsAbovePlottables(RenderPack rp)
    {
        foreach (IGrid grid in rp.Plot.Grids.Where(x => !x.IsBeneathPlottables))
        {
            grid.Render(rp);
        }
    }

    public static void RenderPlottables(RenderPack rp)
    {
        foreach (var plottable in rp.Plot.Plottables.Where(x => x.IsVisible))
        {
            plottable.Axes.DataRect = rp.DataRect;
            rp.Canvas.Save();

            if (plottable is IPlottableGL plottableGL)
            {
                plottableGL.Render(rp);
            }
            else
            {
                rp.Canvas.ClipRect(rp.DataRect.ToSKRect());
                plottable.Render(rp);
            }

            rp.Canvas.Restore();
        }
    }

    public static void RenderLegends(RenderPack rp)
    {
        LegendItem[] items = rp.Plot.Plottables.SelectMany(x => x.LegendItems).ToArray();

        foreach (ILegend legend in rp.Plot.Legends)
        {
            legend.Render(rp);
        }
    }

    public static void RenderPanels(RenderPack rp)
    {
        foreach (IPanel panel in rp.Plot.GetAllPanels())
        {
            float size = rp.Layout.PanelSizes[panel];
            float offset = rp.Layout.PanelOffsets[panel];
            panel.Render(rp, size, offset);
        }
    }

    public static void RenderZoomRectangle(RenderPack rp)
    {
        if (rp.Plot.ZoomRectangle.IsVisible)
        {
            rp.Plot.ZoomRectangle.Render(rp);
        }
    }

    public static void RenderBenchmark(RenderPack rp)
    {
        string message = $"Rendered in {rp.Elapsed.TotalMilliseconds:0.000} ms ({1e3 / rp.Elapsed.TotalMilliseconds:N0} FPS)";

        using SKPaint paint = new()
        {
            IsAntialias = true,
            Typeface = SKTypeface.FromFamilyName("consolas")
        };

        PixelSize textSize = Drawing.MeasureString(message, paint);
        float margin = 5;
        SKRect textRect = new(
            left: rp.DataRect.Left + margin,
            top: rp.DataRect.Bottom - paint.TextSize * .9f - 5 - margin,
            right: rp.DataRect.Left + 5 * 2 + textSize.Width + margin,
            bottom: rp.DataRect.Bottom - margin);

        paint.Color = SKColors.Yellow;
        paint.IsStroke = false;
        rp.Canvas.DrawRect(textRect, paint);

        paint.Color = SKColors.Black;
        paint.IsStroke = true;
        rp.Canvas.DrawRect(textRect, paint);

        paint.Color = SKColors.Black;
        paint.IsStroke = false;
        rp.Canvas.DrawText(
            text: message,
            x: rp.DataRect.Left + 4 + margin,
            y: rp.DataRect.Bottom - 4 - margin,
            paint: paint);
    }

    public static void SyncGLPlottables(RenderPack rp)
    {
        var glPlottables = rp.Plot.Plottables.OfType<IPlottableGL>();
        if (glPlottables.Any())
            glPlottables.First().GLFinish();
    }
}
