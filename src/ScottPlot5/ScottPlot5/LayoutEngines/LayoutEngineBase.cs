namespace ScottPlot.LayoutEngines;

public class LayoutEngineBase
{
    internal static Dictionary<IPanel, float> MeasurePanels(IEnumerable<IPanel> panels)
    {
        Dictionary<IPanel, float> measuredPanels = new();

        foreach (IPanel panel in panels)
        {
            measuredPanels[panel] = panel.MinimumSize == panel.MaximumSize
                ? panel.MinimumSize
                : NumericConversion.Clamp(panel.Measure(), panel.MinimumSize, panel.MaximumSize);
        }

        return measuredPanels;
    }

    internal static void CalculateOffsets(IEnumerable<IPanel> panels, Dictionary<IPanel, float> sizes, Dictionary<IPanel, float> offsets)
    {
        float offset = 0;
        foreach (IPanel panel in panels)
        {
            offsets[panel] = offset;
            offset += sizes[panel];
        }
    }

    internal static Dictionary<IPanel, float> GetPanelOffsets(IEnumerable<IPanel> panels, Dictionary<IPanel, float> panelSizes)
    {
        Dictionary<IPanel, float> panelOffsets = new();

        CalculateOffsets(panels.Where(x => x.Edge == Edge.Left), panelSizes, panelOffsets);
        CalculateOffsets(panels.Where(x => x.Edge == Edge.Right), panelSizes, panelOffsets);
        CalculateOffsets(panels.Where(x => x.Edge == Edge.Bottom), panelSizes, panelOffsets);
        CalculateOffsets(panels.Where(x => x.Edge == Edge.Top), panelSizes, panelOffsets);

        return panelOffsets;
    }
}
