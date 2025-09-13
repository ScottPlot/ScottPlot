namespace ScottPlot;

public class Multiplot : IMultiplot
{
    public MultiplotSharedAxisManager SharedAxes { get; } = new();
    public SubplotCollection Subplots { get; } = new();
    public MultiplotLayoutSnapshot LastRender { get; } = new();
    private readonly List<IShareableManager> _shareableManagers = new();
    public IEnumerable<IShareableManager> ShareableManagers => _shareableManagers;
    public IMultiplotLayout Layout { get; set; } = new MultiplotLayouts.Rows();

    public Multiplot() : this(new Plot()) { }
    public Multiplot(Plot plot) { this.AddPlot(plot); }

    public void Add(IShareableManager mgr)
    {
        _shareableManagers.Add(mgr);
    }
    public bool Remove(IShareableManager mgr)
    {
        return _shareableManagers.Remove(mgr);
    }

    public void Render(SKCanvas canvas, PixelRect figureRect)
    {
        SharedAxes.UpdateSharedPlotAxisLimits();

        foreach (IShareableManager mgr in ShareableManagers)
        {
            mgr.Update();
        }

        canvas.Clear();

        PixelRect[] subplotRectangles = Layout.GetSubplotRectangles(Subplots, figureRect);

        for (int i = 0; i < Subplots.Count; i++)
        {
            Plot plot = Subplots.GetPlot(i);
            plot.RenderManager.ClearCanvasBeforeEachRender = false;
            plot.Render(canvas, subplotRectangles[i]);
            LastRender.Remember(plot, subplotRectangles[i]);
        }
    }
}
