namespace ScottPlot.LayoutSystem;

public interface ILayoutSystem
{
    PixelRect GetDataAreaRect(PixelRect figureRect, IEnumerable<Axis.IXAxis> xAxes, IEnumerable<Axis.IYAxis> yAxes);
}
