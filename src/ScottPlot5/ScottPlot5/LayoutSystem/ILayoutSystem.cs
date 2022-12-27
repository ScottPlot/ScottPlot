using ScottPlot.Axis;

namespace ScottPlot.LayoutSystem;

public interface ILayoutSystem
{
    // TODO: don't separate axes from panels
    public FinalLayout GetLayout(PixelRect figureRect, IEnumerable<IPanel> panels);
}
