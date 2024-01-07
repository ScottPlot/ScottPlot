namespace ScottPlot;

public interface IPlotMenu
{
    public void Clear();

    public void Add(string Label, Action<IPlotControl> action);

    void ShowContextMenu(Pixel pixel);
}
