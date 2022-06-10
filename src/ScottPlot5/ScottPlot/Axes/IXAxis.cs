namespace ScottPlot.Axes;

public interface IXAxis : IAxis
{
    public double Left { get; set; }
    public double Right { get; set; }
    public double Width { get; }
}
