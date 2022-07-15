namespace ScottPlot.Axes;

public interface IYAxis : IAxis
{
    public double Bottom { get; set; }
    public double Top { get; set; }
    public double Height { get; }
}
