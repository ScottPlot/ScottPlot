namespace ScottPlot;

public class SharedAxisManager
{
    private readonly List<IPlotControl> PlotControls = new();
    public readonly bool ShareX;
    public readonly bool ShareY;

    public SharedAxisManager(bool shareX, bool shareY)
    {
        ShareX = shareX;
        ShareY = shareY;
    }

    public void Add(IPlotControl plotControl)
    {
        PlotControls.Add(plotControl);

        plotControl.Plot.RenderManager.RenderFinished += (s, e) =>
        {
            UpdateOtherControls(plotControl);
        };
    }

    public void UpdateOtherControls(IPlotControl parentControl)
    {
        foreach (var childControl in PlotControls)
        {
            AxisLimits childLimitsBefore = childControl.Plot.GetAxisLimits();
            childControl.Plot.MatchAxisLimits(parentControl.Plot, ShareX, ShareY);
            AxisLimits childLimitsAfter = childControl.Plot.GetAxisLimits();
            bool childLimitsChanged = childLimitsBefore != childLimitsAfter;

            if (childLimitsChanged)
            {
                parentControl.RenderQueue.Enqueue(childControl);
            }
        }
    }
}
