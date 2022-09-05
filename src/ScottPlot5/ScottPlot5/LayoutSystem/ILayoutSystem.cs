namespace ScottPlot.LayoutSystem;

public interface ILayoutSystem
{
    PixelRect AutoSizeDataArea(PixelRect figureRect, IEnumerable<Axis.IXAxis> xAxes, IEnumerable<Axis.IYAxis> yAxes);
}
