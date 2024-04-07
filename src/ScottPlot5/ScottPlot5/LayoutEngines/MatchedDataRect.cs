namespace ScottPlot.LayoutEngines;

/// <summary>
/// Generate layouts that match layouts of another control
/// </summary>
public class MatchedDataRect(Plot referencePlot) : LayoutEngineBase, ILayoutEngine
{
    private Plot ReferencePlot { get; } = referencePlot;

    public Layout GetLayout(PixelRect figureRect, Plot plot)
    {
        IEnumerable<IPanel> panels = plot.Axes.GetPanels();
        Dictionary<IPanel, float> panelSizes = LayoutEngineBase.MeasurePanels(panels);
        Dictionary<IPanel, float> panelOffsets = GetPanelOffsets(panels, panelSizes);

        PixelRect dataRect = ReferencePlot.RenderManager.LastRender.DataRect;
        return new Layout(figureRect, dataRect, panelSizes, panelOffsets);
    }
}
