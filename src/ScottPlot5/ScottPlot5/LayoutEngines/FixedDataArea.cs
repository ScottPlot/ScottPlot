namespace ScottPlot.LayoutEngines;

/// <summary>
/// Generate a layout using a fixed rectangle for the data area
/// </summary>
public class FixedDataArea(PixelRect dataRect) : LayoutEngineBase, ILayoutEngine
{
    private PixelRect DataRect { get; } = dataRect;

    public Layout GetLayout(PixelRect figureRect, Plot plot)
    {
        IEnumerable<IPanel> panels = plot.Axes.GetPanels();
        Dictionary<IPanel, float> panelSizes = LayoutEngineBase.MeasurePanels(panels);
        Dictionary<IPanel, float> panelOffsets = GetPanelOffsets(panels, panelSizes);
        return new Layout(figureRect, DataRect, panelSizes, panelOffsets);
    }
}
