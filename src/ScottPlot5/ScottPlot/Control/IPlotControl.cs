namespace ScottPlot.Control;

public interface IPlotControl
{
    Plot Plot { get; }
    void Refresh();
}
