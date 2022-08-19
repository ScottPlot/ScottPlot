namespace ScottPlot.Axis;

public interface IXAxis : IAxis
{
    public double Width { get; }
    public double Left { get; set; }
    public double Right { get; set; }
}
