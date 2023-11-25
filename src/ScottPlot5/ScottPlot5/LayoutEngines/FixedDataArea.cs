namespace ScottPlot.LayoutEngines;

/// <summary>
/// Generate a layout using a fixed rectangle for the data area
/// </summary>
public class FixedDataArea : LayoutEngineBase, ILayoutEngine
{
    private PixelRect DataRect { get; }

    public FixedDataArea(PixelRect dataRect)
    {
        DataRect = dataRect;
    }

    public Layout GetLayout(PixelRect figureRect, IEnumerable<IPanel> panels)
    {
        Dictionary<IPanel, float> panelSizes = LayoutEngineBase.MeasurePanels(panels);
        Dictionary<IPanel, float> panelOffsets = GetPanelOffsets(panels, panelSizes);
        return new Layout(figureRect, DataRect, panelSizes, panelOffsets);
    }
}
