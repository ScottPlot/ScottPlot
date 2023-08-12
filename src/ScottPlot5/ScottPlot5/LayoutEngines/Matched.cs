namespace ScottPlot.LayoutEngines;

/// <summary>
/// Generate layouts that match layouts of another control
/// </summary>
internal class Matched : ILayoutEngine
{
    private Plot ReferencePlot { get; }

    public Matched(Plot referencePlot)
    {
        ReferencePlot = referencePlot;
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
        IEnumerable<IPanel> leftPanels = panels.Where(x => x.Edge == Edge.Left);
        IEnumerable<IPanel> rightPanels = panels.Where(x => x.Edge == Edge.Right);
        IEnumerable<IPanel> bottomPanels = panels.Where(x => x.Edge == Edge.Bottom);
        IEnumerable<IPanel> topPanels = panels.Where(x => x.Edge == Edge.Top);

        Dictionary<IPanel, float> panelOffsets = new();
        CalculateOffsets(leftPanels, panelSizes, panelOffsets);
        CalculateOffsets(rightPanels, panelSizes, panelOffsets);
        CalculateOffsets(bottomPanels, panelSizes, panelOffsets);
        CalculateOffsets(topPanels, panelSizes, panelOffsets);

        return panelOffsets;
    }

    public Layout GetLayout(PixelSize figureSize, IEnumerable<IPanel> panels)
    {
        Dictionary<IPanel, float> panelSizes = MeasurePanels(panels);
        Dictionary<IPanel, float> panelOffsets = GetPanelOffsets(panels, panelSizes);

        PixelRect dataRect = ReferencePlot.RenderManager.LastRender.DataRect;
        return new Layout(figureSize, dataRect, panelSizes, panelOffsets);
    }
}
