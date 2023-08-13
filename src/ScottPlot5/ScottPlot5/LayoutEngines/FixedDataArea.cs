namespace ScottPlot.LayoutEngines;

/// <summary>
/// Generate a layout using a fixed rectangle for the data area
/// </summary>
public class FixedDataArea : ILayoutEngine
{
    private PixelRect DataRect { get; }

    public FixedDataArea(PixelRect dataRect)
    {
        DataRect = dataRect;
    }

    private void CalculateOffsets(IEnumerable<IPanel> panels, Dictionary<IPanel, float> sizes, Dictionary<IPanel, float> offsets)
    {
        float offset = 0;
        foreach (IPanel panel in panels)
        {
            offsets[panel] = offset;
            offset += sizes[panel];
        }
    }

    private Dictionary<IPanel, float> MeasurePanels(IEnumerable<IPanel> panels)
    {
        return panels.ToDictionary(x => x, y => y.Measure());
    }

    private Dictionary<IPanel, float> GetPanelOffsets(IEnumerable<IPanel> panels, Dictionary<IPanel, float> panelSizes)
    {
        Dictionary<IPanel, float> panelOffsets = new();
        CalculateOffsets(panels.Where(x => x.Edge == Edge.Left), panelSizes, panelOffsets);
        CalculateOffsets(panels.Where(x => x.Edge == Edge.Right), panelSizes, panelOffsets);
        CalculateOffsets(panels.Where(x => x.Edge == Edge.Bottom), panelSizes, panelOffsets);
        CalculateOffsets(panels.Where(x => x.Edge == Edge.Top), panelSizes, panelOffsets);
        return panelOffsets;
    }

    public Layout GetLayout(PixelRect figureRect, IEnumerable<IPanel> panels)
    {
        Dictionary<IPanel, float> panelSizes = MeasurePanels(panels);
        Dictionary<IPanel, float> panelOffsets = GetPanelOffsets(panels, panelSizes);
        return new Layout(figureRect, DataRect, panelSizes, panelOffsets);
    }
}
