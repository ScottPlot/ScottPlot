namespace ScottPlot;

public class SharedLayoutManager
{
    private readonly List<IPlotControl> ChildControls = new();
    private IPlotControl? ParentControl = null;
    public readonly bool ShareX;
    public readonly bool ShareY;

    public SharedLayoutManager(bool shareX, bool shareY)
    {
        ShareX = shareX;
        ShareY = shareY;
    }

    public void Add(IPlotControl plotControl)
    {
        if (ParentControl is null)
        {
            ParentControl = plotControl;
            plotControl.Plot.RenderManager.RenderFinished += (s, e) =>
            {
                UpdateChildLayouts();
            };
        }
        else
        {
            ChildControls.Add(plotControl);
        }
    }

    public void UpdateChildLayouts()
    {
        if (ParentControl is null)
            return;

        PixelRect dataRect = ParentControl.Plot.RenderManager.LastRender.DataRect;

        foreach (IPlotControl childControl in ChildControls)
        {
            childControl.Plot.FixedLayout(dataRect);
        }
    }
}
