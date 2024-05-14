namespace ScottPlot;

/// <summary>
/// Horizontal axis
/// </summary>
public interface IXAxis : IAxis
{
    public double Width { get; }

    public void SetTickets(double[] xs, string[] labels);
}
