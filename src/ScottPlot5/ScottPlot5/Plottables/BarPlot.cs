namespace ScottPlot.Plottables;

/// <summary>
/// Holds a collection of individually styled bars
/// </summary>
public class BarPlot : IPlottable
{
    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = new Axes();
    public string? Label { get; set; }
    public IEnumerable<Bar> Bars { get; set; }

    public BarPlot(Bar bar)
    {
        Bars = new Bar[] { bar };
    }

    public BarPlot(IEnumerable<Bar> bars)
    {
        Bars = bars;
    }

    public IEnumerable<LegendItem> LegendItems => LegendItem.None;

    public AxisLimits GetAxisLimits()
    {
        ExpandingAxisLimits limits = new();

        foreach (Bar bar in Bars)
        {
            limits.Expand(bar.AxisLimits);
        }

        return limits.AxisLimits;
    }

    public void Render(RenderPack rp)
    {
        using var paint = new SKPaint();

        foreach (Bar bar in Bars)
        {
            bar.Render(rp, Axes, paint);
        }
    }
}
