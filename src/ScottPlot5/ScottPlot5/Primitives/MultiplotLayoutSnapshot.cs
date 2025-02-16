namespace ScottPlot;

public class MultiplotLayoutSnapshot
{
    record struct SubplotLayoutSnapshot(AxisLimits Limits, PixelRect Rect);

    readonly Dictionary<Plot, SubplotLayoutSnapshot> Shapshots = [];

    public void Remember(Plot plot, PixelRect subplotRect)
    {
        // TODO: improve support for non-primary axis limits
        var axisLimits = plot.RenderManager.LastRender.AxisLimits;

        Shapshots[plot] = new(axisLimits, subplotRect);
    }

    public PixelRect? GetLastRenderRectangle(Plot plot)
    {
        if (Shapshots.TryGetValue(plot, out SubplotLayoutSnapshot snapshot))
        {
            return snapshot.Rect;
        }
        else
        {
            return null;
        }
    }

    public void Forget(Plot plot)
    {
        Shapshots.Remove(plot);
    }

    public void Reset()
    {
        Shapshots.Clear();
    }

    /// <summary>
    /// Return the plot beneath the given pixel according to the last render.
    /// Returns null if no render occurred or the pixel is not over a plot.
    /// </summary>
    public Plot? GetPlotAtPixel(Pixel pixel)
    {
        foreach (var entry in Shapshots)
        {
            if (entry.Value.Rect.Contains(pixel))
            {
                return entry.Key;
            }
        }

        return null;
    }
}
