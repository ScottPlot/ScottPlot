using ScottPlot.DataSources;

namespace ScottPlot.Plottables;

[Obsolete("SignalConst has been deprecated, " +
    "but its functionality may be achieved by creating a Signal plot with a SignalConstSource data source. " +
    "See the Add.SignalConst() method for reference.", true)]
public class SignalConst<T> : Signal, IPlottable, IHasLine, IHasMarker, IHasLegendText
    where T : struct, IComparable
{
    public SignalConst(SignalConstSource<T> data) : base(data)
    {
    }

    /// <summary>
    /// Setting this flag causes lines to be drawn between every visible point
    /// (similar to scatter plots) to improve anti-aliasing in static images.
    /// Setting this will decrease performance for large datasets 
    /// and is not recommended for interactive environments.
    /// </summary>
    public bool AlwaysUseLowDensityMode { get; set; } = false;

    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = ScottPlot.Axes.Default;

    public IEnumerable<LegendItem> LegendItems => LegendItem.Single(this,LegendText, MarkerStyle, LineStyle);

    public AxisLimits GetAxisLimits() => Data.GetAxisLimits();

    public virtual void Render(RenderPack rp)
    {
    }
}

