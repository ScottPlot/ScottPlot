using ScottPlot;
using ScottPlot.WinForms;

namespace WinForms_Demo.Demos;

public class SharedLayoutManager
{
    private readonly List<FormsPlot> ChildControls = new();
    private FormsPlot? ParentControl = null;
    public readonly bool ShareX;
    public readonly bool ShareY;

    public SharedLayoutManager(bool shareX, bool shareY)
    {
        ShareX = shareX;
        ShareY = shareY;
    }

    public void Add(FormsPlot plotControl)
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

        foreach (var childControl in ChildControls)
        {
            childControl.Plot.FixedLayout(dataRect);
        }
    }
}
