using ScottPlot.AxisPanels;

namespace ScottPlot.LayoutEngines;

/// <summary>
/// Generate the layout by measuring all panels and adding
/// enough padding around the data area to fit them all exactly.
/// </summary>
public class Automatic : LayoutEngineBase, ILayoutEngine
{
    public float SizeForAxisPanelsWithoutData = 10;

    public Layout GetLayout(PixelRect figureRect, Plot plot)
    {
        /* PROBLEM: There is a chicken-or-egg situation
         * where the ideal layout depends on the ticks,
         * but the ticks depend on the layout.
         * 
         * SOLUTION: Regenerate ticks using the figure area (not the data area)
         * to create a first-pass estimate of the space needed for axis panels.
         * Ticks require recalculation once more after the axes are repositioned
         * according to the layout determined by this function.
         * 
         */

        IEnumerable<IPanel> panels = plot.Axes.GetPanels();

        // NOTE: the actual ticks will be regenerated later, after the layout is determined
        panels.OfType<IXAxis>().ToList().ForEach(x => x.RegenerateTicks(figureRect.Width));
        panels.OfType<IYAxis>().ToList().ForEach(x => x.RegenerateTicks(figureRect.Height));

        Dictionary<IPanel, float> panelSizes = LayoutEngineBase.MeasurePanels(panels);
        Dictionary<IPanel, float> panelOffsets = GetPanelOffsets(panels, panelSizes);

        PixelPadding paddingNeededForPanels = new(
            left: panels.Where(x => x.Edge == Edge.Left).Select(x => panelSizes[x]).Sum(),
            right: panels.Where(x => x.Edge == Edge.Right).Select(x => panelSizes[x]).Sum(),
            bottom: panels.Where(x => x.Edge == Edge.Bottom).Select(x => panelSizes[x]).Sum(),
            top: panels.Where(x => x.Edge == Edge.Top).Select(x => panelSizes[x]).Sum());

        float dataRectWidth = Math.Max(0, figureRect.Width - paddingNeededForPanels.Horizontal);
        float dataRectHeight = Math.Max(0, figureRect.Height - paddingNeededForPanels.Vertical);
        PixelSize dataRectSize = new(dataRectWidth, dataRectHeight);
        Pixel dataRectLocation = new(paddingNeededForPanels.Left, paddingNeededForPanels.Top);
        PixelRect dataRect = new PixelRect(dataRectLocation, dataRectSize)
            .WithPan(figureRect.Left, figureRect.Top);

        return new Layout(figureRect, dataRect, panelSizes, panelOffsets);
    }
}
