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
        IEnumerable<Coordinates> spokePts = RadialAxis.Spokes
            .Select(i => Coordinates.FromPolarCoordinates(i.Length, i.Angle));
        double radius = CircularAxis.Radii.Max();
        var limit = new AxisLimits(
                  Math.Min(spokePts.Min(i => i.X), -radius),
                  Math.Max(spokePts.Max(i => i.X), radius),
                  Math.Max(spokePts.Max(i => i.Y), radius),
                  Math.Min(spokePts.Min(i => i.Y), -radius));
        return limit;
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
