namespace ScottPlot.Axis;

public interface IYAxis : IAxis
{
    public double Height { get; }
    public double Bottom { get; set; }
    public double Top { get; set; }
}
