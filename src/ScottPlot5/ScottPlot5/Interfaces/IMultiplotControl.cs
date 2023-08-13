namespace ScottPlot;

public interface IMultiplotControl
{
    Multiplot Multiplot { get; }
    void Refresh();
}
