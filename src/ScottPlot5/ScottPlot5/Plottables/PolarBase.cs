namespace ScottPlot.Plottables;

public abstract class PolarBase
    : IPlottable, IManagesAxisLimits
{
    public bool IsVisible { get; set; } = true;

    public IAxes Axes { get; set; } = new Axes();

    public abstract IEnumerable<LegendItem> LegendItems { get; }

    public abstract IRadialAxis RadialAxis { get; }
    public abstract ICircularAxis CircularAxis { get; }

    public bool ManageAxisLimits { get; set; } = true;

    public virtual AxisLimits GetAxisLimits()
    {
        var radialLimit = RadialAxis.GetAxisLimits();
        var circularLimit = CircularAxis.GetAxisLimits();
        return new AxisLimits(
            Math.Min(radialLimit.Left, circularLimit.Left),
            Math.Max(radialLimit.Right, circularLimit.Right),
            Math.Max(radialLimit.Bottom, circularLimit.Bottom),
            Math.Min(radialLimit.Top, circularLimit.Top));
    }

    public virtual void UpdateAxisLimits(Plot plot)
    {
        // Square grid need to render data, so implement IManagesAxisLimits
        plot.Axes.Rules.Add(new AxisRules.SquareZoomOut(Axes.XAxis, Axes.YAxis));
    }

    public virtual void Render(RenderPack rp)
    {
        CircularAxis.Render(rp, Axes);
        RadialAxis.Render(rp, Axes);
    }
}
