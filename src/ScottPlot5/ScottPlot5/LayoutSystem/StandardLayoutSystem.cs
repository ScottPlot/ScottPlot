using ScottPlot.Axis;
using System.Security.Cryptography;

namespace ScottPlot.LayoutSystem;

public class StandardLayoutSystem : ILayoutSystem
{
    public FinalLayout GetLayout(PixelRect figureRect, IEnumerable<IXAxis> xAxes, IEnumerable<IYAxis> yAxes, IEnumerable<IPanel> otherPanels)
    {
        RegenerateTicksForAllAxes(figureRect, xAxes, yAxes);

        var panels = xAxes
            .Cast<IPanel>()
            .Concat(yAxes)
            .Concat(otherPanels)
            .ToArray(); // It's probably worth reifying this given the number of times we iterate over it.

        return GetPanelPositionsAndPadding(figureRect, panels);
    }

    private void RegenerateTicksForAllAxes(PixelRect figureRect, IEnumerable<IXAxis> xAxes, IEnumerable<IYAxis> yAxes)
    {
        foreach (IXAxis xAxis in xAxes)
        {
            xAxis.TickGenerator.Regenerate(xAxis.Range, figureRect.Width);
        }
        foreach (IYAxis yAxis in yAxes)
        {
            yAxis.TickGenerator.Regenerate(yAxis.Range, figureRect.Width);
        }
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
