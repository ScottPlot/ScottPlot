namespace ScottPlot.LayoutSystem;

public interface ILayoutSystem
{
    FinalLayout GetLayout(PixelRect figureRect, IEnumerable<Axis.IXAxis> xAxes, IEnumerable<Axis.IYAxis> yAxes);
}
