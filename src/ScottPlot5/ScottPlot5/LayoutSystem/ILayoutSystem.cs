using ScottPlot.Axis;

namespace ScottPlot.LayoutSystem;

public interface ILayoutSystem
{
    public FinalLayout GetLayout(PixelRect figureRect, IEnumerable<IXAxis> xAxes, IEnumerable<IYAxis> yAxes, IEnumerable<IPanel> otherPanels);
}
