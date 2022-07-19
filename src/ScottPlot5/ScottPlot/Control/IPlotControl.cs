namespace ScottPlot.Control;

public interface IPlotControl
{
    Plot Plot { get; }
    void Refresh();
    public Backend<IPlotControl> Backend { get; }
    Coordinate MouseCoordinates { get; }
}
