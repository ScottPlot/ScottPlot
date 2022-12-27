using ScottPlot.Axis;

namespace ScottPlot.LayoutSystem;

public class StandardLayoutSystem : ILayoutSystem
{
    public FinalLayout GetLayout(PixelRect figureRect, IEnumerable<IPanel> panels)
    {
        // regenerate ticks using the figure are (not the data area)
        // to create a first-pass estimate of the space needed for axis panels

        panels.OfType<IXAxis>()
            .ToList()
            .ForEach(xAxis => xAxis.TickGenerator.Regenerate(xAxis.Range, figureRect.Width));

        panels.OfType<IYAxis>()
            .ToList()
            .ForEach(yAxis => yAxis.TickGenerator.Regenerate(yAxis.Range, figureRect.Height));

        FinalLayout layout = MakeFinalLayout(figureRect, panels);

        return layout;
    }

    private FinalLayout MakeFinalLayout(PixelRect figureRect, IEnumerable<IPanel> panels)
    {
        // Plot edges with visible axes require padding between the figure rectangle and data rectangle.
        // Panels have size and increase the amount of padding needed.
        // This function iterates all panels and records the total padding needed and offset of each panel.

        PixelPadding dataRectPadding = new();

        Dictionary<IPanel, float> panelOffset = new();

        float bottomOffset = 0;
        foreach (var panel in panels.Where(x => x.Edge == Edge.Bottom))
        {
            float pxHeight = panel.Measure();
            dataRectPadding.Bottom += pxHeight;
            panelOffset[panel] = bottomOffset;
            bottomOffset += pxHeight;
        }

        float topOffset = 0;
        foreach (var panel in panels.Where(x => x.Edge == Edge.Top))
        {
            float pxHeight = panel.Measure();
            dataRectPadding.Top += pxHeight;
            panelOffset[panel] = topOffset;
            topOffset -= pxHeight;
        }

        float leftOffset = 0;
        foreach (var panel in panels.Where(x => x.Edge == Edge.Left))
        {
            float pxWidth = panel.Measure();
            dataRectPadding.Left += pxWidth;
            panelOffset[panel] = leftOffset;
            leftOffset -= pxWidth;
        }

        float rightOffset = 0;
        foreach (var panel in panels.Where(x => x.Edge == Edge.Right))
        {
            float pxWidth = panel.Measure();
            dataRectPadding.Right += pxWidth;
            panelOffset[panel] = rightOffset;
            rightOffset += pxWidth;
        }

        // Add a little extra padding around the perimeter of the figure so axes don't touch the edge of the image.
        // NOTE: This must be set to zero for frameless plots.
        dataRectPadding.Expand(20);

        PixelRect dataArea = figureRect.Contract(dataRectPadding);

        FinalLayout layout = new(figureRect, dataArea, panelOffset);

        return layout;
    }
}
