namespace ScottPlot;

public class MultiplotSharedAxisManager
{
    // TODO: improve support for plots with non-standard axis limits
    private readonly List<Plot> PlotsWithSharedX = [];
    private readonly List<Plot> PlotsWithSharedY = [];

    /// <summary>
    /// Link horizontal axis limits of the given collection of plots
    /// so when one changes they all change in unison
    /// </summary>
    public void ShareX(IEnumerable<Plot> plots)
    {
        PlotsWithSharedX.Clear();
        PlotsWithSharedX.AddRange(plots);

        // reset remembered axis limits to force realignment on the next render
        foreach (Plot plot in plots)
        {
            plot.RenderManager.ForgetLastRender();
        }
    }

    /// <summary>
    /// Link vertical axis limits of the given collection of plots
    /// so when one changes they all change in unison
    /// </summary>
    public void ShareY(IEnumerable<Plot> plots)
    {
        PlotsWithSharedY.Clear();
        PlotsWithSharedY.AddRange(plots);

        // reset remembered axis limits to force realignment on the next render
        foreach (Plot plot in plots)
        {
            plot.RenderManager.ForgetLastRender();
        }
    }

    public void UpdateSharedPlotAxisLimits()
    {
        Plot? parentPlotX = GetFirstPlotWithChangedLimitsX();
        if (parentPlotX is not null)
        {
            AxisLimits parentLimits = parentPlotX.Axes.GetLimits();
            PlotsWithSharedX.ForEach(x => x.Axes.SetLimitsX(parentLimits));
        }

        Plot? parentPlotY = GetFirstPlotWithChangedLimitsY();
        if (parentPlotY is not null)
        {
            AxisLimits parentLimits = parentPlotY.Axes.GetLimits();
            PlotsWithSharedY.ForEach(x => x.Axes.SetLimitsY(parentLimits));
        }
    }

    private Plot? GetFirstPlotWithChangedLimitsX()
    {
        foreach (var plot in PlotsWithSharedX)
        {
            var oldRange = plot.Axes.GetLimits().HorizontalRange;
            var newRange = plot.LastRender.AxisLimits.HorizontalRange;
            if (oldRange != newRange)
            {
                return plot;
            }
        }
        return null;
    }

    private Plot? GetFirstPlotWithChangedLimitsY()
    {
        foreach (var plot in PlotsWithSharedY)
        {
            var oldRange = plot.Axes.GetLimits().VerticalRange;
            var newRange = plot.LastRender.AxisLimits.VerticalRange;
            if (oldRange != newRange)
            {
                return plot;
            }
        }
        return null;
    }
}
