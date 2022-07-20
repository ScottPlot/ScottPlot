namespace ScottPlot.Control;

public interface IPlotControl
{
    Plot Plot { get; }
    void Refresh();
    public Backend Backend { get; }
    Coordinates MouseCoordinates { get; }
}
