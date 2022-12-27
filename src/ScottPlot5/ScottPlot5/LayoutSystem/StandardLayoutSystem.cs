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

        FinalLayout layout = GetPanelPositionsAndPadding(figureRect, panels);

        return layout;
    }

    private FinalLayout GetPanelPositionsAndPadding(PixelRect figureRect, IEnumerable<IPanel> panels)
    {
        PixelPadding panelSizeByEdge = new();
        List<PanelWithOffset> offsets = new();

        var xPanels = panels.Where(p => p.IsHorizontal());
        var yPanels = panels.Where(p => p.IsVertical());

        float bottomOffset = 0;
        foreach (var panel in xPanels.Where(x => x.Edge == Edge.Bottom))
        {
            var pxHeight = panel.Measure();

            panelSizeByEdge.Bottom += pxHeight;

            offsets.Add(new(panel, new(0, bottomOffset)));
            bottomOffset += pxHeight;
        }

        float topOffset = 0;
        foreach (var panel in xPanels.Where(x => x.Edge == Edge.Top))
        {
            var pxHeight = panel.Measure();

            panelSizeByEdge.Top += pxHeight;

            offsets.Add(new(panel, new(0, topOffset)));
            topOffset -= pxHeight;
        }

        float leftOffset = 0;
        foreach (var panel in yPanels.Where(x => x.Edge == Edge.Left))
        {
            var pxWidth = panel.Measure();

            panelSizeByEdge.Left += pxWidth;

            offsets.Add(new(panel, new(leftOffset, 0)));
            leftOffset -= pxWidth;
        }

        float rightOffset = 0;
        foreach (var panel in yPanels.Where(x => x.Edge == Edge.Right))
        {
            var pxWidth = panel.Measure();

            panelSizeByEdge.Right += pxWidth;

            offsets.Add(new(panel, new(rightOffset, 0)));
            rightOffset += pxWidth;
        }

        // TODO: manual reduction remove this
        panelSizeByEdge.Left += 20;
        panelSizeByEdge.Bottom += 20;
        panelSizeByEdge.Right += 20;
        panelSizeByEdge.Top += 20;

        return new(figureRect.Contract(panelSizeByEdge), offsets);
    }
}
