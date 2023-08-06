using ScottPlot.Axis;

namespace ScottPlot.Layouts;

public class AutomaticLayoutMaker : ILayoutMaker
{
    public Layout GetLayout(PixelSize figureSize, IEnumerable<IPanel> panels)
    {
        // Regenerate ticks using the figure area (not the data area)
        // to create a first-pass estimate of the space needed for axis panels.
        // Ticks require recalculation once more after the axes are repositioned
        // according to the layout determined by this function.

        panels.OfType<IXAxis>()
            .ToList()
            .ForEach(xAxis => xAxis.TickGenerator.Regenerate(xAxis.Range, xAxis.Edge, figureSize.Width));

        panels.OfType<IYAxis>()
            .ToList()
            .ForEach(yAxis => yAxis.TickGenerator.Regenerate(yAxis.Range, yAxis.Edge, figureSize.Height));

        Layout layout = MakeFinalLayout(figureSize, panels);

        return layout;
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

    private Layout MakeFinalLayout(PixelSize figureSize, IEnumerable<IPanel> panels)
    {
        Dictionary<IPanel, float> panelSizes = MeasurePanels(panels);
        Dictionary<IPanel, float> panelOffsets = GetPanelOffsets(panels, panelSizes);

        PixelPadding paddingNeededForPanels = new(
            left: panels.Where(x => x.Edge == Edge.Left).Select(x => panelSizes[x]).Sum(),
            right: panels.Where(x => x.Edge == Edge.Right).Select(x => panelSizes[x]).Sum(),
            bottom: panels.Where(x => x.Edge == Edge.Bottom).Select(x => panelSizes[x]).Sum(),
            top: panels.Where(x => x.Edge == Edge.Top).Select(x => panelSizes[x]).Sum());

        PixelRect dataRect = new(
            left: paddingNeededForPanels.Left,
            right: figureSize.Width - paddingNeededForPanels.Right,
            bottom: figureSize.Height - paddingNeededForPanels.Bottom,
            top: paddingNeededForPanels.Top);

        return new Layout(figureSize, dataRect, panelSizes, panelOffsets);
    }
}
